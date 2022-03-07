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
    public class Arrive : Seek
    {
        #region Creators

        public new static Arrive CreateInstance(SteeringData steeringData)
        {
            var steeringBehaviour = CreateInstance<Arrive>(steeringData);
            Initialize(steeringBehaviour);
            return steeringBehaviour;
        }

        public new static Arrive CreateInstance(
            SteeringData steeringData,
            VectorXZ targetLocation)
        {
            var steeringBehaviour = CreateInstance<Arrive>(steeringData, targetLocation);
            Initialize(steeringBehaviour);
            return steeringBehaviour;
        }

        public new static Arrive CreateInstance(
            SteeringData steeringData,
            Transform targetTransform)
        {
            var steeringBehaviour = CreateInstance<Arrive>(steeringData, targetTransform);
            Initialize(steeringBehaviour);
            return steeringBehaviour;
        }

        public new static Arrive CreateInstance(
            SteeringData steeringData,
            KinematicData targetKinematicData)
        {
            var steeringBehaviour = CreateInstance<Arrive>(steeringData, targetKinematicData);
            Initialize(steeringBehaviour);
            return steeringBehaviour;
        }

        protected static void Initialize(Arrive steeringBehaviour)
        {
            Seek.Initialize(steeringBehaviour);
            steeringBehaviour.NoSlow = false;
            steeringBehaviour.NoStop = false;
            steeringBehaviour.NeverCompletes = false;
        }

        #endregion Creators

        #region Members and Properties

        public float BrakingDistance
        {
            get => brakingDistance;
            set => brakingDistance = value;
        }
        [SerializeField] float brakingDistance = 5f;
        
        //TODO: Add Braking/Braking Completed
        public virtual bool BrakingStarted
        {
            get => brakingStarted;
            protected set => brakingStarted = value;
        }
        [SerializeField] bool brakingStarted;

        public virtual bool ArriveActive { get; protected set; } = true;
        bool arriveCompletedEventSent;
        
        #endregion Members and Properties

        #region Steering

        public override SteeringOutput Steer()
        {
            // TODO for A3 (optional): Replace

            // no effect
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
        [Description("Arrive completed.")]
        public static readonly EventType ArriveCompleted = (EventType) Count++;

        [Description("Arrive braking started.")]
        public static readonly EventType ArriveBrakingStarted = (EventType) Count++;
    }

    public struct ArriveCompletedEventPayload
    {
        public int id;
        public SteerableAgent steerableAgent;
        public Arrive arrive;

        public ArriveCompletedEventPayload(
            int id,
            SteerableAgent steerableAgent,
            Arrive arrive)
        {
            this.id = id;
            this.steerableAgent = steerableAgent;
            this.arrive = arrive;
        }
    }

    public struct ArriveBrakingStartedEventPayload
    {
        public int id;
        public SteerableAgent steerableAgent;
        public Arrive arrive;

        public ArriveBrakingStartedEventPayload(
            int id,
            SteerableAgent steerableAgent,
            Arrive arrive)
        {
            this.id = id;
            this.steerableAgent = steerableAgent;
            this.arrive = arrive;
        }
    }
}

#endregion Events