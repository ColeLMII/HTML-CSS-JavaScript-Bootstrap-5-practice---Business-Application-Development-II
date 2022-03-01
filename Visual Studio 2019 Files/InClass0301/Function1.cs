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
        
        private class Vehicle
        {
            public string Make { get; set; }
            public string Model { get; set; }
            public int Year { get; set; }

            public Vehicle(string make, string model, int year)
            {
                Make = make;
                Model = model;
                Year = year;
            }
        }

        private class User
        {
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string MiddleInit { get; set; }
            public string Email { get; set; }
            public DateTime DateofBirth { get; set; }

            public User(string first, string last, string middle, string email, DateTime DOB)
            {
                firstName = first;
                lastName = last;
                MiddleInit = middle;
                Email = email;
                DateofBirth = DOB;
            }
        }

        private class Classroom
        {
            public string Building { get; set; }
            public string Room { get; set; }
            public int Capacity { get; set; }
            public classroomType Type { get; set; }

            public Classroom( string building, string room, int capacity, classroomType type)
            {
                Building = building;
                Room = room;
                Capacity = capacity;
                Type = type;
            }
        }

        private class classroomType
        {
            public string Description { get; set; }
            public bool TechnologyExist { get; set; }
            public bool SpecialUse { get; set; }

            public classroomType(string desc, bool exist, bool use)
            {
                Description = desc;
                TechnologyExist = exist;
                SpecialUse = use;
            }
        }

        [FunctionName("Vehicle_Finder")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            classroomType CTLecture = new classroomType("Lecture", true, false);
            classroomType CTLab = new classroomType("Lab", false, true);

            Classroom JH307 = new Classroom("Johnson Hall", "307", 37, CTLecture);

            Vehicle vehPrius = new Vehicle("Toyota", "Prius", 2010);
            Vehicle vehTundra = new Vehicle("Toyota", "Tundra", 2000);

            User tempUser = new User("Larenzle", "Coleman", "M", "lmcoleman44@tntech.edu", DateTime.Parse("2000-09-26"));

            string strVehicleModel = req.Query["strVehicleModel"];

            List<Vehicle> lstVehicles = new List<Vehicle>();

            lstVehicles.Add(vehPrius);
            lstVehicles.Add(vehTundra);

            List<Vehicle> Found = new List<Vehicle>();

            foreach(Vehicle current in lstVehicles)
            {
                if(current.Model == strVehicleModel)
                {
                    Found.Add(current);
                }
            }

            if(Found.Count == 1)
            {
                return new OkObjectResult(Found);
            }
            else
            {
                return new OkObjectResult("No Vehicle Found");
            }

            {/*string[] arrVehicles = { "1995 Dodle Neon", "1995 Subaru Outback", "1996 GMC Sonoma", "1996 Toyota Tacoma", "1998 Toyota Tacoma", "2000 Toyota Tundra", "2001 Toyota Tundra", "2002 Ford Ranger", "2002 Chevrolet Tracker", "2005 Toyota Highlander", "2006 Toyota Corolla", "2008 Honda Element", "2009 Subaru Forestor", "2010 Toyota Prius" };

            foreach (string strVehicle in arrVehicles)
            {
                lstVehicles.Add(strVehicle);
            }

            if (strVehicleModel != "" && strVehicleModel != null)
            {
                string foundVehicles = "";
                bool blnFound = false;

                foreach (string strCurrVehicle in lstVehicles)
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
            */
            }

            string name = req.Query["name"];
            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }
    }
}
