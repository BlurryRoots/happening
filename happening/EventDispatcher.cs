namespace BlurryRoots.Happening {
        
    using System.Collections.Generic;

    /// <summary>
    /// Helper class used for dispatching a specific type of event.
    /// </summary>
    /// <typeparam name="TEventType">Event type to dispatch.</typeparam>
    public class EventDispatcher<TEventType>: IEventDispatcher {

        /// <summary>
        /// Returns the number of events currently waiting for processing.
        /// </summary>
        public int CurrentlyActiveEvents {
            get { return this.ActiveQueue.Count; }
        }

        /// <summary>
        /// Subscribes given callback to handle any event of type TEventType.
        /// </summary>
        /// <exception cref="System.Exception">Is thrown if callback has
        /// already been a subscriber.</exception>
        /// <param name="callback">New subscriber.</param>
        public void Subscribe (EventCallback<TEventType> callback) {
            // Check if the callback has already been registered as a callback
            if (this.subscribers.Contains (callback)) {
                var msg = string.Format (
                    "Could not add {0}! Has already been subscribed before!",
                    callback
                );

                throw new System.Exception (msg);
            }

            // Add the callback to the list of subscribers
            this.subscribers.Add (callback);
        }

        /// <summary>
        /// Unsubscribes given handler from this dispatcher.
        /// </summary>
        /// <exception cref="System.Exception">Is thrown if callback
        /// has not been a subscriber.</exception>
        /// <param name="callback">Subscriber to remove.</param>
        public void Unsubscribe (EventCallback<TEventType> callback) {
            // If this handler has previously subscribed to this dipatcher
            if (false == this.subscribers.Contains (callback)) {
                var msg = string.Format (
                    "Could not remove {0}! Has not been subcribed before!",
                    callback
                );

                throw new System.Exception (msg);
            }

            // Remove it
            this.subscribers.Remove (callback);
        }

        /// <summary>
        /// Raises a new event. It is stored until
        /// <see cref="DispatchAllRaisedEvents"/> is called.
        /// </summary>
        /// <param name="e">New event to raise.</param>
        public void Raise (TEventType e) {
            this.ActiveQueue.Enqueue (e);
        }

        /// <summary>
        /// Directly dispatches given event to all subscribers.
        /// </summary>
        /// <param name="e">Event to dispatch.</param>
        public void Fire (TEventType e) {
            this.Dispatch (e);
        }

        /// <summary>
        /// Dispatches event to all subscribers.
        /// </summary>
        public void DispatchAllRaisedEvents () {
            // Swap active queue so that events producing new events
            // do not interfere with currently processed queue
            this.SwapActiveQueue ();

            // Get current active queue
            var processingQueue = this.InactiveQueue;
            // While there are still events in the queue
            while (0 < processingQueue.Count) {
                // Remove from queue
                var e = processingQueue.Dequeue ();
                // And dispatch to subscribers
                this.Dispatch (e);
            }
        }

        /// <summary>
        /// Creates a new EventDispatcher.
        /// </summary>
        public EventDispatcher () {
            this.subscribers = new List<EventCallback<TEventType>> ();

            // Create the two event queues
            this.eventQueues = new Queue<TEventType>[2];
            this.eventQueues[0] = new Queue<TEventType> ();
            this.eventQueues[1] = new Queue<TEventType> ();

            // Set the currently active queue to be the first one
            this.activeQueueIndex = 0;
        }

        /// <summary>
        /// Dispatches given event to all subscribed handlers.
        /// </summary>
        /// <param name="e">Event to process.</param>
        private void Dispatch (TEventType e) {
            // Make a copy of all handlers. It might be possible that
            // handlers subscribe or unsubscribe while being processed.
            var handlerList =
                new List<EventCallback<TEventType>> (this.subscribers);

            // Let all handlers process the event.
            while (0 < handlerList.Count) {
                // Fetch handler
                var handler = handlerList[0];
                // Remove him from processing list
                handlerList.RemoveAt (0);

                // Invoke handler.
                handler.Invoke (e);
            }
        }

        /// <summary>
        /// Signals the system to swap the currently active queue
        /// </summary>
        private void SwapActiveQueue () {
            // Change index to 1 if 0 and to 0 if 1.
            if (0 == this.activeQueueIndex) {
                this.activeQueueIndex = 1;
            }
            else if (1 == this.activeQueueIndex) {
                this.activeQueueIndex = 0;
            }
            else {
                throw new System.Exception (
                    "activeQueueIndex should not have values other than 1 or 0"
                );
            }
        }

        /// <summary>
        /// Returns the currently active queue. (Used for external events)
        /// </summary>
        private Queue<TEventType> ActiveQueue {
            get { return this.eventQueues[this.activeQueueIndex]; }
        }

        /// <summary>
        /// Returns the currently inactive queue.
        /// </summary>
        private Queue<TEventType> InactiveQueue {
            get { return this.eventQueues[1 - this.activeQueueIndex]; }
        }

        /// <summary>
        /// Holds all subscribers.
        /// </summary>
        private IList<EventCallback<TEventType>> subscribers;
        /// <summary>
        /// Event queue.
        /// </summary>
        private Queue<TEventType>[] eventQueues;
        /// <summary>
        /// Index of queue used to store new external event.
        /// </summary>
        private int activeQueueIndex;

    }

}
