using System;
using System.Collections.Generic;

namespace Util.Events
{
	/// <summary>
	/// Event dispatcher class.
	/// </summary>
	public class EventDispatcher : IEventDispatcher
	{
		private Dictionary<Type, Action<object>> eventCallbacks;
		private Dictionary<object, Action<object>> delegateLookup;

		public EventDispatcher()
		{
			eventCallbacks = new Dictionary<Type, Action<object>>();
			delegateLookup = new Dictionary<object, Action<object>>();
		}

		/// <summary>
		/// Implementation of <see cref="IEventDispatcher.AddEventListener(Type, Action)"/>
		/// </summary>
		/// <param name="type">The type of event to be added.</param>
		/// <param name="handler">The handler.</param>
		public void AddEventListener(Type type, Action handler)
		{
			if (!eventCallbacks.ContainsKey(type))
			{
				eventCallbacks.Add(type, delegate {});
			}

			Action<object> handlerFunc = obj => { handler(); };

			eventCallbacks[type] += handlerFunc;
			delegateLookup[handler] = handlerFunc;
		}

		/// <summary>
		/// Imprementation of <see cref="IEventDispatcher.AddEventListener{T}(Action{T})"/>
		/// </summary>
		/// <typeparam name="T">The type of event to be added.</typeparam>
		/// <param name="handler">The handler.</param>
		public void AddEventListener<T>(Action<T> handler) where T : IEvent
		{
			Type type = typeof(T);

			if (!eventCallbacks.ContainsKey(type))
			{
				eventCallbacks.Add(type, delegate {});
			}

			Action<object> handlerFunc = obj => { handler((T)obj); };

			eventCallbacks[type] += handlerFunc;
			delegateLookup[handler] = handlerFunc;
		}

		/// <summary>
		/// Implementation of <see cref="IEventDispatcher.RemoveEventListener(Type, Action)"/>
		/// </summary>
		/// <param name="type">The type of event to be removed.</param>
		/// <param name="handler">The handler.</param>
		public void RemoveEventListener(Type type, Action handler)
		{
			if (eventCallbacks.ContainsKey(type) && delegateLookup.ContainsKey(handler))
			{
				eventCallbacks[type] -= delegateLookup[handler];
				delegateLookup.Remove(handler);

				if (eventCallbacks[type].GetInvocationList().Length == 1)
				{
					eventCallbacks.Remove(type);
				}
			}
		}

		/// <summary>
		/// Implementation of <see cref="IEventDispatcher.RemoveEventListener{T}(Action{T})"/>
		/// </summary>
		/// <typeparam name="T">The type of event to be removed.</typeparam>
		/// <param name="handler">The handler.</param>
		public void RemoveEventListener<T>(Action<T> handler) where T : IEvent
		{
			Type type = typeof(T);

			if (eventCallbacks.ContainsKey(type) && delegateLookup.ContainsKey(handler))
			{
				eventCallbacks[type] -= delegateLookup[handler];
				delegateLookup.Remove(handler);

				if (eventCallbacks[type].GetInvocationList().Length == 1)
				{
					eventCallbacks.Remove(type);
				}
			}
		}

		/// <summary>
		/// Remove all registered Listeners.
		/// </summary>
		public void RemoveAllListeners()
		{
			eventCallbacks.Clear();
			delegateLookup.Clear();
		}

		/// <summary>
		/// Implementation of <see cref="IEventDispatcher.Invoke(Type, object)"/>
		/// </summary>
		/// <param name="type">Type of event to be evoked.</param>
		/// <param name="evt">The event to be evoked.</param>
		public void Invoke(Type type, object evt)
		{
			Action<object> handler;

			if (eventCallbacks.TryGetValue(type, out handler))
			{
				if (handler != null)
				{
					handler.Invoke(evt);
				}
			}
		}

		/// <summary>
		/// Implementation of <see cref="IEventDispatcher.Invoke{T}(T)"/>
		/// </summary>
		/// <typeparam name="T">Type of event to be evoked.</typeparam>
		/// <param name="evt">The event to be evoked.</param>
		public void Invoke<T>(T evt) where T : IEvent
		{
			Type type = typeof(T);
			Invoke(type, evt);
		}
	}
}
