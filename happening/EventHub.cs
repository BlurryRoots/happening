namespace BlurryRoots.Happening {

    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Manages event handler subscribtions and is responsible for raising,
    /// processing and distributing events.
    /// </summary>
    public class EventHub : IEventDispatcher {

        /// <summary>
        /// Subscribes the given callback for being notifed
        /// when event is dispatched.
        /// </summary>
        /// <typeparam name="TEventType">Event type to subscribe to.</typeparam>
        /// <param name="callback">Handler to subscribe.</param>
        public void Subscribe<TEventType> (EventCallback<TEventType> callback) {
            var eventType = typeof (TEventType);

            // If there has not yet been one single subscriber
            // to this type of event
            if (!this.dispatchers.ContainsKey (eventType)) {
                // Create a new dispatcher for it
                this.dispatchers.Add (
                    eventType, new EventDispatcher<TEventType> ()
                );
            }

            // Retrieve dispatcher responsible for this event type
            var dispatcher =
                (EventDispatcher<TEventType>)this.dispatchers[eventType];

            // And subscribe with provided callback
            dispatcher.Subscribe (callback);
        }

        /// <summary>
        /// Stops the given callback from being notifed when event is dispatched.
        /// </summary>
        /// <typeparam name="TEventType">Event type to unsubscribe from.</typeparam>
        /// <param name="callback">Handler to unsubscribe.</param>
        public void Unsubscribe<TEventType> (EventCallback<TEventType> callback) {
            var eventType = typeof (TEventType);

            // Throw exception ff there does not
            // exist a dispatcher for this type of event
            if (false == this.dispatchers.ContainsKey (eventType)) {
                var msg = string.Format (
                    @"Could not remove {0} for event type {1}.
                    None registered previously!",
                    callback, eventType
                );
                throw new System.ArgumentException (msg, "TEventType");
            }

            // Retrieve dispatcher responsible for this type of event
            var dispatcher =
                (EventDispatcher<TEventType>)this.dispatchers[eventType];
            // And remove callback from the list of subscribers
            dispatcher.Unsubscribe (callback);
        }

        /// <summary>
        /// Raises a new event. It is stored until
        /// <see cref="DispatchAllRaisedEvents"/> is called.
        /// </summary>
        /// <typeparam name="TEventType">Type of event to raise.</typeparam>
        /// <param name="e">New event.</param>
        public void Raise<TEventType> (TEventType e) {
            var eventType = typeof (TEventType);

            // If there has not yet been one single subscriber to
            // this type of event
            if (!this.dispatchers.ContainsKey (eventType)) {
                // Exit without doing anything
                return;
            }

            // Retrieve dispatcher responsible for this type of event
            var dispatcher =
                (EventDispatcher<TEventType>)this.dispatchers[eventType];
            // Store event for dispatch later
            dispatcher.Raise (e);
        }

        /// <summary>
        /// Dispatches all previously raised events.
        /// </summary>
        public void DispatchAllRaisedEvents () {
            // Go through all dispatchers
            foreach (var item in this.dispatchers) {
                // Get dispatcher from dictionary entry
                var dispatcher = item.Value;
                // And let it dispatch all raised events
                dispatcher.DispatchAllRaisedEvents ();
            }
        }

        /// <summary>
        /// Creates a new EventManager.
        /// </summary>
        public EventHub () {
            this.dispatchers = new Dictionary<System.Type, IEventDispatcher> ();
        }

        /// <summary>
        /// Holds all dispatchers.
        /// </summary>
        private Dictionary<Type, IEventDispatcher> dispatchers;

    }

}
