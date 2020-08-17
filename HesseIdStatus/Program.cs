using RestSharp;
using System;
using System.Text.RegularExpressions;

namespace AusweisStatusChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            var idIdentifier = "<id>";
            var idType = "rbPersonalausweis";
            var issuingCity = "Frankfurt";

            //other possibilietes e.g. kassel, giessen, offenbach

            var restResponse = GetStatus(idIdentifier, idType, issuingCity);
            var parsedApiResponse = ParseApiResponse(restResponse);
            var isIdReadyForPickup = EvaluateStatus(parsedApiResponse);
          

            Console.WriteLine("Id is ready for pickup: " + isIdReadyForPickup);
            Console.ReadKey();

        }

        static string ParseApiResponse(IRestResponse restResponse)
        {
            var responseHtml = restResponse.Content.ToString();

            var regex = new Regex(@"lblMessage""\>(.*?)(<)");
            var currentStatus = regex.Matches(responseHtml);
            var parsedResponse = currentStatus[0].Groups[1].ToString();

            return parsedResponse;
        }

        static bool EvaluateStatus(string idStatus)
        {
            if (idStatus == "Das Dokument ist noch in Bearbeitung. Bitte wiederholen Sie Ihre Anfrage in den nächsten Tagen.")
            {
                return false;
            }

            if(idStatus == "Das Dokument liegt zur Abholung bereit.")
            {
                return true;
            }

            return false;
        }

        static IRestResponse GetStatus(string idIdentifier, string idType, string issuingCity)
        {
            var client = new RestClient("https://passauskunft.ekom21.de/mandant/frankfurt/default.aspx");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Connection", "keep-alive");
            request.AddHeader("Pragma", "no-cache");
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Upgrade-Insecure-Requests", "1");
            request.AddHeader("Origin", "https://passauskunft.ekom21.de");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4223.0 Safari/537.36 Edg/86.0.607.0";
            request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            request.AddHeader("Sec-Fetch-Site", "same-origin");
            request.AddHeader("Sec-Fetch-Mode", "navigate");
            request.AddHeader("Sec-Fetch-User", "?1");
            request.AddHeader("Sec-Fetch-Dest", "document");
            request.AddHeader("Referer", "https://passauskunft.ekom21.de/mandant/"+ issuingCity.ToLower() + "/default.aspx");
            request.AddHeader("Accept-Language", "en-US,en;q=0.9,de;q=0.8");
            request.AddHeader("Cookie", "AL_SESS-S=AWpCjQMRoLLbBHV5m5cOaw_FN3RSJAgrUdUaZVfvArDy47Jb96acF3zsIu3N5SQCcJZk");
            request.AddParameter("__VIEWSTATE", "/wEPDwUKMTUxNTQwNDE4OA9kFgICAQ9kFgICCQ8PFgIeBFRleHQFYERhcyBEb2t1bWVudCBpc3Qgbm9jaCBpbiBCZWFyYmVpdHVuZy4gQml0dGUgd2llZGVyaG9sZW4gU2llIElocmUgQW5mcmFnZSBpbiBkZW4gbsOkY2hzdGVuIFRhZ2VuLmRkGAEFHl9fQ29udHJvbHNSZXF1aXJlUG9zdEJhY2tLZXlfXxYDBQtyYlJlaXNlcGFzcwULcmJSZWlzZXBhc3MFEXJiUGVyc29uYWxhdXN3ZWlzxg/tVxzSgcJPfz9y8Sm4fB2cj9c+ZXTmXhZscwAOXJ0=");
            request.AddParameter("__VIEWSTATEGENERATOR", "A641E780");
            request.AddParameter("__EVENTVALIDATION", "/wEdAAXvH9qsrf5EDhqq6seFPg3QwX2oFirzAtw9CR3zslJ+Z+9rOlzTy71jQJpy437MuIzcnyTmcpfAsqgicwQ1ROT9W9T4hZhp/kyBVu0RRJpYLnMperKgR1i9JiIAo+aoiBhfmJS2SM7do1gCaPAhj/Gs");
            request.AddParameter("rbGroupPasstyp", idType);
            request.AddParameter("txtPassnr", idIdentifier);
            request.AddParameter("btnAbfragen", "Abfragen");
            return (client.Execute(request));
        }
    }
}
