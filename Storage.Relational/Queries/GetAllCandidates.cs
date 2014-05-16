using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Dapper;
using Domain;
using Domain.Infrastructure;
using Newtonsoft.Json;

namespace Storage.Relational.Queries
{
	public class GetAllCandidates
	{
		private readonly DbConnection _connection;

		public GetAllCandidates(DbConnection connection)
		{
			_connection = connection;
		}

		public IEnumerable<Candidate> Execute()
		{
			var dtos = _connection.Query<EventDto>(
				"select id, aggregateID, order, type, json from candidates");

			return dtos
				.Select(dto => JsonConvert.DeserializeObject(dto.Json, Type.GetType(dto.Type)))
				.Cast<DomainEvent>()
				.OrderBy(e => e.SequenceID)
				.GroupBy(e => e.AggregateID)
				.Select(group =>
				{
					var candidate = new Candidate();
					((IEventStream)candidate).LoadFromEvents(group);
					return candidate;
				});
		}
	}
}
