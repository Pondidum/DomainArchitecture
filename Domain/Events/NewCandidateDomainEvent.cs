using System;
using Domain.Infrastructure;

namespace Domain.Events
{
	public class NewCandidateDomainEvent : DomainEvent
	{
		public string Name { get; private set; }
		public DateTime DoB { get; private set; }
		public Guid CandidateID { get; private set; }

		public NewCandidateDomainEvent(string name, DateTime dob)
		{
			CandidateID = Guid.NewGuid();
			Name = name;
			DoB = dob;
		}
	}
}
