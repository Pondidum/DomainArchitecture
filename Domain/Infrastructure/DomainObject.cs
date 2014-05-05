using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Domain.Infrastructure
{
	public class DomainObject : IEventStream
	{
		private const BindingFlags MethodFlags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;

		private readonly List<MethodInfo> _domainEventHandlers;
		private readonly List<DomainEvent> _events;
		private int _id;

		public DomainObject()
		{
			_id = 0;
			_events = new List<DomainEvent>();

			_domainEventHandlers = GetType()
				.GetMethods(MethodFlags)
				.Where(m => m.GetParameters().OneOnly())
				.Where(m => m.GetParameters().First().ParameterType.Inherits<DomainEvent>())
				.ToList();
		}

		protected void ApplyEvent(DomainEvent domainEvent)
		{
			domainEvent.SequenceID = _id;

			Apply(domainEvent);
			_events.Add(domainEvent);

			_id++;
		}

		private void Apply(DomainEvent domainEvent)
		{
			var specificMethod = _domainEventHandlers
				.FirstOrDefault(m => m.GetParameters().First().ParameterType == domainEvent.GetType());

			if (specificMethod == null)
			{
				throw new DomainEventHandlerNotFoundException(GetType(), domainEvent.GetType());
			}

			specificMethod.Invoke(this, new object[] { domainEvent });
		}

		IEnumerable<DomainEvent> IEventStream.GetEvents()
		{
			return _events;
		}

		void IEventStream.Clear()
		{
			_events.Clear();
		}

		void IEventStream.LoadFromEvents(IEnumerable<DomainEvent> events)
		{
			var lastID = 0;

			foreach (var domainEvent in events)
			{
				Apply(domainEvent);
				lastID = domainEvent.SequenceID;
			}

			_id = lastID + 1;
		}
	}
}
