using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace HelloWorld
{
    public static class Function1
    {
        [FunctionName("Hello_World")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            List<string> lstAnimals = new List<string>();
                string[] arrAnimals = { "Pig", "Cow", "Hippo" };

            foreach(string strCurrentAnimal in arrAnimals)
            {
                lstAnimals.Add(strCurrentAnimal);
            }
            lstAnimals.Add("Chicken");

            string strAnimal = req.Query["strAnimal"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            strAnimal = strAnimal ?? data?.strAnimal;

            string strResponseMessage = " ";
            bool blnFound = false;

            if (strAnimal != "" && strAnimal != null){ 
                foreach(string strCurrentAnimal in lstAnimals)
                {
                    if (strCurrentAnimal == strAnimal)
                    {
                        blnFound = true;
                    }
                }
            }

            if (blnFound == true)
            {
                return new OkObjectResult("Animal Found");
            }
            else
            {
                return new OkObjectResult("Animal Not Found");
            }
        }
    }
}
 