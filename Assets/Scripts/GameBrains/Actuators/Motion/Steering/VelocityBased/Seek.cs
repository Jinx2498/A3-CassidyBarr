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
    public class Seek : LinearSlow
    {

        #region Creators

        public new static Seek CreateInstance(SteeringData steeringData)
        {
            var steeringBehaviour = CreateInstance<Seek>(steeringData);
            Initialize(steeringBehaviour);
            return steeringBehaviour;
        }

        public static Seek CreateInstance(
            SteeringData steeringData,
            VectorXZ targetLocation)
        {
            var steeringBehaviour = CreateInstance<Seek>(steeringData, targetLocation);
            Initialize(steeringBehaviour);
            return steeringBehaviour;
        }

        public static Seek CreateInstance(
            SteeringData steeringData,
            Transform targetTransform)
        {
            var steeringBehaviour = CreateInstance<Seek>(steeringData, targetTransform);
            Initialize(steeringBehaviour);
            return steeringBehaviour;
        }

        public static Seek CreateInstance(
            SteeringData steeringData,
            KinematicData targetKinematicData)
        {
            var steeringBehaviour = CreateInstance<Seek>(steeringData, targetKinematicData);
            Initialize(steeringBehaviour);
            return steeringBehaviour;
        }

        protected static void Initialize(Seek steeringBehaviour)
        {
            LinearSlow.Initialize(steeringBehaviour);
            steeringBehaviour.NoStop = true;
            steeringBehaviour.NoSlow = true;
            steeringBehaviour.NeverCompletes = true;
        }

        #endregion Creators

        #region Members and Properties
        
        public float CloseEnoughDistance
        {
            get => closeEnoughDistance;
            set => closeEnoughDistance = value;
        }
        [SerializeField] float closeEnoughDistance = 1.5f;

        public virtual bool SeekActive { get; protected set; } = true;
        bool seekCompletedEventSent;

        #endregion Members and Properties

        #region Steering

        public override SteeringOutput Steer()
        {
            VectorXZ desiredDirection = TargetLocation - SteeringData.Location;

            float distance = desiredDirection.magnitude;

            SeekActive = (distance > CloseEnoughDistance) && !seekCompletedEventSent;
            
            if (SeekActive)
            {
                VectorXZ desiredVelocity = desiredDirection / distance * SteeringData.MaximumSpeed;

                return new SteeringOutput
                {
                    Type = SteeringOutput.Types.Velocities,
                    Linear = desiredVelocity - SteeringData.Velocity
                };
            }

            if (!NeverCompletes && !seekCompletedEventSent)
            {
                seekCompletedEventSent = true;
                EventManager.Instance.Enqueue(
                    Events.SeekCompleted,
                    new SeekCompletedEventPayload(
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
        [Description("Seek completed.")]
        public static readonly EventType SeekCompleted = (EventType) Count++;
    }

    public struct SeekCompletedEventPayload
    {
        public int id;
        public SteerableAgent steerableAgent;
        public Seek seek;

        public SeekCompletedEventPayload(
            int id,
            SteerableAgent steerableAgent,
            Seek seek)
        {
            this.id = id;
            this.steerableAgent = steerableAgent;
            this.seek = seek;
        }
    }
}

#endregion Events