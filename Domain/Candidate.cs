using System;
using System.Collections.Generic;
using Domain.Events;
using Domain.Infrastructure;

namespace Domain
{
	public class Candidate : DomainObject
	{
		private readonly List<Address> _addresses;
		private readonly List<Phone> _phones;
		private readonly List<Email> _emails;

		public string Name { get; private set; }
		public DateTime DoB { get; private set; }
		
		public IEnumerable<Address> Addresses { get { return _addresses; } }
		public IEnumerable<Phone> PhoneNumbers { get { return _phones; } }
		public IEnumerable<Email> EmailAddresses { get { return _emails; } }

		private Candidate()
		{
			_addresses = new List<Address>();
			_phones = new List<Phone>();
			_emails = new List<Email>();
		}

		public static Candidate Create(string name, DateTime dob)
		{
			var candidate = new Candidate();
			candidate.ApplyEvent(new NewCandidateDomainEvent(name, dob));

			return candidate;
		}

		private void OnNewCandidate(NewCandidateDomainEvent domainEvent)
		{
			ID = domainEvent.CandidateID;
			Name = domainEvent.Name;
			DoB = domainEvent.DoB;
		}
	}
}
