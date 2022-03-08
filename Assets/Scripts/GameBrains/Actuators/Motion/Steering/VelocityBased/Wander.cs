using System.ComponentModel;
using GameBrains.Actuators.Motion.Steering;
using GameBrains.Actuators.Motion.Steering.VelocityBased;
using GameBrains.Entities;
using GameBrains.Entities.EntityData;
using GameBrains.EventSystem;
using GameBrains.Extensions.MathExtensions;
using GameBrains.Extensions.Vectors;
using GameBrains.Motion.Steering.VelocityBased;
using UnityEngine;

namespace GameBrains.Motion.Steering.VelocityBased
{
    [System.Serializable]
    public class Wander : SteeringBehaviour
    {
        #region Creators

        public static Wander CreateInstance(SteeringData steeringData)
        {
            var move = Arrive.CreateInstance(steeringData);
            var look = FaceHeading.CreateInstance(steeringData);
            var steeringBehaviour = CreateInstance(steeringData, move, look);
            return steeringBehaviour;
        }

        public static Wander CreateInstance(
            SteeringData steeringData,
            LinearStop move,
            AngularStop look)
        {
            var steeringBehaviour = CreateInstance<Wander>(steeringData);
            Initialize(steeringBehaviour);
            InitializeMove(steeringBehaviour, move);
            InitializeLook(steeringBehaviour, look);
            return steeringBehaviour;
        }

        protected static void Initialize(Wander steeringBehaviour)
        {
            steeringBehaviour.NeverCompletes = true;
        }

        protected static void InitializeMove(Wander steeringBehaviour, LinearStop move)
        {
            steeringBehaviour.Move = move;
            if (move != null)
            {
                move.NoStop = false;
                move.NeverCompletes = true;

                var linearSlow = move as LinearSlow;

                if (linearSlow != null)
                {
                    linearSlow.NoSlow = false;
                }
            }
        }

        protected static void InitializeLook(Wander steeringBehaviour, AngularStop look)
        {
            steeringBehaviour.Look = look;
            if (look != null)
            {
                look.NoStop = false;
                look.NeverCompletes = true;

                var angularSlow = look as AngularSlow;

                if (angularSlow != null)
                {
                    angularSlow.NoSlow = false;
                }
            }
        }

        #endregion Creators

        #region Members and Properties

        public LinearStop Move { get; set;  }

        public AngularStop Look { get; set; }

        public float WanderCircleRate
        {
            get => wanderCircleRate;
            set => wanderCircleRate = value;
        }
        [SerializeField] float wanderCircleRate = 1f;

        public float WanderCircleRadius
        {
            get => wanderCircleRadius;
            set => wanderCircleRadius = value;
        }
        [SerializeField] float wanderCircleRadius = 10f;

        public float WanderCircleOffset
        {
            get => wanderCircleOffset;
            set => wanderCircleOffset = value;
        }
        [SerializeField] float wanderCircleOffset = 30f;

        public float MaximumSlideDegrees
        {
            get => maximumSlideDegrees;
            set => maximumSlideDegrees = value;
        }
        [SerializeField] float maximumSlideDegrees = 5f;

        // Optional position to stop at (useful if wander combined with seek or arrive)
        public VectorXZ? WanderStopLocation
        {
            get => wanderStopLocation;
            set => wanderStopLocation = value;
        }
        [SerializeField] VectorXZ? wanderStopLocation;

        public float WanderCloseEnoughDistance
        {
            get => wanderCloseEnoughDistance;
            set => wanderCloseEnoughDistance = value;
        }
        [SerializeField] float wanderCloseEnoughDistance = 1f;

        private float wanderOrientation = 0f;

        private float RandomBinomial() {
            return Random.value - Random.value;
        }

        private VectorXZ OrientationToVector(float orientation) {
            return new VectorXZ(Mathf.Cos(orientation), Mathf.Sin(orientation));
        }

        public virtual bool WanderActive { get; protected set; } = true;
        float wanderAngle;
        bool wanderCompletedEventSent;
        
        #endregion Members and Properties
        public Transform OwnerTransform;      
        
        #region Steering

        public override SteeringOutput Steer()
        {
            VectorXZ desiredDirection = TargetLocation - SteeringData.Location;

            float distance = desiredDirection.magnitude;

            wanderOrientation += RandomBinomial() * wanderCircleRate;

            float characterOrientation = OwnerTransform.rotation.eulerAngles.y * Mathf.Deg2Rad;
            float targetOrientation = wanderOrientation + characterOrientation;
        
            VectorXZ targetPos = SteeringData.Location + (wanderCircleOffset * OrientationToVector(characterOrientation));

            targetPos += wanderCircleRadius * OrientationToVector(targetOrientation);

            WanderActive = (distance > wanderCloseEnoughDistance) && !wanderCompletedEventSent;
            
            if (WanderActive)
            {
                return new SteeringOutput
                {
                    Type = SteeringOutput.Types.Velocities,
                    Linear = targetPos - SteeringData.Location 
                };
            }

            if (!NeverCompletes && !wanderCompletedEventSent)
            {
                wanderCompletedEventSent = true;
                EventManager.Instance.Enqueue(
                    Events.WanderCompleted,
                    new WanderCompletedEventPayload(
                        ID,
                        SteeringData.Owner,
                        this));
            }

            return Steer();
        }

        #endregion Steering
    }
}

#region Events

// ReSharper disable once CheckNamespace
namespace GameBrains.EventSystem // NOTE: Don't change this namespace
{
    // This event only occurs if WanderStopLocation has a value.
    public static partial class Events
    {
        [Description("Wander completed.")]
        public static readonly EventType WanderCompleted = (EventType) Count++;
    }

    public struct WanderCompletedEventPayload
    {
        public int id;
        public SteerableAgent steerableAgent;
        public Wander wander;

        public WanderCompletedEventPayload(
            int id,
            SteerableAgent steerableAgent,
            Wander wander)
        {
            this.id = id;
            this.steerableAgent = steerableAgent;
            this.wander = wander;
        }
    }
}

#endregion Events