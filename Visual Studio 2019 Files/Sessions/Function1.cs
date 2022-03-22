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

namespace TaskSessions
{
    public static class Function1
    {
        [FunctionName("Session")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("HTTP Trigger function process request for Sessions.");

            if (req.Method == HttpMethods.Get)
            {
                //return a session
                log.LogInformation("get request for Session");
            }
            if (req.Method == HttpMethods.Post)
            {
                //create a new session
                // needs SessionID, UserID, StartDateTime, LastUsedDateTime
                log.LogInformation("Post request for Session");
                string strEmail = req.Query["strEmail"];
                string strPassword = req.Query["strPassword"];

                if (strEmail == null || strPassword == null)
                {
                    return new OkObjectResult("You must provide Email First Name, Last Name and Password to create a user.");
                }

                insert into tblCurrentSession values(sessionID,email,currentDATETime, currentDATETime) where(Select count(*) from tblUsers where TOUPPEr(email)==TOUPPER(strEmail) and Password= strPassword)
            }

            



            return new OkObjectResult("...");
        }
    }
}
