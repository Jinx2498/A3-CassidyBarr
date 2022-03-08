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
    public class Flee : LinearSlow
    {
        #region Creators

        public new static Flee CreateInstance(SteeringData steeringData)
        {
            var steeringBehaviour = CreateInstance<Flee>(steeringData);
            Initialize(steeringBehaviour);
            return steeringBehaviour;
        }

        public static Flee CreateInstance(
            SteeringData steeringData,
            VectorXZ targetLocation)
        {
            var steeringBehaviour = CreateInstance<Flee>(steeringData, targetLocation);
            Initialize(steeringBehaviour);
            return steeringBehaviour;
        }

        public static Flee CreateInstance(
            SteeringData steeringData,
            Transform targetTransform)
        {
            var steeringBehaviour = CreateInstance<Flee>(steeringData, targetTransform);
            Initialize(steeringBehaviour);
            return steeringBehaviour;
        }

        public static Flee CreateInstance(
            SteeringData steeringData,
            KinematicData targetKinematicData)
        {
            var steeringBehaviour = CreateInstance<Flee>(steeringData, targetKinematicData);
            Initialize(steeringBehaviour);
            return steeringBehaviour;
        }

        protected static void Initialize(Flee steeringBehaviour)
        {
            LinearSlow.Initialize(steeringBehaviour);
            steeringBehaviour.NeverCompletes = false;
            steeringBehaviour.NoStop = false;
            steeringBehaviour.NoSlow = false;
        }

        #endregion Creators

        #region Members and Properties

        public float EscapeDistance
        {
            get => escapeDistance;
            set => escapeDistance = value;
        }
        [SerializeField] float escapeDistance = 10f;

        public virtual bool FleeActive { get; protected set; } = true;
        bool fleeCompletedEventSent;

        #endregion Members and Properties

        #region Steering

        public override SteeringOutput Steer()
        {
            VectorXZ desiredDirection = TargetLocation - SteeringData.Location;

            float distance = desiredDirection.magnitude;

            FleeActive = (distance > escapeDistance) && !fleeCompletedEventSent;
            
            if (FleeActive) {
                
                VectorXZ desiredVelocity = desiredDirection / distance * SteeringData.MaximumSpeed;

                return new SteeringOutput{
                    Type = SteeringOutput.Types.Velocities,
                    Linear = SteeringData.Velocity - desiredVelocity
                };
            }

            if (!NeverCompletes && !fleeCompletedEventSent) {
                fleeCompletedEventSent = true;
                EventManager.Instance.Enqueue(
                    Events.FleeCompleted,
                    new FleeCompletedEventPayload(
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
        [Description("Flee completed.")]
        public static readonly EventType FleeCompleted = (EventType) Count++;
    }

    public struct FleeCompletedEventPayload
    {
        public int id;
        public SteerableAgent steerableAgent;
        public Flee flee;

        public FleeCompletedEventPayload(
            int id,
            SteerableAgent steerableAgent,
            Flee flee)
        {
            this.id = id;
            this.steerableAgent = steerableAgent;
            this.flee = flee;
        }
    }
}

#endregion Events

