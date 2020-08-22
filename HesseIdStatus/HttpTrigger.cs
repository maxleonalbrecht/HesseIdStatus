using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using HesseIdStatus.Models;

namespace HesseIdStatus
{
    public static class HesseIdStatus
    {
        [FunctionName("HesseIdStatus")]
        public static Id Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req)
        {
            var id = new Id()
            {
                idIdentifier = req.Query["IdIdentifier"],
                idType = req.Query["IdType"],
                issuingCity = req.Query["IssuingCity"]
            };

            if(id.idIdentifier == "")
            {
                throw new System.Exception("IdIdentifier is empty");
            }

            if (id.idType == "")
            {
                throw new System.Exception("IdType is empty");
            }

            if (id.issuingCity == "")
            {
                throw new System.Exception("IssuingCity is empty");
            }

            id.UpdateStatus();

            return id;
        }
    }
}
