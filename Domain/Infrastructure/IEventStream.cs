using System.Collections.Generic;

namespace Domain.Infrastructure
{
	public interface IEventStream
	{
		IEnumerable<DomainEvent> GetEvents();
		void Clear();
		void LoadFromEvents(IEnumerable<DomainEvent> events);
	}
}
