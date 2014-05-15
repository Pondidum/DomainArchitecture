using System;
using System.Data.Common;
using System.Linq;
using Dapper;
using Domain;
using Domain.Infrastructure;
using Newtonsoft.Json;

namespace Storage.Relational.Queries
{
	public class GetCandidateByID
	{
		private readonly DbConnection _session;
		private readonly Guid _id;

		public GetCandidateByID(DbConnection session, Guid id)
		{
			_session = session;
			_id = id;
		}

		public Candidate Execute()
		{
			var dtos = _session.Query<EventDto>(
				"select id, aggregateID, order, type, json from candidates where aggregateID = @id order by order asc",
				new { id = _id });

			var events = dtos
				.Select(dto => JsonConvert.DeserializeObject(dto.Json, Type.GetType(dto.Type)))
				.Cast<DomainEvent>();

			var candidate = new Candidate();
			((IEventStream)candidate).LoadFromEvents(events);
			
			return candidate;
		}
	}
}
