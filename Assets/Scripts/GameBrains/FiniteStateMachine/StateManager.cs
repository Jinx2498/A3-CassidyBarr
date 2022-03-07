using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameBrains.FiniteStateMachine
{
    // Used to lookup states by type.
    public static class StateManager
    {
        static readonly Dictionary<Type, State> RegisteredStates;

        static StateManager() { RegisteredStates = new Dictionary<Type, State>(); }

        public static void Register(Type stateType, State stateToAdd)
        {
            RegisteredStates[stateType] = stateToAdd;
        }

        public static void Deregister(Type stateType)
        {
            RegisteredStates.Remove(stateType);
        }

        public static State Lookup(Type stateType)
        {
            return RegisteredStates.ContainsKey(stateType) ? RegisteredStates[stateType] : Create(stateType);
        }

        // If a state is referenced, but not registered, create and register it.
        static State Create(Type stateType)
        {
            ScriptableObject.CreateInstance(stateType);
            return RegisteredStates[stateType];
        }
    }
}