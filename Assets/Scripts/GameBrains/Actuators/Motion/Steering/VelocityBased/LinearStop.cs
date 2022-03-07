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
    // Stop essentially keeps the velocity at zero, but does not change the acceleration.
    public class LinearStop : SteeringBehaviour
    {
        #region Creators

        public static LinearStop CreateInstance(SteeringData steeringData)
        {
            var steeringBehaviour = CreateInstance<LinearStop>(steeringData);
            Initialize(steeringBehaviour);
            return steeringBehaviour;
        }

        protected static void Initialize(LinearStop steeringBehaviour)
        {
            steeringBehaviour.NoStop = false;
            steeringBehaviour.NeverCompletes = false;
        }

        #endregion Creators

        #region Members and Properties
        
        public bool NoStop
        {
            get => noStop;
            set => noStop = value;
        }
        [SerializeField] bool noStop;

        public float LinearStopAtSpeed { get => linearStopAtSpeed; set => linearStopAtSpeed = value; }
        [SerializeField] float linearStopAtSpeed = 0.1f;
        
        public virtual bool LinearStopActive { get; protected set; } = true;
        bool linearStopCompletedEventSent;

        #endregion Members and Properties

        #region Steering

        public override SteeringOutput Steer()
        {
            LinearStopActive
                = !NoStop &&
                SteeringData.Speed > LinearStopAtSpeed &&
                  !linearStopCompletedEventSent;

            if (LinearStopActive)
            {
                VectorXZ desiredVelocity = VectorXZ.zero;
            
                // Hard stop to avoid numerical inaccuracies.
                SteeringData.Velocity = desiredVelocity;

                return new SteeringOutput
                {
                    Type = SteeringOutput.Types.Velocities,
                    Linear = desiredVelocity - SteeringData.Velocity
                };
            }

            if (!NeverCompletes && !linearStopCompletedEventSent)
            {
                linearStopCompletedEventSent = true;
                EventManager.Instance.Enqueue(
                    Events.LinearStopCompleted,
                    new LinearStopCompletedEventPayload(
                        ID,
                        SteeringData.Owner,
                        this));
            }
            
            // No effect
            return new SteeringOutput { Type = SteeringOutput.Types.Velocities };
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
        [Description("Linear Stop completed.")]
        public static readonly EventType LinearStopCompleted = (EventType) Count++;
    }

    public struct LinearStopCompletedEventPayload
    {
        public int id;
        public SteerableAgent steerableAgent;
        public LinearStop linearStop;

        public LinearStopCompletedEventPayload(
            int id,
            SteerableAgent steerableAgent,
            LinearStop linearStop)
        {
            this.id = id;
            this.steerableAgent = steerableAgent;
            this.linearStop = linearStop;
        }
    }
}

#endregion Events