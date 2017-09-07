using System;
using UnityEngine;

namespace Util.Events
{
	/// <summary>
	/// Use this for local event handling.
	/// </summary>
	public class LocalEvents : MonoBehaviour, IEventDispatcher
	{
		private IEventDispatcher eventDispatcher;
		private IEventDispatcher EventDispatcher
		{
			get
			{
				if (eventDispatcher == null)
				{
					eventDispatcher = new EventDispatcher();
				}

				return eventDispatcher;
			}
		}

		/// <summary>
		/// Implementation of <see cref="IEventDispatcher.AddEventListener(Type, Action)"/>.
		/// </summary>
		/// <param name="type">The type of event.</param>
		/// <param name="handler">The handler.</param>
		public void AddEventListener(Type type, Action handler)
		{
			EventDispatcher.AddEventListener(type, handler);
		}

		/// <summary>
		/// Implementation of <see cref="IEventDispatcher.AddEventListener{T}(Action{T})"/>.
		/// </summary>
		/// <typeparam name="T">The type of event.</typeparam>
		/// <param name="handler">The handler.</param>
		public void AddEventListener<T>(Action<T> handler) where T : IEvent
		{
			EventDispatcher.AddEventListener(handler);
		}

		/// <summary>
		/// Implementation of <see cref="IEventDispatcher.RemoveEventListener(Type, Action)"/>.
		/// </summary>
		/// <param name="type">The type of event.</param>
		/// <param name="handler">The handler.</param>
		public void RemoveEventListener(Type type, Action handler)
		{
			EventDispatcher.RemoveEventListener(type, handler);
		}

		/// <summary>
		/// Implementation of <see cref="IEventDispatcher.RemoveEventListener{T}(Action{T})"/>.
		/// </summary>
		/// <typeparam name="T">The type of event.</typeparam>
		/// <param name="handler">The handler.</param>
		public void RemoveEventListener<T>(Action<T> handler) where T : IEvent
		{
			EventDispatcher.RemoveEventListener(handler);
		}

		/// <summary>
		/// Implementation of <see cref="IEventDispatcher.Invoke(Type, Action)"/>.
		/// </summary>
		/// <param name="type">The type of event.</param>
		/// <param name="evt">The event.</param>
		public void Invoke(Type type, object evt)
		{
			EventDispatcher.Invoke(type, evt);
		}

		/// <summary>
		/// Implementation of <see cref="IEventDispatcher.Invoke{T}(Action{T})"/>.
		/// </summary>
		/// <typeparam name="T">The type of event.</typeparam>
		/// <param name="evt">The event.</param>
		public void Invoke<T>(T evt) where T : IEvent
		{
			EventDispatcher.Invoke(evt);
		}
	}
}
