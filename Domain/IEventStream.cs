using System.Collections.Generic;

namespace Domain
{
	public interface IEventStream
	{
		IEnumerable<DomainEvent> GetEvents();
		void Clear();
		void LoadFromEvents(IEnumerable<DomainEvent> events);
	}
}
