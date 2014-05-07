using System;
using System.Collections.Generic;
using Domain.Infrastructure;
using Newtonsoft.Json;

namespace Storage
{
	public class RelationalSession
	{
		private readonly Dictionary<Guid, String> _store;

		public RelationalSession()
		{
			_store = new Dictionary<Guid, string>();
		}

		public T GetByID<T>(Guid id) where T : DomainObject
		{
			if (_store.ContainsKey(id) == false)
			{
				throw new KeyNotFoundException();
			}

			return JsonConvert.DeserializeObject<T>(_store[id]);
		}

		public void Store<T>(T entity) where T : DomainObject
		{
			var json = JsonConvert.SerializeObject(entity);

			_store[entity.ID] = json;
		}
	}
}
