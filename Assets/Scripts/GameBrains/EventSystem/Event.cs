namespace GameBrains.EventSystem
{
    public abstract partial class Event
    {
        // Initializes a new instance of the Event class.
        protected Event(
            int eventId,
            EventType eventType,
            Lifespan lifespan,
            double dispatchTime,
            int senderId,
            int receiverId,
            System.Delegate eventDelegate,
            System.Type eventDataType,
            object eventData)
        {
            EventId = eventId;
            EventType = eventType;
            EventLifespan = lifespan;
            DispatchTime = dispatchTime;
            SenderId = senderId;
            ReceiverId = receiverId;
            EventDelegate = eventDelegate;
            EventDataType = eventDataType;
            EventData = eventData;
        }

        protected internal Event() { }
        
        // The event id for tracking the event.
        public int EventId { get; protected set; }
        
        // Gets or sets the event type.
        public EventType EventType { get; protected set; }
        
        // Gets or sets the maximum duration of the event.
        public Lifespan EventLifespan { get; protected set; }
        
        // Gets or sets the time to dispatch the event. Events can be dispatched immediately, queued for the next
        // processing cycle or delayed for a specified amount of time. If a delay is necessary this field is stamped
        // with the time the event should be dispatched.
        public double DispatchTime { get; protected set; }
        
        // Gets or sets the ID of the game object that sent this event (or Event.SenderIDIrrelevant).
        public int SenderId { get; protected set; }
        
        // Gets or sets the ID of the intended receiver of this event (or Event.ReceiverIDIrrelevant).
        public int ReceiverId { get; protected set; }
        
        // Gets or sets the type of event data.
        public System.Type EventDataType { get; protected set; }
        
        // Gets or sets the event data (or null).
        public object EventData { get; protected set; }
        
        // Gets or sets the delegate to call when the event is triggered.
        public System.Delegate EventDelegate { get; protected set; }
        
        // Trigger event.
        internal abstract void Fire(System.Delegate eventDelegate);

        internal abstract void Send();
	}
}
