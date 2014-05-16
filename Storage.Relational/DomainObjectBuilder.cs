using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Infrastructure;
using Newtonsoft.Json;

namespace Storage.Relational
{
	internal class DomainObjectBuilder
	{
		public static IEnumerable<T> Build<T>(IEnumerable<EventDto> eventDtos) where T : IEventStream, new()
		{
			return eventDtos
				.Select(dto => JsonConvert.DeserializeObject(dto.Json, Type.GetType(dto.Type)))
				.Cast<DomainEvent>()
				.OrderBy(e => e.SequenceID)
				.GroupBy(e => e.AggregateID)
				.Select(group =>
				{
					var entity = new T();
					entity.LoadFromEvents(group);
					return entity;
				});
		}
	}
}