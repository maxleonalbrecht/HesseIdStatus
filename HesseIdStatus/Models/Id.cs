using System;
using System.Text.RegularExpressions;
using RestSharp;

namespace HesseIdStatus.Models
{
    public class Id
    {
        public string idIdentifier;

        public string idType;

        public string issuingCity;

        public IdStatus idStatus;
        

        public void UpdateStatus()
        {
            var ekomData = GetDataFromEkom(idIdentifier, idType, issuingCity);
            var parsedResponse = ParseApiResponse(ekomData);
            this.idStatus = EvaluateStatus(parsedResponse);
        }

        IRestResponse GetDataFromEkom(string idIdentifier, string idType, string issuingCity)
        {
            var apiEndpoint = "https://passauskunft.ekom21.de/mandant/" + issuingCity.ToLower() + "/default.aspx";

            var client = new RestClient(apiEndpoint);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Cookie", "AL_SESS-S=AeGHh0quPD4h_m938B6IpPA5Fo5BfyZ30hX3tJtahAc8hoew5PnG8mgx_EKGpvdStH1i");
            request.AddParameter("__VIEWSTATE", "/wEPDwUKMTUxNTQwNDE4OA9kFgICAQ9kFgICCQ8PFgIeBFRleHQFYERhcyBEb2t1bWVudCBpc3Qgbm9jaCBpbiBCZWFyYmVpdHVuZy4gQml0dGUgd2llZGVyaG9sZW4gU2llIElocmUgQW5mcmFnZSBpbiBkZW4gbsOkY2hzdGVuIFRhZ2VuLmRkGAEFHl9fQ29udHJvbHNSZXF1aXJlUG9zdEJhY2tLZXlfXxYDBQtyYlJlaXNlcGFzcwULcmJSZWlzZXBhc3MFEXJiUGVyc29uYWxhdXN3ZWlzxg/tVxzSgcJPfz9y8Sm4fB2cj9c+ZXTmXhZscwAOXJ0=");
            request.AddParameter("__VIEWSTATEGENERATOR", "A641E780");
            request.AddParameter("__EVENTVALIDATION", "/wEdAAXvH9qsrf5EDhqq6seFPg3QwX2oFirzAtw9CR3zslJ+Z+9rOlzTy71jQJpy437MuIzcnyTmcpfAsqgicwQ1ROT9W9T4hZhp/kyBVu0RRJpYLnMperKgR1i9JiIAo+aoiBhfmJS2SM7do1gCaPAhj/Gs");
            request.AddParameter("rbGroupPasstyp", idType);
            request.AddParameter("txtPassnr", idIdentifier);
            return (client.Execute(request));
        }

        string ParseApiResponse(IRestResponse restResponse)
        {
            var responseHtml = restResponse.Content.ToString();

            var regex = new Regex(@"lblMessage""\>(.*?)(<)");
            var currentStatus = regex.Matches(responseHtml);
            var parsedResponse = currentStatus[0].Groups[1].ToString();

            return parsedResponse;
        }

        IdStatus EvaluateStatus(string parsedResponse)
        {
            var currentStatus = new IdStatus();
            
            if (parsedResponse == "Das Dokument ist noch in Bearbeitung. Bitte wiederholen Sie Ihre Anfrage in den nächsten Tagen.")
            {
                currentStatus.inProduction = true;
                currentStatus.readyForPickup = false;
            }

            if (parsedResponse == "Das Dokument liegt zur Abholung bereit.")
            {
                currentStatus.inProduction = false;
                currentStatus.readyForPickup = true;
            }

            return currentStatus;

            //TODO - Implement Picked up
        }

        public string EvaluateType(string input)
        {
            string type = String.Empty;
            return type;
        }
    }
}
