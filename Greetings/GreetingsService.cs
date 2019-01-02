using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Greetings
{
	public class GreetingsService : IGreetingsService
	{
		private readonly string _from;
		private readonly string _country;

		public GreetingsService(IOptions<GreetingServiceOptions> options)
		{
			_from = options.Value.From; // allows options to be passed into the constructor
			_country = options.Value.Country;
		}

		public string Greet(string name)
		{
			return $"Hello {name} greetings from {_from} in {_country}";
		}
	}
}
