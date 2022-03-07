using GameBrains.Entities.EntityData;
using GameBrains.Extensions.ScriptableObjects;
using GameBrains.Extensions.Vectors;
using UnityEngine;

namespace GameBrains.Actuators.Motion.Steering
{
    [System.Serializable]
    public abstract class SteeringBehaviour : ExtendedScriptableObject
    {
        #region Creators

        public static T CreateInstance<T>(SteeringData steeringData)
            where T : SteeringBehaviour
        {
            var steeringBehaviour = CreateInstance<T>();
            Initialize(steeringData, steeringBehaviour);
            return steeringBehaviour;
        }

        public static T CreateInstance<T>(SteeringData steeringData, VectorXZ targetLocation)
            where T : SteeringBehaviour
        {
            var steeringBehaviour = CreateInstance<T>(steeringData);
            InitializeLocation(steeringBehaviour, targetLocation);
            return steeringBehaviour;
        }

        public static T CreateInstance<T>(SteeringData steeringData, float targetOrientation)
            where T : SteeringBehaviour
        {
            var steeringBehaviour = CreateInstance<T>(steeringData);
            InitializeOrientation(steeringBehaviour, targetOrientation);
            return steeringBehaviour;
        }

        public static T CreateInstance<T>(SteeringData steeringData, Transform targetTransform)
            where T : SteeringBehaviour
        {
            var steeringBehaviour = CreateInstance<T>(steeringData);
            InitializeTransform(steeringBehaviour, targetTransform);
            return steeringBehaviour;
        }

        public static T CreateInstance<T>(SteeringData steeringData,
            KinematicData targetKinematicData)
            where T : SteeringBehaviour
        {
            var steeringBehaviour = CreateInstance<T>(steeringData);
            InitializeKinematicData(steeringBehaviour, targetKinematicData);
            return steeringBehaviour;
        }
        protected static void Initialize(
            SteeringData steeringData,
            SteeringBehaviour steeringBehaviour)
        {
            steeringBehaviour.SteeringData = steeringData;
        }

        protected static void InitializeLocation(
            SteeringBehaviour steeringBehaviour,
            VectorXZ targetLocation)
        {
            steeringBehaviour.TargetLocation = targetLocation;
        }

        protected static void InitializeOrientation(
            SteeringBehaviour steeringBehaviour,
            float targetOrientation)
        {
            steeringBehaviour.TargetOrientation = targetOrientation;
        }

        protected static void InitializeTransform(
            SteeringBehaviour steeringBehaviour,
            Transform targetTransform)
        {
            steeringBehaviour.TargetTransform = targetTransform;
        }

        protected static void InitializeKinematicData(
            SteeringBehaviour steeringBehaviour,
            KinematicData targetKinematicData)
        {
            steeringBehaviour.TargetKinematicData = targetKinematicData;
        }

        #endregion Creators

        #region Members and Properties

        public SteeringData SteeringData { get; set; }

        #region Target
        public VectorXZ TargetLocation
        {
            get => GetTargetLocation();
            set => targetLocation = value;
        }

        public float TargetOrientation
        {
            get => GetTargetOrientation();
            set => targetOrientation = value;
        }

        public Transform TargetTransform { get; set; }
        public KinematicData TargetKinematicData { get; set; }

        protected VectorXZ GetTargetLocation()
        {
            if (TargetKinematicData && TargetKinematicData.OwnerTransform)
            {
                return TargetKinematicData.Location;
            }

            if (TargetTransform)
            {
                return (VectorXZ)TargetTransform.position;
            }

            return targetLocation;
        }

        protected float GetTargetOrientation()
        {
            if (TargetKinematicData && TargetKinematicData.OwnerTransform)
            {
                return TargetKinematicData.Orientation;
            }

            if (TargetTransform)
            {
                return TargetTransform.eulerAngles.y;
            }

            return targetOrientation;
        }

        #endregion Target

        #region Other Target

        public VectorXZ OtherTargetLocation
        {
            get => GetOtherTargetLocation();
            set => otherTargetLocation = value;
        }

        public float OtherTargetOrientation
        {
            get => GetOtherTargetOrientation();
            set => otherTargetOrientation = value;
        }

        public Transform OtherTargetTransform { get; set; }
        public KinematicData OtherTargetKinematicData { get; set; }

        protected VectorXZ GetOtherTargetLocation()
        {
            if (OtherTargetKinematicData && OtherTargetKinematicData.OwnerTransform)
            {
                return OtherTargetKinematicData.Location;
            }

            if (OtherTargetTransform)
            {
                return (VectorXZ)OtherTargetTransform.position;
            }

            return otherTargetLocation;
        }

        protected float GetOtherTargetOrientation()
        {
            if (OtherTargetKinematicData && OtherTargetKinematicData.OwnerTransform)
            {
                return OtherTargetKinematicData.Orientation;
            }

            if (OtherTargetTransform)
            {
                return OtherTargetTransform.eulerAngles.y;
            }

            return otherTargetOrientation;
        }

        #endregion Other Target

        static int nextId;
        static int NextID => nextId++;

        public int ID => id;
        //[ReadOnly]
        [SerializeField] int id = NextID;

        // Used by Steering Behaviour Property Drawers
        [HideInInspector] [SerializeField] protected bool showInfo;

        VectorXZ targetLocation;
        float targetOrientation;

        VectorXZ otherTargetLocation;
        float otherTargetOrientation;
        
        public virtual bool NeverCompletes
        {
            get => neverCompletes;
            set => neverCompletes = value;
        }
        [SerializeField] bool neverCompletes;

        #endregion Members and Properties

        #region Steering

        public abstract SteeringOutput Steer();

        #endregion Steering
    }
}