using System;
using System.Collections.Generic;
using Domain.Infrastructure;
using Should;
using Xunit;

namespace Domain.Tests.DomainObjectTests
{
	public class AggregateIDTests
	{
		[Fact]
		public void When_creating_new_person_it_should_have_an_id()
		{
			var p = Person.Create();

			p.ID.ShouldNotEqual(Guid.Empty);
		}

		[Fact]
		public void When_loading_via_events_it_should_set_the_id()
		{
			var events = new[] { new PersonCreatedEvent() };
			var p = Person.Load(events);

			p.ID.ShouldNotEqual(Guid.Empty);
		}

		private class Person : DomainObject
		{
			private Person()
			{
			}

			public static Person Load(IEnumerable<DomainEvent> events)
			{
				var p = new Person();
				((IEventStream)p).LoadFromEvents(events);
				return p;
			}

			public static Person Create()
			{
				var person = new Person();
				person.ApplyEvent(new PersonCreatedEvent());
				return person;
			}

			private void OnTest(PersonCreatedEvent domainEvent)
			{
				ID = domainEvent.PersonID;
			}

		}

		private class PersonCreatedEvent : DomainEvent
		{
			public Guid PersonID { get; private set; }

			public PersonCreatedEvent()
			{
				PersonID = Guid.NewGuid();
			}
		}
	}
}
