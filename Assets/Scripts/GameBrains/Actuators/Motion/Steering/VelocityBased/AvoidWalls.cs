using GameBrains.Entities.EntityData;
using GameBrains.Extensions.Vectors;
using GameBrains.Motion.Steering.VelocityBased;
using GameBrains.Visualization;
using UnityEngine;

namespace GameBrains.Actuators.Motion.Steering.VelocityBased
{
    [System.Serializable]
    public class AvoidWalls : Seek
    {
        #region Creators

        public new static AvoidWalls CreateInstance(SteeringData steeringData)
        {
            var steeringBehaviour = CreateInstance<AvoidWalls>(steeringData);
            Initialize(steeringBehaviour);
            return steeringBehaviour;
        }

        protected static void Initialize(AvoidWalls steeringBehaviour)
        {
            Seek.Initialize(steeringBehaviour);
            steeringBehaviour.NeverCompletes = true;
            steeringBehaviour.NoStop = true;
            steeringBehaviour.NoSlow = true;

            steeringBehaviour.RayCastVisualizer = new RayCastVisualizer[3];

            for (int i = 0; i < FeelerCount; i++)
            {
                steeringBehaviour.RayCastVisualizer[i] 
                    = ScriptableObject.CreateInstance<RayCastVisualizer>();
            }
        }

        #endregion Creators

        #region Members and Properties

        public float SteeringMultiplier
        {
            get => steeringMultiplier;
            set => steeringMultiplier = value;
        }
        [SerializeField] float steeringMultiplier = 4f;
        
        public float ForceMultiplier
        {
            get => forceMultiplier;
            set => forceMultiplier = value;
        }
        [SerializeField] float forceMultiplier = 2f;

        public float LookAheadMultiplier
        {
            get => lookAheadMultiplier;
            set => lookAheadMultiplier = value;
        }
        [SerializeField] float lookAheadMultiplier = 2f;

        public const int FeelerCount = 3;

        #region Visualizers
        
        public Transform TargetMarker
        {
            get => targetMarker;
            set => targetMarker = value;
        }
        [SerializeField] Transform targetMarker;
        
        public bool ShowTargetMarker
        {
            get => showTargetMarker;
            set
            {
                showTargetMarker = value;
                targetMarker.gameObject.SetActive(showTargetMarker);
            }
        }
        [SerializeField] bool showTargetMarker;

        public RayCastVisualizer[] RayCastVisualizer { get; set;  }

        public bool ShowVisualizer
        {
            get => showVisualizer;
            set => showVisualizer = value;
        }
        [SerializeField] bool showVisualizer;

        public bool ShowOnlyWhenBlocked
        {
            get => showOnlyWhenBlocked;
            set => showOnlyWhenBlocked = value;
        }
        [SerializeField] bool showOnlyWhenBlocked;

        public Color ClearColor
        {
            get => clearColor;
            set => clearColor = value;
        }
        [SerializeField] Color clearColor = Color.cyan;

        public Color BlockedColor
        {
            get => blockedColor;
            set => blockedColor = value;
        }
        [SerializeField] Color blockedColor = Color.magenta;

        #endregion Visualizers

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