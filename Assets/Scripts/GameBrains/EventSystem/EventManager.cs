using System.Collections.Generic;
using GameBrains.DataStructures;
using GameBrains.Extensions.MonoBehaviours;
using GameBrains.GUI;
using UnityEngine;
namespace GameBrains.EventSystem
{
    // Manager for events. Events can be fired for immediate processing or queued for later processing. Objects that
    // subscribe to an event are notified when it is processed via its event delegate. Objects cease to be notified
    // when they unsubscribe from an event. Events can also be scheduled and can be directed to specified receivers.
    public sealed partial class EventManager : ExtendedMonoBehaviour
    {
        // The id of the sender is irrelevant (system generated).
        public const int SenderIDIrrelevant = -1;
        
        // The id of the receiver is irrelevant (system generated).
        public const int ReceiverIDIrrelevant = -1;
        
        // Event should be dispatched without delay.
        public const double NoDelay = 0.0f;
        
        // Dictionary used to get event subscribers by event type.
        readonly Dictionary<EventType, List<Subscription>> eventSubscribers =
            new Dictionary<EventType, List<Subscription>>();

        // Priority queue to gather events as they are enqueued.
        PriorityQueue<Event, double> eventGatherQueue =
            new PriorityQueue<Event, double>(PriorityQueue<Event, double>.PriorityOrder.LowFirst);
        
        // Priority queue of events taken from the eventGatherQueue that can now be processed.
        PriorityQueue<Event, double> eventProcessQueue =
            new PriorityQueue<Event, double>(PriorityQueue<Event, double>.PriorityOrder.LowFirst);
        
        // The next event id.
        int nextEventId;
        
        // A value indicating whether the event manager is currently processing events.
        bool isProcessingEvents;
	
        static EventManager instance;
	
        public static EventManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<EventManager>();
                }
			
