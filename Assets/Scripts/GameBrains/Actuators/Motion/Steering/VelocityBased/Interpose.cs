using GameBrains.Entities.EntityData;
using GameBrains.Extensions.Vectors;
using GameBrains.Motion.Steering.VelocityBased;
using UnityEngine;

namespace GameBrains.Actuators.Motion.Steering.VelocityBased
{
    [System.Serializable]
    public class Interpose : Arrive
    {
        #region Creators

        public new static Interpose CreateInstance(SteeringData steeringData)
        {
            var steeringBehaviour = CreateInstance<Interpose>(steeringData);
            Initialize(steeringBehaviour);
            return steeringBehaviour;
        }

        public static Interpose CreateInstance(
            SteeringData steeringData,
            KinematicData firstKinematicData,
            KinematicData secondKinematicData)
        {
            var steeringBehaviour = CreateInstance<Interpose>(steeringData, firstKinematicData.Location);
            Initialize(steeringBehaviour);
            steeringBehaviour.FirstKinematicData = firstKinematicData;
            steeringBehaviour.SecondKinematicData = secondKinematicData;
            return steeringBehaviour;
        }

        public static Interpose CreateInstance(
            SteeringData steeringData,
            KinematicData firstKinematicData,
            Transform secondTransform)
        {
            var steeringBehaviour = CreateInstance<Interpose>(steeringData, firstKinematicData.Location);
            Initialize(steeringBehaviour);
            steeringBehaviour.FirstKinematicData = firstKinematicData;
            steeringBehaviour.SecondTransform = secondTransform;
            return steeringBehaviour;
        }

        public static Interpose CreateInstance(
            SteeringData steeringData,
            KinematicData firstKinematicData,
            VectorXZ secondLocation)
        {
            var steeringBehaviour = CreateInstance<Interpose>(steeringData, firstKinematicData.Location);
            Initialize(steeringBehaviour);
            steeringBehaviour.FirstKinematicData = firstKinematicData;
            steeringBehaviour.SecondLocation = secondLocation;
            return steeringBehaviour;
        }

        public static Interpose CreateInstance(
            SteeringData steeringData,
            Transform firstTransform,
            Transform secondTransform)
        {
            var steeringBehaviour = CreateInstance<Interpose>(steeringData, (VectorXZ)firstTransform.position);
            Initialize(steeringBehaviour);
            steeringBehaviour.FirstTransform = firstTransform;
            steeringBehaviour.SecondTransform = secondTransform;
            return steeringBehaviour;
        }

        public static Interpose CreateInstance(
            SteeringData steeringData,
            Transform firstTransform,
            VectorXZ secondLocation)
        {
            var steeringBehaviour = CreateInstance<Interpose>(steeringData, (VectorXZ)firstTransform.position);
            Initialize(steeringBehaviour);
            steeringBehaviour.FirstTransform = firstTransform;
            steeringBehaviour.SecondLocation = secondLocation;
            return steeringBehaviour;
        }

        public static Interpose CreateInstance(
            SteeringData steeringData,
            VectorXZ firstLocation,
            VectorXZ secondLocation)
        {
            var steeringBehaviour = CreateInstance<Interpose>(steeringData, firstLocation);
            Initialize(steeringBehaviour);
            steeringBehaviour.FirstLocation = firstLocation;
            steeringBehaviour.SecondLocation = secondLocation;
            return steeringBehaviour;
        }

        protected static void Initialize(Interpose steeringBehaviour)
        {
            Arrive.Initialize(steeringBehaviour);
            steeringBehaviour.NeverCompletes = true;
            steeringBehaviour.NoSlow = false;
            steeringBehaviour.NoStop = false;
        }

        #endregion Creators

        #region Members and Properties

        public KinematicData FirstKinematicData { get; set; }
        public Transform FirstTransform { get; set; }
        
        public VectorXZ FirstLocation
        {
            get => GetFirstLocation();
            set => firstLocation = value;
        }
        VectorXZ firstLocation;
        
        protected VectorXZ GetFirstLocation()
        {
            if (FirstKinematicData != null && FirstKinematicData.OwnerTransform)
            {
                return FirstKinematicData.Location;
            }

            if (FirstTransform != null) return (VectorXZ)FirstTransform.position;

            return firstLocation;
        }

        public KinematicData SecondKinematicData { get; set; }

        public Transform SecondTransform { get; set; }
        
        public VectorXZ SecondLocation
        {
            get => GetSecondLocation();
            set => secondLocation = value;
        }
        VectorXZ secondLocation;

        protected VectorXZ GetSecondLocation()
        {
            if (SecondKinematicData != null && SecondKinematicData.OwnerTransform)
            {
                return SecondKinematicData.Location;
            }

            if (SecondTransform != null) return (VectorXZ)SecondTransform.position;

            return secondLocation;
        }

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