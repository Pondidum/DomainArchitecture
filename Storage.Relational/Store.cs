namespace Storage.Relational
{
	public class Store
	{
		public ISession GetSession()
		{
			return new Session();
		}
	}

	public interface ISession
	{
	}
}