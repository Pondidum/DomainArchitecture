using System;
using System.Data.Common;
using System.Linq;
using Dapper;
using Domain;

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
				"select id, aggregateID, sequence, type, json from candidates where aggregateID = @id order by sequence asc",
				new { id = _id });

			return DomainObjectBuilder
				.Build<Candidate>(dtos)
				.First();
		}
	}
}
