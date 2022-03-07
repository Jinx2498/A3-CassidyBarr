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
    public class Evade : Flee
    {
        #region Creators

        public new static Evade CreateInstance(SteeringData steeringData)
        {
            var steeringBehaviour = CreateInstance<Evade>(steeringData);
            Initialize(steeringBehaviour);
            return steeringBehaviour;
        }

        public new static Evade CreateInstance(
            SteeringData steeringData,
            KinematicData fleeTargetKinematicData)
        {
            var steeringBehaviour = CreateInstance<Evade>(steeringData, fleeTargetKinematicData.Location);
            Initialize(steeringBehaviour);
            InitializeActualKinematicData(steeringBehaviour, fleeTargetKinematicData);
            return steeringBehaviour;
        }

        protected static void Initialize(Evade steeringBehaviour)
        {
            Flee.Initialize(steeringBehaviour);
            steeringBehaviour.NeverCompletes = false;
            steeringBehaviour.NoStop = false;
            steeringBehaviour.NoSlow = false;
        }
        
        protected static void InitializeActualKinematicData(
            Evade steeringBehaviour,
            KinematicData fleeTargetKinematicData)
        {
            steeringBehaviour.OtherTargetKinematicData = fleeTargetKinematicData;
        }

        #endregion Creators

        #region Members and Properties

        public virtual bool EvadeActive { get; protected set; } = true;
        bool evadeCompletedEventSent;

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
        [Description("Evade completed.")]
        public static readonly EventType EvadeCompleted = (EventType) Count++;
    }

    public struct EvadeCompletedEventPayload
    {
        public int id;
        public SteerableAgent steerableAgent;
        public Evade evade;

        public EvadeCompletedEventPayload(
            int id,
            SteerableAgent steerableAgent,
            Evade evade)
        {
            this.id = id;
            this.steerableAgent = steerableAgent;
            this.evade = evade;
        }
    }
}

#endregion Events