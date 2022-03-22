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
using Microsoft.Data.Sqlite;


namespace authenticateUser
{
    public static class Function1
    {
        private class TaskUser {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public List<TaskRole> Roles { get; set; }

            public TaskUser(string firstName, string lastName, string email, string password, List<TaskRole> roles)
            {
                FirstName = firstName;
                LastName = lastName;
                Email = email;
                Password = password;
                Roles = roles;
            }

            public bool VerifyPassword(string strPassword)
            {
                if (strPassword == Password)
                {
                    return true;
                }
                else
                    return false;
            }
        }

        private class TaskRole
        {
            public string Description { get; set; }
            public List<TaskModules> AccessTo { get; set; }

            public TaskRole(string Desc, List<TaskModules> Acc)
            {
                Description = Desc;
                AccessTo = Acc;
            }
        }

        private class TaskModules
        {
            string Description { get; set; }
            double Price { get; set; }
            public TaskModules(string Desc, double price)
            {
                Description = Desc;
                Price = price;
            }
        }

        [FunctionName("authenticateUser")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            TaskModules Basic= new TaskModules("Basic Tasks", 0.99);
            TaskModules Advanced = new TaskModules("Advanced Tasks", 1.99);
            List<TaskModules> lstBasicMod = new List<TaskModules>();
            lstBasicMod.Add(Basic);
            List<TaskModules> lstAdvancedMod = new List<TaskModules>();
            lstAdvancedMod.Add(Basic);
            lstAdvancedMod.Add(Advanced);


            TaskRole baTask = new TaskRole("Basic", lstBasicMod);
            TaskRole adTask = new TaskRole("Premier", lstAdvancedMod);
            List<TaskRole> lstbaTaskRole = new List<TaskRole>();
                lstbaTaskRole.Add(baTask);
            List<TaskRole> lstadTaskRole = new List<TaskRole>();
                lstadTaskRole.Add(baTask);
                lstadTaskRole.Add(adTask);

            TaskUser userOne = new TaskUser("Jane","Doeling", "janedoeling@gmail.com","Mickey2020!",lstbaTaskRole);
            TaskUser userTwo = new TaskUser("John", "Doe", "johndoe12@gmail.com", "Mickey2020!", lstadTaskRole);


            string strUsername = req.Query["strUsername"];
            string strPassword = req.Query["strPassword"];

            List<TaskUser> lstUsers = new List<TaskUser>();
                lstUsers.Add(userOne);
                lstUsers.Add(userTwo);

            foreach(TaskUser current in lstUsers){
                if(current.Email == strUsername && current.VerifyPassword(strPassword) == true)
                {
                    return new OkObjectResult(current.Roles);
                }
            }

            return new OkObjectResult("User Credentials Not Found, try again.");
        }
    }
}
