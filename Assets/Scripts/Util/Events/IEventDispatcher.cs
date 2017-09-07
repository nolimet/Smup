using System;

namespace Util.Events
{
	/// <summary>
	/// Event dispatcher interface.
	/// </summary>
	public interface IEventDispatcher
	{
		/// <summary>
		/// Add an event listener.
		/// </summary>
		/// <param name="type">The type of the event.</param>
		/// <param name="handler">The handler for the event.</param>
		void AddEventListener(Type type, Action handler);

		/// <summary>
		/// Add an event listener.
		/// </summary>
		/// <typeparam name="T">The type of the event.</typeparam>
		/// <param name="handler">The handler for the event.</param>
		void AddEventListener<T>(Action<T> handler) where T : IEvent;

		/// <summary>
		/// Remove an event listener.
		/// </summary>
		/// <param name="type">The type of the event.</param>
		/// <param name="handler">The handler.</param>
		void RemoveEventListener(Type type, Action handler);

		/// <summary>
		/// Remove an event listener.
		/// </summary>
		/// <typeparam name="T">The type of the event.</typeparam>
		/// <param name="handler">The handler.</param>
		void RemoveEventListener<T>(Action<T> handler) where T : IEvent;

		/// <summary>
		/// Invoke an event.
		/// </summary>
		/// <param name="type">The type of event to be invoked.</param>
		/// <param name="evt">The event to be invoked.</param>
		void Invoke(Type type, object evt);

		/// <summary>
		/// Invoke an event.
		/// </summary>
		/// <typeparam name="T">The type of event to be invoked.</typeparam>
		/// <param name="evt">The event to be invoked.</param>
		void Invoke<T>(T evt) where T : IEvent;
	}
}
