using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Domain.Infrastructure;

namespace Domain
{
	public class DomainObject
	{
		private const BindingFlags MethodFlags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;
		private readonly List<MethodInfo> _domainEventHandlers;

		public DomainObject()
		{
			_domainEventHandlers = GetType()
				.GetMethods(MethodFlags)
				.Where(m => m.GetParameters().OneOnly())
				.Where(m => m.GetParameters().First().ParameterType.Implements<IDomainEvent>())
				.ToList();
		}

		protected void ApplyEvent(IDomainEvent domainEvent)
		{
			
			var specificMethod = _domainEventHandlers
				.FirstOrDefault(m => m.GetParameters().First().ParameterType == domainEvent.GetType());

			if (specificMethod == null)
			{
				throw new DomainEventHandlerNotFoundException(GetType(), domainEvent.GetType());
			}

			specificMethod.Invoke(this, new[] { domainEvent });
		}
	}
}
