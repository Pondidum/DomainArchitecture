using System;
using System.Data.SQLite;
using System.IO;
using Domain;
using Domain.Infrastructure;
using Storage;
using Storage.Relational.Commands;
using Storage.Relational.Queries;
using StructureMap;

namespace Interface.Cli
{
	class Program
	{
		static void Main(string[] args)
		{
			File.Delete("temp.db");
			SQLiteConnection.CreateFile("temp.db");

			var connection = new SQLiteConnection("Data Source=temp.db;Version=3;");
			connection.Open();

			using (var command = connection.CreateCommand())
			{
				command.CommandText = "create table candidates (id uniqueidentifier, aggregateID uniqueidentifier, sequence int, type varchar(8000), json varchar)";
				command.ExecuteNonQuery();
			}

			var candidate = Candidate.Create("Andy", new DateTime(1986, 05, 27));

			var save = new SaveCandidate(connection, candidate);
			save.Execute();

			var load = new GetCandidateByID(connection, candidate.ID);
			var c2 = load.Execute();
		}
	}


}
