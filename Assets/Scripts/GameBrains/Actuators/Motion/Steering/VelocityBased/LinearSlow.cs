using System.ComponentModel;
using GameBrains.Actuators.Motion.Steering;
using GameBrains.Entities;
using GameBrains.Entities.EntityData;
using GameBrains.EventSystem;
using GameBrains.Extensions.Vectors;
using GameBrains.Motion.Steering.VelocityBased;
using UnityEngine;

namespace GameBrains.Motion.Steering.VelocityBased
{
    [System.Serializable]
    public class LinearSlow : LinearStop
    {
        #region Creators

        public new static LinearSlow CreateInstance(SteeringData steeringData)
        {
            var steeringBehaviour = CreateInstance<LinearSlow>(steeringData);
            Initialize(steeringBehaviour);
            return steeringBehaviour;
        }

        protected static void Initialize(LinearSlow steeringBehaviour)
        {
            LinearStop.Initialize(steeringBehaviour);
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
        
        public float SlowEnoughLinearSpeed { get => slowEnoughLinearSpeed; set => slowEnoughLinearSpeed = value; }
        [SerializeField] float slowEnoughLinearSpeed = 0.5f;

        public float LinearDrag { get => linearDrag; set => linearDrag = value; }
        [SerializeField] float linearDrag = 1.015f;

        public virtual bool LinearSlowActive { get; protected set; } = true;
        bool linearSlowCompletedEventSent;

        #endregion Members and Properties

        #region Steering

        public override SteeringOutput Steer()
        {
            LinearSlowActive 
                = !NoSlow &&
                SteeringData.Speed > SlowEnoughLinearSpeed &&
                  !linearSlowCompletedEventSent;
                
            if (LinearSlowActive)
            {
                VectorXZ desiredVelocity = SteeringData.Velocity / LinearDrag;

                return new SteeringOutput
                {
                    Type = SteeringOutput.Types.Velocities,
                    Linear = desiredVelocity - SteeringData.Velocity
                };
            }

            if (!NeverCompletes && !linearSlowCompletedEventSent)
            {
                linearSlowCompletedEventSent = true;
                EventManager.Instance.Enqueue(
                    Events.LinearSlowCompleted,
                    new LinearSlowCompletedEventPayload(
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
        [Description("Linear Slow completed.")]
        public static readonly EventType LinearSlowCompleted = (EventType) Count++;
    }

    public struct LinearSlowCompletedEventPayload
    {
        public int id;
        public SteerableAgent steerableAgent;
        public LinearSlow linearSlow;

        public LinearSlowCompletedEventPayload(
            int id,
            SteerableAgent steerableAgent,
            LinearSlow linearSlow)
        {
            this.id = id;
            this.steerableAgent = steerableAgent;
            this.linearSlow = linearSlow;
        }
    }
}

#endregion Events