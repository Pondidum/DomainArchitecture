using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Infrastructure;
using StructureMap;

namespace Storage
{
	public class Session : IDisposable
	{
		private readonly IContainer _container;
		private readonly Dictionary<Type, List<DomainEvent>> _store;

		public Session(IContainer container)
		{
			_container = container;
			_store = new Dictionary<Type, List<DomainEvent>>();
		}

		public T GetByID<T>(Guid id) where T : IEventStream
		{
			var type = typeof(T);

			if (_store.ContainsKey(type) == false)
			{
				throw new KeyNotFoundException();
			}

			var events = _store[type].Where(e => e.AggregateID == id).OrderBy(e => e.SequenceID);

			var entity = _container.GetInstance<T>();
			entity.LoadFromEvents(events);

			return entity;
		}

		public void Store<T>(T entity) where T : IEventStream
		{
			var type = typeof(T);

			if (_store.ContainsKey(type) == false)
			{
				_store[type] = new List<DomainEvent>();
			}

			_store[type].AddRange(entity.GetEvents());

		}

		public void Commit()
		{

		}

		public void Dispose()
		{
		}
	}
}
