using System;

namespace Domain.Events
{
	public class NewCandidateDomainEvent : DomainEvent
	{
		public string Name { get; private set; }
		public DateTime DoB { get; private set; }

		public NewCandidateDomainEvent(string name, DateTime dob)
		{
			Name = name;
			DoB = dob;
		}
	}
}
