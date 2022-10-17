using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace Installers
{
	public abstract class BaseInstaller : MonoBehaviour
	{
		protected void RegisterService<T>(T service) where T : class
		{
			ServicesStorage.RegisterService<T>(service);
		}

		protected T GetService<T>() where T : class
		{
			return ServicesStorage.GetService<T>();
		}

		private class ServicesStorage
		{
			private Dictionary<Type, object> _services = new Dictionary<Type, object>();
			private static ServicesStorage _instance;

			static ServicesStorage()
			{
				_instance = new ServicesStorage();
			}

			public static void RegisterService<T>(T service) where T : class
			{
				if (_instance._services.ContainsKey(typeof(T)))
				{
					return;
				}
				_instance._services.Add(typeof(T), service);
			}

			public static T GetService<T>() where T : class
			{
				if (_instance._services.ContainsKey(typeof(T)))
				{
					return (T)_instance._services[typeof(T)];
				}
				else
					return null;
			}
		}
	}
}