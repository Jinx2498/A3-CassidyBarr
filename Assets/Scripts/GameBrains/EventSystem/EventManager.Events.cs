using System.ComponentModel;

namespace GameBrains.EventSystem
{
    public static partial class Events
    {
        [Description("Invalid")]
        public static readonly EventType Invalid = (EventType)Count++;

        [Description("Message")]
        public static readonly EventType Message = (EventType)Count++;

        [Description("ImmediateUpdate")]
        public static readonly EventType ImmediateUpdate = (EventType)Count++;

        [Description("QueuedUpdate")]
        public static readonly EventType QueuedUpdate = (EventType)Count++;
    }
}
