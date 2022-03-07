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
    public class Depart : Flee
    {
        #region Creators

        public new static Depart CreateInstance(SteeringData steeringData)
        {
            var steeringBehaviour = CreateInstance<Depart>(steeringData);
            Initialize(steeringBehaviour);
            return steeringBehaviour;
        }

        public new static Depart CreateInstance(
            SteeringData steeringData,
            VectorXZ targetLocation)
        {
            var steeringBehaviour = CreateInstance<Depart>(steeringData, targetLocation);
            Initialize(steeringBehaviour);
            return steeringBehaviour;
        }

        public new static Depart CreateInstance(
            SteeringData steeringData,
            Transform targetTransform)
        {
            var steeringBehaviour = CreateInstance<Depart>(steeringData, targetTransform);
            Initialize(steeringBehaviour);
            return steeringBehaviour;
        }

        public new static Depart CreateInstance(
            SteeringData steeringData,
            KinematicData targetKinematicData)
        {
            var steeringBehaviour = CreateInstance<Depart>(steeringData, targetKinematicData);
            Initialize(steeringBehaviour);
            return steeringBehaviour;
        }

        protected static void Initialize(Depart steeringBehaviour)
        {
            Flee.Initialize(steeringBehaviour);
            steeringBehaviour.NeverCompletes = false;
            steeringBehaviour.NoStop = false;
            steeringBehaviour.NoSlow = false;
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

        
        public virtual bool DepartActive { get; protected set; } = true;
        bool departCompletedEventSent;

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
        [Description("Depart completed.")]
        public static readonly EventType DepartCompleted = (EventType) Count++;
        
        [Description("Depart braking started.")]
        public static readonly EventType DepartBrakingStarted = (EventType) Count++;
    }

    public struct DepartCompletedEventPayload
    {
        public int id;
        public SteerableAgent steerableAgent;
        public Depart depart;

        public DepartCompletedEventPayload(
            int id,
            SteerableAgent steerableAgent,
            Depart depart)
        {
            this.id = id;
            this.steerableAgent = steerableAgent;
            this.depart = depart;
        }
    }
    
    public struct DepartBrakingStartedEventPayload
    {
        public int id;
        public SteerableAgent steerableAgent;
        public Depart depart;

        public DepartBrakingStartedEventPayload(
            int id,
            SteerableAgent steerableAgent,
            Depart depart)
        {
            this.id = id;
            this.steerableAgent = steerableAgent;
            this.depart = depart;
        }
    }
}

#endregion Events