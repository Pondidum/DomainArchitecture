using System.Collections.Generic;
using System.Data.Common;
using Dapper;
using Domain;

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

			return DomainObjectBuilder.Build<Candidate>(dtos);
		}
	}
}
