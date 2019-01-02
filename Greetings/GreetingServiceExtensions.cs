using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Greetings
{
	public static class GreetingServiceExtensions
	{
		//This is an extension method for IServiceCollection which allows you to pass in option
		public static IServiceCollection AddGreetingService(
			this IServiceCollection collection,
			Action<GreetingServiceOptions> setupAction)
		{
			collection.Configure(setupAction); 
			return collection.AddTransient<IGreetingsService, GreetingsService>();
		}
	}
}
