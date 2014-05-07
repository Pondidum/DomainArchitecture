using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using Domain.Infrastructure;
using Storage;
using StructureMap;

namespace Interface.Cli
{
	class Program
	{
		static void Main(string[] args)
		{

			var container = new Container(c => c.Scan(s =>
			{
				s.AssemblyContainingType<DomainEvent>();
				s.WithDefaultConventions();
			}));

			var store = new DataStore(container);

			using (var session = store.CreateStreamSession())
			{
				var input = Candidate.Create("Andy", new DateTime(1986, 5, 27));
				
				session.Store(input);

				var output = session.GetByID<Candidate>(input.ID);

				Console.WriteLine("Input:  {0}", input.ID);
				Console.WriteLine("Output: {0}", output.ID);

			}

			Console.ReadKey();
		}
	}

	
}
