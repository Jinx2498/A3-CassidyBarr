using GameBrains.Entities.EntityData;
using GameBrains.Extensions.Vectors;
using GameBrains.Motion.Steering.VelocityBased;
using GameBrains.Utilities;
using UnityEngine;

namespace GameBrains.Actuators.Motion.Steering.VelocityBased
{
    [System.Serializable]
    public class Hide : Arrive
    {
        #region Creators

        public new static Hide CreateInstance(SteeringData steeringData)
        {
            var steeringBehaviour = CreateInstance<Hide>(steeringData);
            Initialize(steeringBehaviour);
            return steeringBehaviour;
        }

        public new static Hide CreateInstance(
            SteeringData steeringData,
            VectorXZ targetLocation)
        {
            var steeringBehaviour = CreateInstance<Hide>(steeringData, steeringData.Location);
            Initialize(steeringBehaviour);
            steeringBehaviour.OtherTargetLocation = targetLocation;
            return steeringBehaviour;
        }

        public new static Hide CreateInstance(
            SteeringData steeringData,
            Transform targetTransform)
        {
            var steeringBehaviour = CreateInstance<Hide>(steeringData,steeringData.Location);
            Initialize(steeringBehaviour);
            steeringBehaviour.OtherTargetTransform = targetTransform;
            return steeringBehaviour;
        }

        public new static Hide CreateInstance(
            SteeringData steeringData,
            KinematicData targetKinematicData)
        {
            var steeringBehaviour = CreateInstance<Hide>(steeringData, steeringData.Location);
            Initialize(steeringBehaviour);
            steeringBehaviour.OtherTargetKinematicData = targetKinematicData;
            return steeringBehaviour;
        }

        protected static void Initialize(Hide steeringBehaviour)
        {
            Arrive.Initialize(steeringBehaviour);
            steeringBehaviour.NeverCompletes = true;
            steeringBehaviour.NoSlow = false;
            steeringBehaviour.NoStop = false;
            steeringBehaviour.CreateHidingSpotLocator();
        }

        public void CreateHidingSpotLocator()
        {
            HidingSpotsLocator =
                new HidingSpotsLocator(SteeringData.Owner)
                {
                    Offset = locatorOffset,
                    SearchRadius = locatorSearchRadius,
                    MaxColliders = locatorMaxColliders
                };
        }

        #endregion Creators

        #region Members and Properties

        protected HidingSpotsLocator HidingSpotsLocator { get; set; }

        public float LocatorOffset
        {
            get => locatorOffset;
            set => locatorOffset = value;
        }

        [SerializeField] float locatorOffset = 4f;

        public float LocatorSearchRadius
        {
            get => locatorSearchRadius;
            set => locatorSearchRadius = value;
        }
        [SerializeField] float locatorSearchRadius = 10f;

        public int LocatorMaxColliders
        {
            get => locatorMaxColliders;
            set => locatorMaxColliders = value;
        }
        [SerializeField] int locatorMaxColliders = 10;

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