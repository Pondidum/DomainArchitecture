using System.Collections.Generic;

namespace Domain
{
	public interface IEventStream
	{
		IEnumerable<IDomainEvent> GetEvents();
		void Clear();
		void LoadFromEvents(IEnumerable<IDomainEvent> events);
	}
}
