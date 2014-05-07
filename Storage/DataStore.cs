using StructureMap;

namespace Storage
{
	public class DataStore
	{
		private readonly IContainer _container;

		public DataStore(IContainer container)
		{
			_container = container;
			
		}

		public Session CreateStreamSession()
		{
			return new Session(_container);
		}
	}
}
