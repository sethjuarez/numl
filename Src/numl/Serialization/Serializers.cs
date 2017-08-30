using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using numl.Utils;

namespace numl.Serialization
{
	public static class Serializers
	{
		private static Lazy<List<ISerializer>> _serializers = new Lazy<List<ISerializer>>(loadSerializers);
		private static Dictionary<Type, ISerializer> _mapping = new Dictionary<Type, ISerializer>();

		public static ISerializer GetSerializer(this Type t)
		{
			// is it an actual serializer?
			if (t.GetTypeInfo().ImplementedInterfaces.Contains(typeof(ISerializer)))
			{
				if (_mapping.Values.Count != 0)
					return _mapping.Values
									   .Where(s => s.GetType() == t)
									   .First();
				else
					return _serializers.Value.Where(s => s.GetType() == t).First();
			}
			else // looking for a specific type serializer
			{
				if (HasSerializer(t)) return _mapping[t];
				else return null;
			}
		}

		public static bool HasSerializer(this Type t)
		{
			if (_mapping.ContainsKey(t)) return true;
			else
			{
				List<ISerializer> s = new List<ISerializer>();
				foreach (var serializer in _serializers.Value)
					if (serializer.CanConvert(t))
						s.Add(serializer);

				if (s.Count == 0) return false;
				if (s.Count == 1)
				{
					_mapping[t] = s[0];
					return true;
				}
				else
				{
					if (s[0].GetType().GetTypeInfo().IsSubclassOf(s[1].GetType()))
						_mapping[t] = s[0];
					else
						_mapping[t] = s[1];
					return true;
				}
			}
		}

		internal static void ReloadSerializers()
		{
			_serializers = new Lazy<List<ISerializer>>(loadSerializers);
		}
		
		private static List<ISerializer> loadSerializers()
		{
			// this will take some time....
			var serializers = new List<ISerializer>();
			foreach (var a in Ject.GetLoadedAssemblies())
			{
				foreach (var type in a.GetTypes())
				{
					if (typeof(ISerializer).IsAssignableFrom(type))
					{
						var info = type.GetTypeInfo();
						if (info.IsClass && !info.IsAbstract && !info.IsInterface)
						{
							var s = (ISerializer)Activator.CreateInstance(type);
							if (!serializers.Contains(s))
								serializers.Add(s);
						}
					}
				}
			}
			return serializers;
		}
	}
}
