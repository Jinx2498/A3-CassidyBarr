using System;
using log4net;
using UnityEngine;

namespace GameBrains.Extensions.MonoBehaviours
{
    public abstract class ExtendedMonoBehaviour : MonoBehaviour
    {
        #region VerbosityStates
        
        [Flags] public enum VerbosityStates { None, Debug, Log, }
        
        [Tooltip("verbosity selects whether Debug, Log, etc. states are active.")]
        public VerbosityStates verbosity;
        public bool VerbosityDebug => VerbosityHasAny(VerbosityStates.Debug);
        public bool VerbosityLog => VerbosityHasAny(VerbosityStates.Log);

        public bool VerbosityDebugOrLog =>
            VerbosityHasAny(VerbosityStates.Debug | VerbosityStates.Log);

        public bool VerbosityHasAll(VerbosityStates stateFlags)
        {
            return (verbosity & stateFlags) == stateFlags;
        }

        public bool VerbosityHasAny(VerbosityStates stateFlags)
        {
            return (verbosity & stateFlags) != 0;
        }

        #endregion VerbosityStates

        #region Log
        
        protected ILog Log
        {
            get
            {
                log ??= LogManager.GetLogger(GetType());
                return log;
            }
        }
        ILog log;
        
        #endregion Log

        //TODO-1: Add shortcuts and extensions.

        #region MonoBehaviour Callback Events

        //Note: Explicitly including unused event functions may negatively impact performance.
        //Note: Unity will disable a script if it throws an exception in Awake().
        public virtual void Awake() { }
        public virtual void OnEnable() { }
        public virtual void OnDisable() { }
        public virtual void OnDestroy() { }
        public virtual void Start() { }
        public virtual void Update() { }
        public virtual void FixedUpdate() { }
        public virtual void LateUpdate() { }

        #endregion MonoBehaviour Callback Events
    }
}