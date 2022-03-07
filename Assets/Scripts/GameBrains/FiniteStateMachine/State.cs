using GameBrains.EventSystem;
using GameBrains.Extensions.ScriptableObjects;
using UnityEngine;

namespace GameBrains.FiniteStateMachine
{
    // Abstract base class to define an interface for a state.
    public abstract class State : ExtendedScriptableObject
    {
        // Register the state so it can be looked up later.
        public override void OnEnable() { base.OnEnable(); StateManager.Register(GetType(), this); }
        
        // Deregister the state so it can't be found by lookup..
        public override void OnDisable() { base.OnDisable(); StateManager.Deregister(GetType()); }
        
        // This will execute when the state is entered.
        // Default is to do nothing unless overridden in a derived class.
        public virtual void Enter(StateMachine stateMachine)
        {
            if (VerbosityDebug)
            {
                Debug.Log($"{name} Enter");
            }
        }
        
        // This is the state's normal update function.
        // Default is to do nothing unless overridden in a derived class.
        public virtual void Execute(StateMachine stateMachine)
        {
            if (VerbosityDebug)
            {
                Debug.Log($"{name} Execute");
            }
        }
        
        // This will execute when the state is exited.
        // Default is to do nothing unless overridden in a derived class.
        public virtual void Exit(StateMachine stateMachine)
        {
            if (VerbosityDebug)
            {
                Debug.Log($"{name} Exit");
            }
        }
        
        // This executes if the state receives an event from the state machine.
        // Default is to ignore events unless overridden in a derived class.
        public virtual bool HandleEvent<TEvent>(
            StateMachine stateMachine,
            Event<TEvent> eventArguments)
        {
            return false; // not handled by any state
        }
    }
}