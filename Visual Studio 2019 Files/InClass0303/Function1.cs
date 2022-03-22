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

namespace InClass0303
{
    public static class Function1
    {
		private class Classification { 
			public string description { get; set; }
			public int minHours { get; set; }
			public Classification(string desc, int hours)
            {
				description = desc;
				minHours = hours;
            }
		}
		private class Athlete
		{
			public string FirstName { set; get; }
			public string LastName { set; get; }
			public int JerseyNumber { set; get; }
			public DateTime DateofBirth { set; get; }
			public Classification Classification { set; get; }

			public Athlete(string fName, string lName, int jNum, DateTime DoB, Classification classi){
				FirstName=fName;
				LastName=lName;
				JerseyNumber=jNum;
				DateofBirth=DoB;
				Classification=classi;
			}

		}

		private class Vehicle
		{
			public string Make { get; set; }
			public string Model { get; set; }
			public int Year { get; set; }
			public int MPG { get; set; }
			public Vehicle(string strMake, string strModel, int intYear, int intMPG)
			{
				Make = strMake;
				Model = strModel;
				Year = intYear;
				MPG = intMPG;
			}

			public double CalculateFuelCost(int milesTravelled, double PricePerGallon) { return (milesTravelled / MPG) * PricePerGallon; }

		}
		private class SportsTeam
	{
		public string Name { set; get; }
		public string Season { set; get; }
		public int TotalPlayers { set; get; }
		public int TravelPlayers { set; get; }
		public List<Athlete> Players { set; get; }
			public int PlayersNotTravel { get; set; }

		public SportsTeam(string name, string season, int total, int travel, List<Athlete> playerss)
		{
			Name = name;
			Season = season;
			TotalPlayers = total;
			TravelPlayers = travel;
			Players = playerss;
				PlayersNotTravel = total - travel;
		}
			public int CalculatePlayersCannotTravel()
			{
				return TotalPlayers - TravelPlayers;
			}
		}

	[FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

			Classification clsFreshman = new Classification("Freshmen", 0);
			Classification clsSophomore = new Classification("Sophomore", 0);

			Athlete athJane = new Athlete("Jane", "Doeling", 45, System.DateTime.Now, clsFreshman);
			Athlete athJohn = new Athlete("John", "Doe", 44, System.DateTime.Now, clsSophomore);

			List<Athlete> lstAthletes = new List<Athlete>();
			lstAthletes.Add(athJane);
			lstAthletes.Add(athJohn);

			SportsTeam sprtBasketball = new SportsTeam("BasketBall", "Winter", 32, 21,lstAthletes);

			Vehicle Prius = new Vehicle("Toyota", "Prius", 2010, 40);
			Vehicle Jetta = new Vehicle("VW", "Jetta", 2013, 42);
			Vehicle Bettle = new Vehicle("VW", "Beetle", 2006, 32);
			Vehicle Challenger = new Vehicle("Dodge", "Challenger", 2013, 21);
			Vehicle Tundra = new Vehicle("Toyota", "Tundra", 2000, 12);

			List<Vehicle> lstVehicle = new List<Vehicle>();

			lstVehicle.Add(Prius);
			lstVehicle.Add(Jetta);
			lstVehicle.Add(Bettle);
			lstVehicle.Add(Challenger);
			lstVehicle.Add(Tundra);

			string strVehicleMake = req.Query["strVehicleMake"];
			string strVehicleModel = req.Query["strVehicleModel"];
			int intVehicleYear = int.Parse(req.Query["intVehicleYear"]);
			int intMilesToGo = int.Parse(req.Query["intMilesToGo"]);
			double dblCostPerGallon = double.Parse(req.Query["dblCostPerGallon"]);

			foreach (Vehicle vehCurrent in lstVehicle) { 
				if (vehCurrent.Make == strVehicleMake &&vehCurrent.Model==strVehicleModel && vehCurrent.Year==intVehicleYear)
                {
					return new OkObjectResult(vehCurrent.CalculateFuelCost(intMilesToGo, dblCostPerGallon));
                }
			}



			return new OkObjectResult(Prius.CalculateFuelCost(1200,3.37));
        }
    }
}
