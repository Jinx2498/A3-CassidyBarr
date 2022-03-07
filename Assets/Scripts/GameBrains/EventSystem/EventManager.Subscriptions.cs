namespace GameBrains.EventSystem
{
    // Manager for events. Events can be fired for immediate processing or queued for later
    // processing. Objects that subscribe to an event are notified when it is processed via its
    // event delegate. Objects cease to be notified when they unsubscribe from an event.
    public sealed partial class EventManager
    {
        // An event subscription record.
        struct Subscription
        {
            // The event delegate.
            public System.Delegate EventDelegate;
            
            // Gets the key used to identify subscriptions.
            public object EventKey;
            
            // Initializes a new instance of the Subscription struct.
            public Subscription(System.Delegate eventDelegate, object eventKey)
                : this()
            {
                EventDelegate = eventDelegate;
                EventKey = eventKey;
            }
        }
    }
}