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

namespace VehicleFinder
{
    public static class Function1
    {
        [FunctionName("Vehicle_Finder")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];
            string strVehicleModel = req.Query["strVehicleModel"];
            List<string> lstVehicles = new List<string>();
            string[] arrVehicles = { "1995 Dodle Neon", "1995 Subaru Outback", "1996 GMC Sonoma", "1996 Toyota Tacoma", "1998 Toyota Tacoma", "2000 Toyota Tundra", "2001 Toyota Tundra", "2002 Ford Ranger", "2002 Chevrolet Tracker", "2005 Toyota Highlander", "2006 Toyota Corolla", "2008 Honda Element", "2009 Subaru Forestor", "2010 Toyota Prius" };
            
            foreach (string strVehicle in arrVehicles)
            {
                lstVehicles.Add(strVehicle);
            }

            if (strVehicleModel != "" && strVehicleModel != null)
            {
                string foundVehicles = "";
                bool blnFound = false;

                foreach( string strCurrVehicle in lstVehicles)
                {
                    if (strCurrVehicle.Contains(strVehicleModel))
                    {
                        blnFound = true;
                        foundVehicles += strCurrVehicle + ", ";
                    }

                }
                if (blnFound == true)
                {
                    return new OkObjectResult(foundVehicles);
                }
                else
                {
                    return new OkObjectResult("Vehicle Not found inside Inventory.");
                }

            }
            else
            {
                return new OkObjectResult("You have failed to enter a parameter.");
            }

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }
    }
}
