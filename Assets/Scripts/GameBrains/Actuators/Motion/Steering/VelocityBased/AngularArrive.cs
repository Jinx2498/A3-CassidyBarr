using System.ComponentModel;
using GameBrains.Actuators.Motion.Steering;
using GameBrains.Entities;
using GameBrains.Entities.EntityData;
using GameBrains.EventSystem;
using GameBrains.Extensions.MathExtensions;
using GameBrains.Motion.Steering.VelocityBased;
using UnityEngine;

namespace GameBrains.Motion.Steering.VelocityBased
{
    [System.Serializable]
    public class AngularArrive : Align
    {
        #region Creators

        public new static AngularArrive CreateInstance(SteeringData steeringData)
        {
            var steeringBehaviour = CreateInstance<AngularArrive>(steeringData);
            Initialize(steeringBehaviour);
            return steeringBehaviour;
        }

        public new static AngularArrive CreateInstance(
            SteeringData steeringData,
            float targetOrientation)
        {
            var steeringBehaviour = CreateInstance<AngularArrive>(steeringData, targetOrientation);
            Initialize(steeringBehaviour);
            return steeringBehaviour;
        }

        public new static AngularArrive CreateInstance(
            SteeringData steeringData,
            Transform targetTransform)
        {
            var steeringBehaviour = CreateInstance<AngularArrive>(steeringData, targetTransform);
            Initialize(steeringBehaviour);
            return steeringBehaviour;
        }

        public new static AngularArrive CreateInstance(
            SteeringData steeringData,
            KinematicData targetKinematicData)
        {
            var steeringBehaviour = CreateInstance<AngularArrive>(steeringData, targetKinematicData);
            Initialize(steeringBehaviour);
            return steeringBehaviour;
        }

        protected static void Initialize(AngularArrive steeringBehaviour)
        {
            Align.Initialize(steeringBehaviour);
            steeringBehaviour.NoStop = false;
            steeringBehaviour.NoSlow = false;
            steeringBehaviour.NeverCompletes = false;
        }

        #endregion Creators

        #region Members and Properties

        public float BrakingAngle
        {
            get => brakingAngle;
            set => brakingAngle = value;
        }
        [SerializeField] float brakingAngle = 10f;

        //TODO: Add Braking/Braking Completed
        public virtual bool BrakingStarted
        {
            get => brakingStarted;
            protected set => brakingStarted = value;
        }
        [SerializeField] bool brakingStarted;

        public virtual bool AngularArriveActive { get; protected set; } = true;
        bool angularArriveCompletedEventSent;

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
        [Description("Angular Arrive completed.")]
        public static readonly EventType AngularArriveCompleted = (EventType) Count++;

        [Description("Angular Arrive braking started.")]
        public static readonly EventType AngularArriveBrakingStarted = (EventType) Count++;
    }

    public struct AngularArriveCompletedEventPayload
    {
        public int id;
        public SteerableAgent steerableAgent;
        public AngularArrive angularArrive;

        public AngularArriveCompletedEventPayload(
            int id,
            SteerableAgent steerableAgent,
            AngularArrive angularArrive)
        {
            this.id = id;
            this.steerableAgent = steerableAgent;
            this.angularArrive = angularArrive;
        }
    }

    public struct AngularArriveBrakingStartedEventPayload
    {
        public int id;
        public SteerableAgent steerableAgent;
        public AngularArrive angularArrive;

        public AngularArriveBrakingStartedEventPayload(
            int id,
            SteerableAgent steerableAgent,
            AngularArrive angularArrive)
        {
            this.id = id;
            this.steerableAgent = steerableAgent;
            this.angularArrive = angularArrive;
        }
    }
}

#endregion Events