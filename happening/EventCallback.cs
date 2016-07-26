namespace BlurryRoots.Happening {

    /// <summary>
    /// Delegate used to store events.
    /// </summary>
    /// <typeparam name="TEventType">Event type.</typeparam>
    /// <param name="e">Occuring event.</param>
    public delegate void EventCallback<TEventType> (TEventType e);

}
