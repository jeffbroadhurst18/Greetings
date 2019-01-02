using Microsoft.Extensions.DependencyInjection;
using System;

namespace Greetings
{
	class Program
	{
		static void Main(string[] args)
		{
			using (var container = RegisterServices(true))
			{
				var controller = container.GetService<HomeController>();
				string result = controller.Hello("Matty");
				Console.WriteLine(result);

			}
			using (var container = RegisterServices(false))
			{
				var controller = container.GetService<HomeController>();
				string result = controller.Hello("Matty");
				Console.WriteLine(result);

			}
			Console.ReadLine();
		}

		static ServiceProvider RegisterServices(bool german)
		{
			var services = new ServiceCollection();
			services.AddOptions();


			//This adds the GreetingsService with the option
			if (german)
			{
				services.AddGreetingService(options =>
				{
					options.From = "Christian";
					options.Country = "Germany";
				});
			}
			else
			{
				services.AddGreetingService(options =>
				{
					options.From = "Pierre";
					options.Country = "France";
				});
			}
			services.AddTransient<HomeController>(); //Greetings Service is passed into HomeController
			return services.BuildServiceProvider();
		}
	}
}
