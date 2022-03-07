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
    public class AngularStop : SteeringBehaviour
    {
        #region Creators

        public static AngularStop CreateInstance(SteeringData steeringData)
        {
            var steeringBehaviour = CreateInstance<AngularStop>(steeringData);
            Initialize(steeringBehaviour);
            return steeringBehaviour;
        }

        protected static void Initialize(AngularStop steeringBehaviour)
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

        public float AngularStopAtSpeed { get => angularStopAtSpeed; set => angularStopAtSpeed = value; }
        [SerializeField] float angularStopAtSpeed = 0.1f;
        
        public virtual bool AngularStopActive { get; protected set; } = true;
        bool angularStopCompletedEventSent;

        #endregion Members and Properties

        #region Steering

        public override SteeringOutput Steer()
        {
            AngularStopActive = 
                !NoStop &&
                Mathf.Abs(SteeringData.AngularVelocity) > AngularStopAtSpeed &&
                !angularStopCompletedEventSent;

            if (AngularStopActive)
            {
                float desiredAngularVelocity = 0;

                // Hard stop to avoid numerical inaccuracies.
                SteeringData.AngularVelocity = desiredAngularVelocity;

                return new SteeringOutput
                {
                    Type = SteeringOutput.Types.Velocities,
                    Angular = desiredAngularVelocity - SteeringData.AngularVelocity
                };
            }

            if (!NeverCompletes && !angularStopCompletedEventSent)
            {
                angularStopCompletedEventSent = true;
                EventManager.Instance.Enqueue(
                    Events.AngularStopCompleted,
                    new AngularStopCompletedEventPayload(
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
        [Description("Angular Stop completed.")]
        public static readonly EventType AngularStopCompleted = (EventType) Count++;
    }

    public struct AngularStopCompletedEventPayload
    {
        public int id;
        public SteerableAgent steerableAgent;
        public AngularStop angularStop;

        public AngularStopCompletedEventPayload(
            int id,
            SteerableAgent steerableAgent,
            AngularStop angularStop)
        {
            this.id = id;
            this.steerableAgent = steerableAgent;
            this.angularStop = angularStop;
        }
    }
}

#endregion Events