using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.IO;
using Parkrun.Models.AdoStuff;
using System.Threading.Tasks;

namespace AdoStuff
{
	//private methods are visible only within the same class
	//internal methods are visible within the same assembly (project)
	//protected methods are accessible from within the same class or from classes derived from the first class.
	//virtual methods have code in them and can be overriden but DON'T HAVE TO BE (i.e. you can inherit the class and use the methods as they are)
	//abstract methods just have the signature with no code in them so they MUST be overriden.

	public class DatabaseAccess
	{
		private readonly ILogger<DatabaseAccess> _logger;
		private string _config;

		SqlConnection sqlConnection;
		public DatabaseAccess(ILogger<DatabaseAccess> logger)
		{
			DefineConfiguration();
			_config = Configuration.GetConnectionString("Location");
			_logger = logger;
			logger.LogInformation(_config);
		}


		public async Task ReadData(int year)
		{
			using (SqlConnection connection = new SqlConnection(_config))
			{
				connection.Open();
				string sql = @"SELECT [Id],[RaceDate],[Race],[Position],[Grade],[Minutes],[Seconds]" +
					" FROM[LocationDB].[dbo].[Parkruns] WHERE RaceDate >= @Start and RaceDate <=@End" +
					" ORDER BY RaceDate";
				var command = new SqlCommand(sql, connection);
				command.Parameters.Add("Start", SqlDbType.DateTime);
				command.Parameters["Start"].Value = new DateTime(year, 1, 1);
				command.Parameters.Add("End", SqlDbType.DateTime);
				command.Parameters["End"].Value = new DateTime(year + 1, 1, 1);

				using (SqlDataReader reader = await command.ExecuteReaderAsync())
				{
					while (reader.Read())
					{
						var secs = int.Parse(reader["Seconds"].ToString()) < 10 ? "0" + reader["Seconds"] : reader["Seconds"];
						_logger.LogInformation($"Race: {reader["Race"]}, Date: {reader["RaceDate"]}," +
							$" Time: {reader["Minutes"]}:{secs}");
					}
				}
			}
		}

		public async Task ReadDataUsingStoredProcedure(int year)
		{
			using (SqlConnection connection = new SqlConnection(_config))
			{
				SqlCommand command = connection.CreateCommand();
				command.CommandText = "[dbo].[GetParkrunsByYear]";
				command.CommandType = CommandType.StoredProcedure;
				SqlParameter p1 = command.CreateParameter();
				p1.SqlDbType = SqlDbType.NVarChar;
				p1.ParameterName = "@Year";
				p1.Value = year;
				command.Parameters.Add(p1);
				connection.Open();

				using (SqlDataReader reader = await command.ExecuteReaderAsync())
				{
					while (reader.Read())
					{
						var secs = int.Parse(reader["Seconds"].ToString()) < 10 ? "0" + reader["Seconds"] : reader["Seconds"];
						_logger.LogInformation($"Race: {reader["Race"]}, Date: {reader["RaceDate"]}," +
							$" Time: {reader["Minutes"]}:{secs}");
					}
				}
			}
		}


		internal async Task GetBestRace()
		{
			using (SqlConnection connection = new SqlConnection(_config))
			{
				connection.Open();
				string sql = "Select max(grade) from dbo.parkruns";
				SqlCommand command = new SqlCommand(sql, connection);
				var best = await command.ExecuteScalarAsync();
				_logger.LogInformation("Best Grade = " + best);

				sql = "Select min(grade) from dbo.parkruns";
				command = new SqlCommand(sql, connection);
				var worst = await command.ExecuteScalarAsync();
				_logger.LogInformation("Worst Grade = " + worst);
			}
		}

		public async Task InsertRace(ParkrunModel parkrun)
		{
			using (SqlConnection connection = new SqlConnection(_config))
			{
				connection.Open();
				string sql = @"INSERT INTO [dbo].[Parkruns] ([RaceDate],[Race],[Position],[Grade],[Minutes],[Seconds])
								VALUES (@RaceDate,@Race,@Position,@Grade,@Minutes,@Seconds)";

				var command = new SqlCommand(sql, connection);
				command.Parameters.Add("RaceDate", SqlDbType.DateTime);
				command.Parameters["RaceDate"].Value = parkrun.RaceDate;
				command.Parameters.Add("Race", SqlDbType.Int);
				command.Parameters["Race"].Value = parkrun.Race;
				command.Parameters.Add("Position", SqlDbType.Int);
				command.Parameters["Position"].Value = parkrun.Position;
				command.Parameters.Add("Grade", SqlDbType.NVarChar, 50);
				command.Parameters["Grade"].Value = parkrun.Grade;
				command.Parameters.Add("Minutes", SqlDbType.Int);
				command.Parameters["Minutes"].Value = parkrun.Minutes;
				command.Parameters.Add("Seconds", SqlDbType.Int);
				command.Parameters["Seconds"].Value = parkrun.Seconds;

				var result = await command.ExecuteNonQueryAsync();
				_logger.LogInformation("Record successfully inserted");
			}
		}
		private static void DefineConfiguration()
		{
			IConfigurationBuilder configBuilder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json");
			Configuration = configBuilder.Build();
		}

		public static IConfiguration Configuration { get; set; }
	}
}

