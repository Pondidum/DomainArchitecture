using System;
using Domain;

namespace Storage.Relational.Queries
{
	public class GetCandidateByID
	{
		private readonly ISession _session;
		private readonly Guid _id;

		public GetCandidateByID(ISession session, Guid id)
		{
			_session = session;
			_id = id;
		}

		public Candidate Execute()
		{
			return null;
		}
	}
}
