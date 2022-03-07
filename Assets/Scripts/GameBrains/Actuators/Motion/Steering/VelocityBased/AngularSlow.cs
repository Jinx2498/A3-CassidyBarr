using System.ComponentModel;
using GameBrains.Actuators.Motion.Steering;
using GameBrains.Entities;
using GameBrains.Entities.EntityData;
using GameBrains.EventSystem;
using GameBrains.Motion.Steering.VelocityBased;
using UnityEngine;

namespace GameBrains.Motion.Steering.VelocityBased
{
    [System.Serializable]
    public class AngularSlow : AngularStop
    {
        #region Creators

        public new static AngularSlow CreateInstance(SteeringData steeringData)
        {
            var steeringBehaviour = CreateInstance<AngularSlow>(steeringData);
            Initialize(steeringBehaviour);
            return steeringBehaviour;
        }

        protected static void Initialize(AngularSlow steeringBehaviour)
        {
            AngularStop.Initialize(steeringBehaviour);
            steeringBehaviour.NoStop = false;
            steeringBehaviour.NoSlow = false;
            steeringBehaviour.NeverCompletes = false;
        }

        #endregion Creators

        #region Members and Properties
        
        public bool NoSlow
        {
            get => noSlow;
            set => noSlow = value;
        }
        [SerializeField] bool noSlow;

        public float SlowEnoughAngularVelocity
        {
            get => slowEnoughAngularVelocity; 
            set => slowEnoughAngularVelocity = value;
        }
        [SerializeField] float slowEnoughAngularVelocity = 5f;

        public float AngularDrag
        {
            get => angularDrag;
            set => angularDrag = value;
        }
        [SerializeField] float angularDrag = 1.01f;

        public virtual bool AngularSlowActive { get; protected set; } = true;
        bool angularSlowCompletedEventSent;

        #endregion Members and Properties

        #region Steering

        public override SteeringOutput Steer()
        {
            AngularSlowActive
                = !NoSlow &&
                  Mathf.Abs(SteeringData.AngularVelocity) > SlowEnoughAngularVelocity &&
                  !angularSlowCompletedEventSent;
                
            if (AngularSlowActive)
            {
                float desiredAngularVelocity = SteeringData.AngularVelocity / AngularDrag;

                return new SteeringOutput
                {
                    Type = SteeringOutput.Types.Velocities,
                    Angular = desiredAngularVelocity - SteeringData.AngularVelocity
                };
            }

            if (!NeverCompletes && !angularSlowCompletedEventSent)
            {
                angularSlowCompletedEventSent = true;
                EventManager.Instance.Enqueue(
                    Events.AngularSlowCompleted,
                    new AngularSlowCompletedEventPayload(
                        ID,
                        SteeringData.Owner,
                        this));
            }

            return base.Steer();
        }

        #endregion Steering
    }
}

#region Events

// ReSharper disable once CheckNamespace
namespace GameBrains.EventSystem // NOTE: Don't change this namespace
{
    public static partial class Events
    {
        [Description("Angular Slow completed.")]
        public static readonly EventType AngularSlowCompleted = (EventType) Count++;
    }

    public struct AngularSlowCompletedEventPayload
    {
        public int id;
        public SteerableAgent steerableAgent;
        public AngularSlow angularSlow;

        public AngularSlowCompletedEventPayload(
            int id,
            SteerableAgent steerableAgent,
            AngularSlow angularSlow)
        {
            this.id = id;
            this.steerableAgent = steerableAgent;
            this.angularSlow = angularSlow;
        }
    }
}

#endregion Events