using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using HesseIdStatus.Models;
using RestSharp;
using System.Text.RegularExpressions;

namespace HesseIdStatus
{
    public static class HesseIdStatus
    {
        [FunctionName("HesseIdStatus")]
        public static Id Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var id = new Id()
            {
                idIdentifier = req.Query["IdIdentifier"],
                idType = req.Query["IdType"],
                issuingCity = req.Query["IssuingCity"]
            };

            id.UpdateStatus();

            return id;

        }
    }
}
