using System;
using Domain;

namespace Interface.Cli
{
	class Program
	{
		static void Main(string[] args)
		{
			var candidate = Candidate.Create("Andy", new DateTime(1986, 5, 27));

			Console.WriteLine("Hi {0}", candidate.Name);
			Console.ReadKey();
		}
	}
}
