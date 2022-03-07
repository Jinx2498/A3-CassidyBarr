using System.ComponentModel;
using GameBrains.Actuators.Motion.Steering;
using GameBrains.Entities;
using GameBrains.Entities.EntityData;
using GameBrains.EventSystem;
using GameBrains.Extensions.Vectors;
using GameBrains.Motion.Steering.VelocityBased;

namespace GameBrains.Motion.Steering.VelocityBased
{
    [System.Serializable]
    public class Pursue : Seek
    {
        #region Creators

        public new static Pursue CreateInstance(SteeringData steeringData)
        {
            var steeringBehaviour = CreateInstance<Pursue>(steeringData);
            Initialize(steeringBehaviour);
            return steeringBehaviour;
        }

        public new static Pursue CreateInstance(
            SteeringData steeringData,
            KinematicData seekTargetKinematicData)
        {
            var steeringBehaviour = CreateInstance<Pursue>(steeringData, seekTargetKinematicData.Location);
            Initialize(steeringBehaviour);
            InitializeActualKinematicData(steeringBehaviour, seekTargetKinematicData);
            return steeringBehaviour;
        }

        protected static void Initialize(Pursue steeringBehaviour)
        {
            Seek.Initialize(steeringBehaviour);
            steeringBehaviour.NeverCompletes = false;
            steeringBehaviour.NoStop = false;
            steeringBehaviour.NoSlow = false;
        }

        protected static void InitializeActualKinematicData(
            Pursue steeringBehaviour,
            KinematicData seekTargetKinematicData)
        {
            steeringBehaviour.OtherTargetKinematicData = seekTargetKinematicData;
        }

        #endregion Creators

        #region Members and Properties

        public virtual bool PursueActive { get; protected set; } = true;
        bool pursueCompletedEventSent;

        #endregion Members and Properties

        #region Steering

        public override SteeringOutput Steer()
        {
            // TODO for A3: Replace

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
        [Description("Pursue completed.")]
        public static readonly EventType PursueCompleted = (EventType) Count++;
    }

    public struct PursueCompletedEventPayload
    {
        public int id;
        public SteerableAgent steerableAgent;
        public Pursue pursue;

        public PursueCompletedEventPayload(
            int id,
            SteerableAgent steerableAgent,
            Pursue pursue)
        {
            this.id = id;
            this.steerableAgent = steerableAgent;
            this.pursue = pursue;
        }
    }
}

#endregion Events