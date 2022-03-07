using GameBrains.EventSystem;
using GameBrains.Extensions.MonoBehaviours;
using UnityEngine;

namespace Testing
{
    // Add as component in the Receiver hierarchy
    public class W15MyEventHandler : ExtendedMonoBehaviour, IEventHandlingComponent
    {
        // Entity should forward event here
        public bool HandleEvent<T>(Event<T> eventArguments)
        {
            if (eventArguments.EventType == Events.MyEntityEvent)
            {
                if (VerbosityDebug)
                {
                    Debug.Log($"Entity MyEvent {name} {eventArguments}");
                }
                return true;
            }

            return false;
        }
    }
}