using GameBrains.Entities;

namespace GameBrains.EventSystem
{
	// The base class for events.
   public sealed class Event<T> : Event
    {
	    // Initializes a new instance of the Event class.
	    Event(
            int eventId,
            EventType eventType,
            Lifespan lifespan,
            double dispatchTime,
            int senderId,
            int receiverId,
            EventDelegate<T> eventDelegate,
            T eventData)
            : base(eventId, eventType, lifespan, dispatchTime, senderId, receiverId, eventDelegate, typeof(T), eventData)
        {
        }

		Event()
        {
        }
		
        // Gets the event data (may be null).
        public new T EventData
        {
            get => (T)base.EventData;

            private set => base.EventData = value;
        }
        
        // Gets the delegate to call when the event is triggered.
        public new EventDelegate<T> EventDelegate
        {
            get => (EventDelegate<T>)base.EventDelegate;

            private set => base.EventDelegate = value;
        }
		
		public static Event<T> Obtain(
            int eventId,
            EventType eventType,
            Lifespan lifespan,
            double dispatchTime,
            int senderId,
            int receiverId,
            EventDelegate<T> eventDelegate,
            T eventData)
        {
			// TODO: make events poolable to reduce garbage
            return new Event<T>(
                    eventId,
                    eventType,
                    lifespan,
                    dispatchTime,
                    senderId,
                    receiverId,
                    eventDelegate,
                    eventData);
		}
		
        // Returns a System.String that represents the event.
        public override string ToString()
        {
            return string.Format(
                "Id:{0}, Type:{1}, Lifespan:{2} Sender:{3}, Receiver:{4}, Data:{5}",
                EventId,
                EventType,
                EventLifespan,
                SenderId,
                ReceiverId,
                EventData);
        }
        
        // Trigger event.
        internal override void Fire(System.Delegate delegateToFire)
        {
	        var eventDelegate = delegateToFire as EventDelegate<T>;

	        if (eventDelegate != null)
            {
                eventDelegate(this);
            }
        }

        internal override void Send()
        {
			if (ReceiverId != EventManager.ReceiverIDIrrelevant)
			{
				Entity entity = EntityManager.Find<Entity>(ReceiverId);
				if (entity != null)
				{
					entity.HandleEvent(this);
				}
			}
			else
			{
				foreach (var messageViewer in EventManager.Instance.DefaultMessageViewers)
				{
					messageViewer.HandleEvent(this);
				}
			}
        }
	}
}