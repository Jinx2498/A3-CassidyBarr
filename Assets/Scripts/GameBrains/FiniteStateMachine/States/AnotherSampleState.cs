using GameBrains.Entities;
using GameBrains.EventSystem;
using UnityEngine;

namespace GameBrains.FiniteStateMachine.States
{
    [CreateAssetMenu(menuName = "StateMachine/States/AnotherSampleState")]
    public class AnotherSampleState : State
    {
        State sampleStartState; // cache state here to save multiple lookups
        Entity receiver; // cache the receiver here to save multiple lookups

        public override void Enter(StateMachine stateMachine)
        {
            base.Enter(stateMachine);

            sampleStartState = StateManager.Lookup(typeof(SampleStartState));
            receiver = EntityManager.Find<Entity>("Receiver");
        }

        public override void Execute(StateMachine stateMachine)
        {
            base.Execute(stateMachine);

            if (Random.value < 0.3f)
            {
                EventManager.Instance.Fire(
                    Events.MyOtherEvent,
                    stateMachine.Owner.ID,
                    receiver.ID,
                    "Test");

                stateMachine.ChangeState(sampleStartState);
            }
        }

        public override void Exit(StateMachine stateMachine)
        {
            base.Exit(stateMachine);
        }

        public override bool HandleEvent<TEvent>(
            StateMachine stateMachine,
            Event<TEvent> eventArguments)
        {
            return base.HandleEvent(stateMachine, eventArguments);
        }
    }
}