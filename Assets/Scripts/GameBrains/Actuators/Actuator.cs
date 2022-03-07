using System.Collections.Generic;
using GameBrains.Actions;
using GameBrains.Entities;
using GameBrains.Extensions.MonoBehaviours;
using GameBrains.Timers;
using UnityEngine;

namespace GameBrains.Actuators
{
    public abstract class Actuator : ExtendedMonoBehaviour
    {
        #region Agent
        
        [SerializeField] protected Agent agent;
        protected virtual Agent Agent => agent;
        
        #endregion Agent

        #region Regulator

        [SerializeField] protected float minimumTimeMs;
        [SerializeField] protected float maximumDelayMs;
        [SerializeField] protected RegulatorMode mode;
        [SerializeField] protected DelayDistribution regulatorDistribution;
        [SerializeField] protected AnimationCurve distributionCurve 
            = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
		
        protected Regulator actuatorUpdateRegulator;
        public Regulator ActuatorUpdateRegulator => actuatorUpdateRegulator;
        
        #endregion Regulator

        public override void Awake()
        {
            base.Awake();
            
            // The Agent component should either be attached to the same
            // gameObject as the Actuator component or above it in the hierarchy.
            // This checks the gameObject first and then works its way upward.
            if (agent == null) { agent = GetComponentInParent<Agent>(); }
        }

        public override void Start()
        {
            base.Start();
            
            actuatorUpdateRegulator ??= new Regulator
            {
                MinimumTimeMs = minimumTimeMs,
                MaximumDelayMs = maximumDelayMs,
                Mode = mode,
                DelayDistribution = regulatorDistribution,
                DistributionCurve = distributionCurve
            };
        }
        
        public virtual void Act(List<Action> actions)
        {
            if (ActuatorUpdateRegulator.IsReady)
            {
                foreach (Action action in actions)
                {
                    Act(action);
                }
            }
        }

        protected virtual void Act(Action action) { }
        
        #region Capsule Collider
        
        // TODO: Where should the capsule collider (if any) be set up?
        protected virtual void SetupCapsuleCollider()
        {
            var capsuleCollider = Agent.CapsuleCollider;
            if (capsuleCollider != null) { return; }

            capsuleCollider = Agent.gameObject.AddComponent<CapsuleCollider>();
            Vector3 center = capsuleCollider.center;
            center.y = 1.08f; // Agent's pivot is at 0, not its center
            capsuleCollider.center = center;
            capsuleCollider.height = 2;
            Agent.CapsuleCollider = capsuleCollider;
        }

        #endregion

        #region Character Controller
        
        // Should be called by subclasses that use a Character Controller
        protected virtual void SetupCharacterController()
        {
            var characterController = Agent.CharacterController;
            if (characterController != null) { return; }
            
            characterController = Agent.gameObject.AddComponent<CharacterController>();
   
            Vector3 center = characterController.center;
            center.y = 1.08f; // Agent's pivot is at 0, not its center
            characterController.center = center;
            Agent.CharacterController = characterController;
        }
        
        #endregion Character Controller

        #region Rigidbody

        protected virtual void SetupRigidbody()
        {
            var rb = Agent.Rigidbody;
            if (rb != null) { return; }
            
            rb = Agent.gameObject.AddComponent<Rigidbody>();
            
            //rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

            Agent.Rigidbody = rb;
        }
        
        #endregion Rigidbody
    }
}