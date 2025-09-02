using System;

namespace Util.Events
{
    /// <summary>
    /// Use this for global event handling.
    /// </summary>
    public static class GlobalEvents
    {
        private static IEventDispatcher _eventDispatcher;

        static GlobalEvents() => _eventDispatcher = new EventDispatcher();

        /// <summary>
        /// Static implementation of <see cref="EventDispatcher.AddEventListener(Type, Action)"/>.
        /// </summary>
        /// <param name="type">The type of event.</param>
        /// <param name="handler">The handler.</param>
        public static void AddEventListener(Type type, Action handler)
        {
            _eventDispatcher.AddEventListener(type, handler);
        }

        /// <summary>
        /// Static implementation of <see cref="EventDispatcher.AddEventListener{T}(Action{T})"/>.
        /// </summary>
        /// <typeparam name="T">The type of event.</typeparam>
        /// <param name="handler">The handler.</param>
        public static void AddEventListener<T>(Action<T> handler) where T : IEvent
        {
            _eventDispatcher.AddEventListener(handler);
        }

        /// <summary>
        /// Static implementation of <see cref="EventDispatcher.RemoveEventListener(Type, Action)"/>.
        /// </summary>
        /// <param name="type">The type of event.</param>
        /// <param name="handler">The handler.</param>
        public static void RemoveEventListener(Type type, Action handler)
        {
            _eventDispatcher.RemoveEventListener(type, handler);
        }

        /// <summary>
        /// Static implementation of <see cref="EventDispatcher.RemoveEventListener{T}(Action{T})"/>.
        /// </summary>
        /// <typeparam name="T">The type of event.</typeparam>
        /// <param name="handler">The handler.</param>
        public static void RemoveEventListener<T>(Action<T> handler) where T : IEvent
        {
            _eventDispatcher.RemoveEventListener(handler);
        }

        /// <summary>
        /// Static implementation of <see cref="EventDispatcher.Invoke(Type, object)"/>.
        /// </summary>
        /// <param name="type">The type of event.</param>
        /// <param name="evt">The event.</param>
        public static void Invoke(Type type, object evt)
        {
            _eventDispatcher.Invoke(type, evt);
        }

        /// <summary>
        /// Static implementation of <see cref="EventDispatcher.Invoke{T}(T)"/>.
        /// </summary>
        /// <typeparam name="T">The type of event.</typeparam>
        /// <param name="evt">The event.</param>
        public static void Invoke<T>(T evt) where T : IEvent
        {
            _eventDispatcher.Invoke(evt);
        }
    }
}
