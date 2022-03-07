using GameBrains.EventSystem;
using UnityEngine;

namespace GameBrains.FiniteStateMachine.States
{
    [CreateAssetMenu(menuName = "StateMachine/States/SampleGlobalStartState")]
    public class SampleGlobalStartState : State
    {
        public override void Enter(StateMachine stateMachine)
        {
            base.Enter(stateMachine);
        }

        public override void Execute(StateMachine stateMachine)
        {
            base.Execute(stateMachine);
        }

        public override void Exit(StateMachine stateMachine)
        {
            base.Exit(stateMachine);
        }

        public override bool HandleEvent<TEvent>(
            StateMachine stateMachine,
            Event<TEvent> eventArguments)
        {
            if (eventArguments.EventType == Events.MyOtherEvent
                && eventArguments.ReceiverId == stateMachine.Owner.ID)
            {
                if (VerbosityDebug)
                {
                    Debug.Log($"Event {eventArguments.EventType} received by {stateMachine.Owner.name} at time: {Time.time}");
                }

                EventManager.Instance.Fire(
                    Events.Message,
                    stateMachine.Owner.ID,
                    stateMachine.Owner.ID,
                    "Test");

                return true;
            }

            return base.HandleEvent(stateMachine, eventArguments);
        }
    }
}