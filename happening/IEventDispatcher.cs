namespace BlurryRoots.Happening {

    /// <summary>
    /// Used to describe an event dispatcher.
    /// </summary>
    public interface IEventDispatcher {

        /// <summary>
        /// Dispatches all events raised within the context this dispatcher.
        /// </summary>
        void DispatchAllRaisedEvents ();

    }

}
