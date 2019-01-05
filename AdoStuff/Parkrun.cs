using System;

namespace Parkrun.Models
{

	namespace AdoStuff
	{

		public class ParkrunModel
		{
			public int Id { get; set; }

			public DateTime RaceDate { get; set; }

			public int Race { get; set; }

			public int Position { get; set; }

			public string Grade { get; set; }

			public int Minutes { get; set; }

			public int Seconds { get; set; }
		}
	}
}

