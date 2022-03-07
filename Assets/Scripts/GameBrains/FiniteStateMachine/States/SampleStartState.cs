using GameBrains.Entities;
using GameBrains.EventSystem;
using UnityEngine;

namespace GameBrains.FiniteStateMachine.States
{
    [CreateAssetMenu(menuName = "StateMachine/States/SampleStartState")]
    public class SampleStartState : State
    {
        State anotherSampleState; // cache state here to save multiple lookups

        public override void Enter(StateMachine stateMachine)
        {
            base.Enter(stateMachine);

            anotherSampleState = StateManager.Lookup(typeof(AnotherSampleState));
        }

        public override void Execute(StateMachine stateMachine)
        {
            base.Execute(stateMachine);

            if (Random.value < 0.2f)
            {
                var receiver = EntityManager.Find<Entity>("Receiver");
                if (receiver != null)
                {
                    EventManager.Instance.Fire(
                        Events.Message,
                        stateMachine.Owner.ID,
                        receiver.ID,
                        "Hello");
                }

                stateMachine.ChangeState(anotherSampleState);
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