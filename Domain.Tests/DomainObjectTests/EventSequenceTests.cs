using System.Linq;
using Should;
using Xunit;

namespace Domain.Tests.DomainObjectTests
{
	public class EventSequenceTests
	{
		[Fact]
		public void EventShouldBeOrdered()
		{
			var p = new Person();

			p.DoTest();
			p.DoTest();
			p.DoTest();

			var events = ((IEventStream)p).GetEvents().ToList();

			events[0].SequenceID.ShouldEqual(0);
			events[1].SequenceID.ShouldEqual(1);
			events[2].SequenceID.ShouldEqual(2);
		}

		[Fact]
		public void When_loading_from_a_stream_a_new_event_should_be_ordered()
		{
			var stream = new[]
			{
				new TestEvent {SequenceID = 0}, 
				new TestEvent {SequenceID = 1}, 
				new TestEvent {SequenceID = 2},
			};

			var p = new Person();
			((IEventStream)p).LoadFromEvents(stream);

			p.DoTest();

			var events = ((IEventStream)p).GetEvents().ToList();

			events.First().SequenceID.ShouldEqual(3);
		}





		private class Person : DomainObject
		{
			public void DoTest()
			{
				ApplyEvent(new TestEvent());
			}

			private void OnTest(TestEvent domainEvent)
			{
				//do some things
			}

		}

		private class TestEvent : DomainEvent
		{
		}
	}
}
