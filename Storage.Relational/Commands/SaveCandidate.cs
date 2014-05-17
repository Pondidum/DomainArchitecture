using System.Data;
using System.Data.Common;
using System.Linq;
using Dapper;
using Domain;
using Domain.Infrastructure;
using Newtonsoft.Json;

namespace Storage.Relational.Commands
{
	public class SaveCandidate
	{
		private readonly DbConnection _connection;
		private readonly Candidate _candidate;

		public SaveCandidate(DbConnection connection, Candidate candidate)
		{
			_connection = connection;
			_candidate = candidate;
		}

		public void Execute()
		{
			var stream = (IEventStream)_candidate;

			var events = stream.GetEvents().ToList();

			var lastEvent = _connection
				.Query<long?>(
					"select max(sequence) from candidates where aggregateID = @id",
					new { id = _candidate.ID})
				.SingleOrDefault();

			if (lastEvent.HasValue && lastEvent.Value >= events.First().SequenceID)
			{
				throw new DBConcurrencyException();
			}

			using (var transaction = _connection.BeginTransaction())
			{
				var dtos = events.Select(e => new EventDto()
				{
					AggregateID = e.AggregateID,
					Sequence =  e.SequenceID,
					Type =  e.GetType().FullName,
					Json = JsonConvert.SerializeObject(e)
				});

				_connection.Execute(
					"insert into candidates (aggregateID, sequence, type, json) values(@aggregateID, @sequence, @type, @json)", 
					dtos);

				transaction.Commit();
			}
		}
	}
}
