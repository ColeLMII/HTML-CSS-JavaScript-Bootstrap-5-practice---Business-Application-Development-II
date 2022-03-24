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
using System.Data;
using System.Data.SqlClient;

namespace InClass_03222022
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            //connecting back to your database
            string strTasksConnectionString = @"Server=PCLABSQL01\COB_DS2,1436;Database=DS3870;User Id=student;Password=Mickey2020!;"; //use the @ symbol to use the statement as a literal
            string name = req.Query["name"];
            DataSet dsTasks = new DataSet(); //using Systems.Data;

            //building the connection using a try catch, to catch the errors
            try
            {
                string strQuery = "select * from dbo.tblUsers";
               
                using (SqlConnection conTasks= new SqlConnection(strTasksConnectionString))
                using (SqlCommand comTasks = new SqlCommand(strQuery, conTasks))
                {
                    //must create a data adapter, all you to fill a dataset
                    SqlDataAdapter daTasks = new SqlDataAdapter(comTasks);
                    daTasks.Fill(dsTasks);

                    return new OkObjectResult(dsTasks);
                }
            }
            catch(Exception Ex)
            {
                return new OkObjectResult(Ex.Message.ToString());
            }


          
            return new OkObjectResult("Test");
        }
    }
}
