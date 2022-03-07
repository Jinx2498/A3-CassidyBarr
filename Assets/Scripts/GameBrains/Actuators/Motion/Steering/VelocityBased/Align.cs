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
    public class Align : AngularSlow
    {
        #region Creators

        public new static Align CreateInstance(SteeringData steeringData)
        {
            var steeringBehaviour = CreateInstance<Align>(steeringData);
            Initialize(steeringBehaviour);
            return steeringBehaviour;
        }

        public static Align CreateInstance(
            SteeringData steeringData,
            float targetOrientation)
        {
            var steeringBehaviour = CreateInstance<Align>(steeringData, targetOrientation);
            Initialize(steeringBehaviour);
            return steeringBehaviour;
        }

        public static Align CreateInstance(
            SteeringData steeringData,
            Transform targetTransform)
        {
            var steeringBehaviour = CreateInstance<Align>(steeringData, targetTransform);
            Initialize(steeringBehaviour);
            return steeringBehaviour;
        }

        public static Align CreateInstance(
            SteeringData steeringData,
            KinematicData targetKinematicData)
        {
            var steeringBehaviour = CreateInstance<Align>(steeringData, targetKinematicData);
            Initialize(steeringBehaviour);
            return steeringBehaviour;
        }

        protected static void Initialize(Align steeringBehaviour)
        {
            AngularSlow.Initialize(steeringBehaviour);
            steeringBehaviour.NoStop = false;
            steeringBehaviour.NoSlow = false;
            steeringBehaviour.NeverCompletes = false;
        }

        #endregion Creators

        #region Members and Properties

        public float CloseEnoughAngle
        {
            get => closeEnoughAngle;
            set => closeEnoughAngle = value;
        }
        [SerializeField] float closeEnoughAngle = 5f;

        public virtual bool AlignActive { get; protected set; } = true;
        bool alignCompletedEventSent;

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
        [Description("Align completed.")]
        public static readonly EventType AlignCompleted = (EventType) Count++;
    }

    public struct AlignCompletedEventPayload
    {
        public int id;
        public SteerableAgent steerableAgent;
        public Align align;

        public AlignCompletedEventPayload(
            int id,
            SteerableAgent steerableAgent,
            Align align)
        {
            this.id = id;
            this.steerableAgent = steerableAgent;
            this.align = align;
        }
    }
}

#endregion Events