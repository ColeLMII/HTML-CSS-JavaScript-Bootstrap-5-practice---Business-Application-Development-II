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
        [FunctionName("Users")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            string strConnection = @"Server=PCLABSQL01\COB_DS2,1436;Database=DS3870;User Id=student;Password=Mickey2020!;";
            if (req.Method == HttpMethods.Post)
            {
                string strFirstName = req.Query["FirstName"];
                string strLastName = req.Query["LastName"];
                string strEmail = req.Query["Email"];
                string strPassword = req.Query["Password"];
                string strStatus = req.Query["Status"];
                bool blnError = false;
                string strErrorMessage = "";

                //validations for parameters
                {
                    if (strEmail.Length < 0)
                    {
                        blnError = true;
                        strErrorMessage += Environment.NewLine + "email cannot be blank";
                    }
                    if (strPassword.Length < 0)
                    {
                        blnError = true;
                        strErrorMessage += Environment.NewLine + "email cannot be blank";
                    }
                    if (strEmail.Length < 0)
                    {
                        blnError = true;
                        strErrorMessage += Environment.NewLine + "email cannot be blank";
                    }
                    if (strFirstName.Length < 0)
                    {
                        blnError = true;
                        strErrorMessage += Environment.NewLine + "First Name cannot be blank";
                    }
                    if (strLastName.Length < 0)
                    {
                        blnError = true;
                        strErrorMessage += Environment.NewLine + "Last Name cannot be blank";
                    }

                    if (blnError == true)
                    {
                        return new OkObjectResult(strErrorMessage);
                    }
                }

                //creating the connection to the database
                //string strQuery = "select * from dbo.tblUsers;";
                string strQuery = "insert into dbo.tblUsers (email,firstname,lastname,status,password)values(@Email, @FirstName,@LastName,@Status,@Password)";
                DataSet dsUsers = new DataSet();

                using (SqlConnection conDS3870 = new SqlConnection(strConnection))
                using (SqlCommand comDS3870 = new SqlCommand(strQuery, conDS3870))
                {
                    //used to prevent sql injections we create parameterized statements
                    SqlParameter parEmail = new SqlParameter("Email", SqlDbType.VarChar);
                    parEmail.Value = strEmail;
                    comDS3870.Parameters.Add(parEmail);

                    SqlParameter parFirstName = new SqlParameter("FirstName", SqlDbType.VarChar);
                    parFirstName.Value = strFirstName;
                    comDS3870.Parameters.Add(parFirstName);

                    SqlParameter parLastName = new SqlParameter("LastName", SqlDbType.VarChar);
                    parLastName.Value = strLastName;
                    comDS3870.Parameters.Add(parLastName);

                    SqlParameter parStatus = new SqlParameter("Status", SqlDbType.VarChar);
                    parStatus.Value = strLastName;
                    comDS3870.Parameters.Add(parStatus);

                    SqlParameter parPassword = new SqlParameter("Password", SqlDbType.VarChar);
                    parPassword.Value = strPassword;
                    comDS3870.Parameters.Add(parPassword);
                    /* //manually accessing your connection
                    conDS3870.Open();
                    comDS3870.ExecuteNonQuery();
                    conDS3870.Close();
                    return new OkObjectResult("User Added");
                    */
                    
                    //used to fill the dataset
                    SqlDataAdapter daDS3870 = new SqlDataAdapter(comDS3870);
                    daDS3870.Fill(dsUsers);
                    return new OkObjectResult(dsUsers.Tables[0]);//return Data Table back
                }
            }

            else if (req.Method == HttpMethods.Get) {
                string strEmail = req.Query["Email"];
                DataSet dsUsers = new DataSet();

                if (strEmail == null || strEmail == " ")
                {
                    string strQuery = "select * from dbo.tblUsers;";

                    using (SqlConnection conDS3870 = new SqlConnection(strConnection))
                    using (SqlCommand comDS3870 = new SqlCommand(strQuery, conDS3870))
                    {
                        SqlDataAdapter daDS3870 = new SqlDataAdapter(comDS3870);
                        daDS3870.Fill(dsUsers);
                        return new OkObjectResult(dsUsers.Tables[0]);//return Data Table back
                    }
                } else
                {
                    string strQuery = "select * from dbo.tblUsers where Email = @Email;";
                    
                    using (SqlConnection conDS3870 = new SqlConnection(strConnection))
                    using (SqlCommand comDS3870 = new SqlCommand(strQuery, conDS3870))
                    {
                        SqlParameter parEmail = new SqlParameter("Email", SqlDbType.VarChar);
                        parEmail.Value = strEmail;
                        comDS3870.Parameters.Add(parEmail);

                        SqlDataAdapter daUsers = new SqlDataAdapter(comDS3870);
                        daUsers.Fill(dsUsers);
                        return new OkObjectResult(dsUsers.Tables[0]);
                    }
                }
            }

            else { return new OkObjectResult("You can only perform a GET or POST"); } 
        }
    }
}
