using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Parkrun.Models.AdoStuff;
using System;
using System.Threading.Tasks;

namespace AdoStuff
{
	class Program
	{
		static async Task Main(string[] args)
		{
			RegisterServices();
			var access = ApplicationServices.GetRequiredService<DatabaseAccess>();
			await access.ReadData(2016);
			await access.ReadData(2017);
			ParkrunModel parkrun = new ParkrunModel
			{
				Race = 293,
				Grade = "50.48",
				RaceDate = new DateTime(2019, 1, 5),
				Position = 207,
				Minutes = 29,
				Seconds = 13
			};

			await access.InsertRace(parkrun);

			await access.GetBestRace();

			Console.ReadLine();
		}

		

		public static void RegisterServices()
		{
			var services = new ServiceCollection();
			services.AddTransient<DatabaseAccess>();
			services.AddLogging(logger => { logger.AddConsole(); });

			ApplicationServices = services.BuildServiceProvider();
		}

		public static IServiceProvider ApplicationServices { get; private set; }
	}
}

