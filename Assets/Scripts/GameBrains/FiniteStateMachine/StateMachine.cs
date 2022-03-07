#region Derived from Mat Buckland's Programming Game AI by Example
// Please see Buckland's book for the original C++ code and examples and the Pluggable AI tutorial.
#endregion

using GameBrains.Entities;
using GameBrains.EventSystem;
using GameBrains.Extensions.Attributes;
using GameBrains.Extensions.MonoBehaviours;
using GameBrains.Timers;
using UnityEngine;

namespace GameBrains.FiniteStateMachine
{
    // State machine class. Create some states to give your entities FSM functionality.
    public sealed class StateMachine : ExtendedMonoBehaviour, IEventHandlingComponent
    {
        [ReadOnlyInPlaymode]
		[SerializeField] State globalStartState;
		public State GlobalStartState { get => globalStartState; set => globalStartState = value; }

		[ReadOnlyInPlaymode]
		[SerializeField] State startState;
		public State StartState { get => startState; set => startState = value; }

        // Gets and sets the global state that is continuously active.
        // This state continuously updates and does not transition to another state.
        // This is intended to be used for functionality that repeats and
        // is independent of the current state.
        public State GlobalState { get; private set; }
        
        // Gets and sets the current state.
        public State CurrentState { get; private set; }
        
        // Gets and sets the previous state.
        public State PreviousState { get; private set; }
        
        // Gets and sets the entity associated with this state.
        public Entity Owner { get; set; }

        #region Regulator

        [SerializeField] float minimumTimeMs;
        [SerializeField] float maximumDelayMs;
        [SerializeField] RegulatorMode regulatorMode;
        [SerializeField] DelayDistribution delayDistribution;

        [SerializeField] AnimationCurve distributionCurve
            = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));

        Regulator stateMachineUpdateRegulator;
        public Regulator StateMachineUpdateRegulator => stateMachineUpdateRegulator;

        public float MinimumTimeMs
        {
            get => minimumTimeMs;
            set => minimumTimeMs = value;
        }

        public float MaximumDelayMs
        {
            get => maximumDelayMs;
            set => maximumDelayMs = value;
        }

        public RegulatorMode RegulatorMode
        {
            get => regulatorMode;
            set => regulatorMode = value;
        }

        public DelayDistribution DelayDistribution
        {
            get => delayDistribution;
            set => delayDistribution = value;
        }

        public AnimationCurve DistributionCurve
        {
            get => distributionCurve;
            set => distributionCurve = value;
        }

        #endregion Regulator

        public override void Start()
        {
            base.Start();

            stateMachineUpdateRegulator ??= new Regulator
            {
                MinimumTimeMs = minimumTimeMs,
                MaximumDelayMs = maximumDelayMs,
                Mode = regulatorMode,
                DelayDistribution = delayDistribution,
                DistributionCurve = distributionCurve
            };
            
            Owner = transform.GetComponent<Entity>();

            if (StartState != null) { ChangeState(StartState); }
 			if (GlobalStartState != null) { ChangeGlobalState(GlobalStartState); }
        }

        public override void Update()
        {
            base.Update();

            if (!stateMachineUpdateRegulator.IsReady) { return; }
            
            // Update the global continuously active state.
            if (GlobalState != null) GlobalState.Execute(this);

            //  Update the current state.
            if (CurrentState != null) CurrentState.Execute(this);
        }
        
        // Sets the global continuously active state.
        public void ChangeGlobalState(State globalState)
        {
            // call the exit method of the continuously active state
            // Generally, we do not change this state, but in case we do ...
            if (GlobalState != null) GlobalState.Exit(this);

            GlobalState = globalState;

            if (GlobalState != null) GlobalState.Enter(this);
        }
        
        // Transition to another state.
        public void ChangeState(State nextState)
        {
            PreviousState = CurrentState;

            // call the exit method of the current state
            if (CurrentState != null) CurrentState.Exit(this);

            // Set the new current state
            CurrentState = nextState;

            // call the entry method of the new state
            if (CurrentState != null) CurrentState.Enter(this);
        }
        
        // Change state back to the previous state.
        public void RevertToPreviousState()
        {
            ChangeState(PreviousState);
        }
        
        // This executes if the state machine receives an event from the owner.
        // Event handling is first delegated to the current state.
        // If the current state does not handle the event then it is forwarded to
        // the global continuously active state. If it is not handed there, we ignore it.
        public bool HandleEvent<TEvent>(Event<TEvent> eventArguments)
        {
            // First see if the current state is valid and that it can handle the event.
            if (CurrentState != null && CurrentState.HandleEvent(this, eventArguments))
            {
                return true;
            }

            // If not, and if a global continuously active state has been implemented,
            // forward the event to it.
            return GlobalState != null
                   && GlobalState.HandleEvent(this, eventArguments);
        }
        
        // Only ever used during debugging to grab the name of the current state.
        public string GetNameOfCurrentState()
        {
            return CurrentState != null ? CurrentState.GetType().Name : string.Empty;
        }
    }
}