                return instance;
            }
        }
        
        [SerializeField] public List<MessageViewer> defaultMessageViewers;
        public List<MessageViewer> DefaultMessageViewers
        {
            get => defaultMessageViewers;
            set => defaultMessageViewers = value;
        }

        public void Subscribe<T>(
            EventType eventType,
            EventDelegate<T> eventDelegate)
        {
            Subscribe(eventType, eventDelegate, null);
        }

        public void Subscribe<T>(
            EventType eventType,
            EventDelegate<T> eventDelegate,
            object eventKey)
        {
            var subscriptionToAdd = new Subscription(eventDelegate, eventKey);

            lock (eventSubscribers)
            {
                if (eventSubscribers.TryGetValue(eventType, out List<Subscription> eventSubscriptionList) &&
                    eventSubscriptionList != null)
                {
                    if (!eventSubscriptionList.Contains(subscriptionToAdd))
                    {
                        eventSubscriptionList.Add(subscriptionToAdd);
                    }

                    return;
                }

                eventSubscribers[eventType] = new List<Subscription> { subscriptionToAdd };
            }
        }

        public void Unsubscribe<T>(
            EventType eventType,
            EventDelegate<T> eventDelegate)
        {
            Unsubscribe(eventType, eventDelegate, null);
        }

        public void Unsubscribe<T>(
            EventType eventType,
            EventDelegate<T> eventDelegate,
            object eventKey)
        {
            var subscriptionToRemove = new Subscription(eventDelegate, eventKey);

            lock (eventSubscribers)
            {
                if (eventSubscribers.TryGetValue(eventType, out List<Subscription> eventSubscriptionList) &&
                    eventSubscriptionList != null)
                {
                    eventSubscriptionList.Remove(subscriptionToRemove);
                }
            }
        }

        public int Enqueue<T>(
            EventType eventType,
            T eventData)
        {
            return Enqueue(eventType, Event.Lifespans.Cycle, NoDelay, SenderIDIrrelevant, ReceiverIDIrrelevant, null, eventData, null);
        }

        public int Enqueue<T>(
            EventType eventType,
            double delay,
            T eventData)
        {
            return Enqueue(eventType, Event.Lifespans.Level, delay, SenderIDIrrelevant, ReceiverIDIrrelevant, null, eventData, null);
        }

        public int Enqueue<T>(
            EventType eventType,
            double delay,
            EventDelegate<T> eventDelegate,
            T eventData)
        {
            return Enqueue(eventType, Event.Lifespans.Level, delay, SenderIDIrrelevant, ReceiverIDIrrelevant, eventDelegate, eventData, null);
        }

        public int Enqueue<T>(
            EventType eventType,
            EventDelegate<T> eventDelegate,
            T eventData)
        {
            return Enqueue(eventType, Event.Lifespans.Cycle, NoDelay, SenderIDIrrelevant, ReceiverIDIrrelevant, eventDelegate, eventData, null);
        }

        public int Enqueue<T>(
            EventType eventType,
            double delay,
            int receiverId,
            T eventData)
        {
            return Enqueue(eventType, Event.Lifespans.Level, delay, SenderIDIrrelevant, receiverId, null, eventData, null);
        }

        public int Enqueue<T>(
            EventType eventType,
            int receiverId,
            T eventData)
        {
            return Enqueue(eventType, Event.Lifespans.Cycle, NoDelay, SenderIDIrrelevant, receiverId, null, eventData, null);
        }

        public int Enqueue<T>(
            EventType eventType,
            double delay,
            int senderId,
            int receiverId,
            T eventData)
        {
            return Enqueue(eventType, Event.Lifespans.Level, delay, senderId, receiverId, null, eventData, null);
        }

        public int Enqueue<T>(
            EventType eventType,
            int senderId,
            int receiverId,
            T eventData)
        {
            return Enqueue(eventType, Event.Lifespans.Cycle, NoDelay, senderId, receiverId, null, eventData, null);
        }

        public int Enqueue<T>(
            EventType eventType,
            int senderId,
            int receiverId,
            EventDelegate<T> eventDelegate,
            T eventData,
            object eventKey)
        {
            return Enqueue(eventType, Event.Lifespans.Cycle, NoDelay, senderId, receiverId, eventDelegate, eventData, eventKey);
        }

        public int Enqueue<T>(
            EventType eventType,
            double delay,
            int senderId,
            int receiverId,
            EventDelegate<T> eventDelegate,
            T eventData,
            object eventKey)
        {
            return Enqueue(eventType, Event.Lifespans.Level, delay, senderId, receiverId, eventDelegate, eventData, eventKey);
        }

        public int Enqueue<T>(
            EventType eventType,
            Event.Lifespan lifespan,
            T eventData)
        {
            return Enqueue(eventType, lifespan, NoDelay, SenderIDIrrelevant, ReceiverIDIrrelevant, null, eventData, null);
        }

        public int Enqueue<T>(
            EventType eventType,
            Event.Lifespan lifespan,
            double delay,
            T eventData)
        {
            return Enqueue(eventType, lifespan, delay, SenderIDIrrelevant, ReceiverIDIrrelevant, null, eventData, null);
        }

        public int Enqueue<T>(
            EventType eventType,
            Event.Lifespan lifespan,
            double delay,
            EventDelegate<T> eventDelegate,
            T eventData)
        {
            return Enqueue(eventType, lifespan, delay, SenderIDIrrelevant, ReceiverIDIrrelevant, eventDelegate, eventData, null);
        }

        public int Enqueue<T>(
            EventType eventType,
            Event.Lifespan lifespan,
            EventDelegate<T> eventDelegate,
            T eventData)
        {
            return Enqueue(eventType, lifespan, NoDelay, SenderIDIrrelevant, ReceiverIDIrrelevant, eventDelegate, eventData, null);
        }

        public int Enqueue<T>(
            EventType eventType,
            Event.Lifespan lifespan,
            double delay,
            int receiverId,
            T eventData)
        {
            return Enqueue(eventType, lifespan, delay, SenderIDIrrelevant, receiverId, null, eventData, null);
        }

        public int Enqueue<T>(
            EventType eventType,
            Event.Lifespan lifespan,
            int receiverId,
            T eventData)
        {
            return Enqueue(eventType, lifespan, NoDelay, SenderIDIrrelevant, receiverId, null, eventData, null);
        }

        public int Enqueue<T>(
            EventType eventType,
            Event.Lifespan lifespan,
            double delay,
            int senderId,
            int receiverId,
            T eventData)
        {
            return Enqueue(eventType, lifespan, delay, senderId, receiverId, null, eventData, null);
        }

        public int Enqueue<T>(
            EventType eventType,
            Event.Lifespan lifespan,
            int senderId,
            int receiverId,
            T eventData)
        {
            return Enqueue(eventType, lifespan, NoDelay, senderId, receiverId, null, eventData, null);
        }

        public int Enqueue<T>(
            EventType eventType,
            Event.Lifespan lifespan,
            int senderId,
            int receiverId,
            EventDelegate<T> eventDelegate,
            T eventData,
            object eventKey)
        {
            return Enqueue(eventType, lifespan, NoDelay, senderId, receiverId, eventDelegate, eventData, eventKey);
        }

        public int Enqueue<T>(
            EventType eventType,
            Event.Lifespan lifespan,
            double delay,
            int senderId,
            int receiverId,
            EventDelegate<T> eventDelegate,
            T eventData,
            object eventKey)
        {
            var eventToSchedule =
                Event<T>.Obtain(
                    ++nextEventId,
                    eventType,
                    lifespan,
                    Time.time + delay, // dispatch time
                    senderId,
                    receiverId,
                    eventDelegate,
                    eventData);

            lock (eventGatherQueue)
            {
                eventGatherQueue.Enqueue(eventToSchedule, eventToSchedule.DispatchTime);
            }

            return eventToSchedule.EventId;
        }

        public void Fire<T>(
            EventType eventType,
            T eventData)
        {
            Fire(eventType, SenderIDIrrelevant, ReceiverIDIrrelevant, null, eventData, null);
        }

        public void Fire<T>(
            EventType eventType,
            EventDelegate<T> eventDelegate,
            T eventData)
        {
            Fire(eventType, SenderIDIrrelevant, ReceiverIDIrrelevant, eventDelegate, eventData, null);
        }

        public void Fire<T>(
            EventType eventType,
            int receiverId,
            T eventData)
        {
            Fire(eventType, SenderIDIrrelevant, receiverId, eventData);
        }

        public void Fire<T>(
            EventType eventType,
            int senderId,
            int receiverId,
            T eventData)
        {
            Fire(eventType, senderId, receiverId, null, eventData, null);
        }

        public void Fire<T>(
            EventType eventType,
            int senderId,
            int receiverId,
            EventDelegate<T> eventDelegate,
            T eventData,
            object eventKey)
        {
            var eventToFire =
                Event<T>.Obtain(++nextEventId, eventType, Event.Lifespans.Cycle, NoDelay, senderId, receiverId, eventDelegate, eventData);
            Fire(eventToFire);
        }
        
        // Remove the event with the given event ID.
        public bool Remove(int eventId)
        {
            lock (eventGatherQueue)
            {
                return eventGatherQueue.Remove(i => i.EventId == eventId);
            }
        }

        public void RemoveAll(Event.Lifespan lifespan)
        {
            lock (eventGatherQueue)
            {
                eventGatherQueue.Remove(i => i.EventLifespan == lifespan);
            }
        }
        
        // Process all events.
        public override void Update()
        {
            base.Update();
            
            // Fire a (non-queued) update event per cycle. Trigger processes the
            // event immediately without putting it on the event queue.
            Fire(Events.ImmediateUpdate, Time.time);

            // post a (queued) update event per cycle.
            Enqueue(Events.QueuedUpdate, Time.time);

            while (ProcessEvents())
            {
            }
        }
        
        // Processes all events queued up since last ProcessEvents call.
        bool ProcessEvents()
        {
            // if already processing event, leave.
            if (isProcessingEvents)
            {
                return false;
            }

            // if no events to process, leave.
            if (eventGatherQueue.Count == 0 || eventGatherQueue.Peek().Priority > Time.time)
            {
                return false;
            }

            isProcessingEvents = true;

            if (eventProcessQueue.Count != 0)
            {
                System.Diagnostics.Debug.WriteLine("EventManager: event process list should be empty at this point.");
            }

            lock (eventGatherQueue)
            {
                // We use a double buffer scheme (gather, process) to minimize lock time.
                Swap(ref eventProcessQueue, ref eventGatherQueue);
            }

            while (eventProcessQueue.Count > 0 && eventProcessQueue.Peek().Priority <= Time.time)
            {
                Fire(eventProcessQueue.Dequeue().Value);
            }

            lock (eventGatherQueue)
            {
                // transfer remaining events
                while (eventProcessQueue.Count > 0)
                {
                    var queueItem = eventProcessQueue.Dequeue();
                    var unprocessedEvent = queueItem.Value;
                    if (unprocessedEvent.EventLifespan == Event.Lifespans.Cycle)
                    {
                        //unprocessedEvent.Recycle(); // shouldn't happen. If it does, event is skipped.
                    }
                    else
                    {
                        eventGatherQueue.Enqueue(queueItem);
                    }
                }
            }

            eventProcessQueue.Clear();

            isProcessingEvents = false;

            // if we get here, then some events where processed.
            return true;
        }
        
        // Fire an event (call the subscriber delegates).
        void Fire(Event eventToFire)
        {
            // call subscriber delegates
            if (eventToFire.EventType != Events.Message &&
                eventSubscribers.TryGetValue(eventToFire.EventType, out List<Subscription> subscriptionList))
            {
                if (subscriptionList != null)
                {
                    for (var i = 0; i < subscriptionList.Count; i++)
                    {
                        eventToFire.Fire(subscriptionList[i].EventDelegate);
                    }
                }
            }

            if (eventToFire.EventDelegate != null)
            {
                eventToFire.Fire(eventToFire.EventDelegate);
            }
            else
            {
                eventToFire.Send(); // Entity messages and default message viewers
            }

            //eventToFire.Recycle(); //TODO: Investigate issue of recycling before completed
        }
        
        // Swap references to two objects.
        // TODO: This should be in a Utility class
        void Swap<T>(ref T a, ref T b) { (a, b) = (b, a); }
    }
}