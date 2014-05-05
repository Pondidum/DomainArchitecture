using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using Domain.Infrastructure;
using StructureMap;

namespace Interface.Cli
{
	class Program
	{
		static void Main(string[] args)
		{

			var container = new Container(c => c.Scan(s =>
			{
				s.AssemblyContainingType<DomainEvent>();
				s.WithDefaultConventions();
			}));

			var store = new DataStore(container);

			using (var session = store.CreateSession())
			{
				var input = Candidate.Create("Andy", new DateTime(1986, 5, 27));
				
				session.Store(input);

				var output = session.GetByID<Candidate>(input.ID);

				Console.WriteLine("Input:  {0}", input.ID);
				Console.WriteLine("Output: {0}", output.ID);

			}

			Console.ReadKey();
		}
	}

	internal class DataStore
	{
		private readonly IContainer _container;
		private readonly Dictionary<Type, List<DomainEvent>> _store;

		public DataStore(IContainer container)
		{
			_container = container;
			_store = new Dictionary<Type, List<DomainEvent>>();
		}

		public Session CreateSession()
		{
			return new Session(_container, _store);
		}
	}

	internal class Session : IDisposable
	{
		private readonly IContainer _container;
		private readonly Dictionary<Type, List<DomainEvent>> _store;

		public Session(IContainer container, Dictionary<Type, List<DomainEvent>> store)
		{
			_container = container;
			_store = store;
		}

		public T GetByID<T>(Guid id) where T : IEventStream
		{
			var type = typeof (T);

			if (_store.ContainsKey(type) == false)
			{
				throw new KeyNotFoundException();
			}

			var events = _store[type].Where(e => e.AggregateID == id).OrderBy(e => e.SequenceID);

			var entity = _container.GetInstance<T>();
			entity.LoadFromEvents(events);

			return entity;
		}

		public void Store<T>(T entity) where T: IEventStream
		{
			var type = typeof (T);

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
