using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Domain.Infrastructure;

namespace Domain
{
	public class DomainObject : IEventStream
	{
		private const BindingFlags MethodFlags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;

		private readonly List<MethodInfo> _domainEventHandlers;
		private readonly List<IDomainEvent> _events;

		public DomainObject()
		{
			_events = new List<IDomainEvent>();
			_domainEventHandlers = GetType()
				.GetMethods(MethodFlags)
				.Where(m => m.GetParameters().OneOnly())
				.Where(m => m.GetParameters().First().ParameterType.Implements<IDomainEvent>())
				.ToList();
		}

		protected void ApplyEvent(IDomainEvent domainEvent)
		{
			Apply(domainEvent);
			_events.Add(domainEvent);
		}

		private void Apply(IDomainEvent domainEvent)
		{
			var specificMethod = _domainEventHandlers
				.FirstOrDefault(m => m.GetParameters().First().ParameterType == domainEvent.GetType());

			if (specificMethod == null)
			{
				throw new DomainEventHandlerNotFoundException(GetType(), domainEvent.GetType());
			}

			specificMethod.Invoke(this, new object[] { domainEvent });
		}

		IEnumerable<IDomainEvent> IEventStream.GetEvents()
		{
			return _events;
		}

		void IEventStream.Clear()
		{
			_events.Clear();
		}

		void IEventStream.LoadFromEvents(IEnumerable<IDomainEvent> events)
		{
			foreach (var domainEvent in events)
			{
				Apply(domainEvent);
			}
		}
	}
}
