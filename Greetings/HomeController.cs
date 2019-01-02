using System;
using System.Collections.Generic;
using System.Text;

namespace Greetings
{
	public class HomeController
	{
		private readonly IGreetingsService _greetingsService;

		public HomeController(IGreetingsService greetingsService)
		{
			_greetingsService = greetingsService ?? throw new ArgumentNullException(nameof(greetingsService));
		}

		public string Hello(string name)
		{
			return _greetingsService.Greet(name);
		}
	}
}
