namespace GameBrains.EventSystem
{
    public sealed class EventType
    {
        readonly int eventType;

        public EventType(int eventType)
        {
            this.eventType = eventType;
        }

        public static implicit operator int(EventType eventType)
        {
            return eventType.eventType;
        }

        public static explicit operator EventType(int eventType)
        {
            return new EventType(eventType);
        }

        public override string ToString()
        {
            return Events.GetDescription(this);
        }
    }
}