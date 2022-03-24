using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data; //needed to access the built-in dataset functions
using System.Data.SqlClient;

namespace _0324DataIntegration
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string strFirstName = req.Query["strFirstName"];
            //creating the connection to the database
            string strConnection = @"Server=PCLABSQL01\COB_DS2,1436;Database=DS3870;User Id=student;Password=Mickey2020!;";
            string strQuery = "select * from dbo.tblUsers;";
            //string strQuery = "insert into dbo.tblUsers (email,firstname,lastname,status,password)values('lmcoleman@tntech.edu', 'Larenzle','Coleman','Active','apassword')";
            DataSet dsUsers = new DataSet();

            //
            using (SqlConnection conDS3870 = new SqlConnection(strConnection))
            using (SqlCommand comDS3870 = new SqlCommand(strQuery, conDS3870))
            {
                /*
                //manaually accessing the database
                comDS3870.Connection = conDS3870;
                    comDS3870.CommandText = strQuery;
                    conDS3870.Open();
                    comDS3870.ExecuteNonQuery();
                    conDS3870.Close();
                return new OkObjectResult("User Added");
                */


                ///*
                //used to prevent sql injections
                SqlParameter parFirstName = new SqlParameter("FirstName", SqlDbType.VarChar);
                parFirstName.Value = strFirstName;
                comDS3870.Parameters.Add(parFirstName);

                //used to fill the dataset
                SqlDataAdapter daDS3870 = new SqlDataAdapter(comDS3870);
                daDS3870.Fill(dsUsers);
                return new OkObjectResult(dsUsers.Tables[0]);//return Data Table back
                //*/

            }
                return new OkObjectResult("Tests");
        }
    }
}
