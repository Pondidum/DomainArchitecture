using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Infrastructure
{
	internal static class Extensions
	{
		public static Boolean Implements<T>(this Type self)
		{
			return self.GetInterfaces().Contains(typeof (T));
		}

		public static Boolean Inherits<T>(this Type self)
		{
			return typeof (T).IsAssignableFrom(self);
		}

		public static Boolean OneOnly<T>(this IEnumerable<T> self)
		{
			return self.Take(2).Count() == 1;
		}
	}
}
