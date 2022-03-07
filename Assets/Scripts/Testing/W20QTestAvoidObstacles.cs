using GameBrains.Actuators.Motion.Steering.VelocityBased;
using GameBrains.Entities;
using GameBrains.Extensions.MonoBehaviours;
using GameBrains.Extensions.Vectors;
using GameBrains.Motion.Steering.VelocityBased;
using GameBrains.Visualization;
using UnityEngine;

namespace Testing
{
    public sealed class W20QTestAvoidObstacles : ExtendedMonoBehaviour
    {
        public bool respawn;
        public bool testAvoidObstacles;
        public bool removeAvoidObstaclesFromSteeringBehaviours;
        public bool removeArriveFromSteeringBehaviours;
        public VectorXZ spawnLocation = new VectorXZ(10f, 0f);
        public VectorXZ arriveTarget = new VectorXZ(10f, 15f);
        public SteerableAgent steerableAgent;
        public Transform targetMarker;
        AvoidObstacles avoidObstacles;
        Arrive arrive;
        CapsuleCastVisualizer capsuleCastVisualizer;
        public bool noStop = true;
        public bool noSlow = true;
        public bool neverCompletes = true;
        public float linearStopAtSpeed = 0.1f;
        public float slowEnoughLinearSpeed = 0.5f;
        public float linearDrag = 1.015f;
        public float closeEnoughDistance = 1.5f;
        public float castRadiusMultiplier = 1.5f;
        public bool showTargetMarker;
        public bool showVisualizer = true;
        public bool showOnlyWhenBlocked = false;
        public float lookAheadMultiplier = 4f;
        public float forceMultiplier = 4f;
        public float steeringMultiplier = 4f;

        public override void Awake()
        {
            base.Awake();
            capsuleCastVisualizer = ScriptableObject.CreateInstance<CapsuleCastVisualizer>();
        }

        public override void Update()
        {
            base.Update();
            
            if (steerableAgent == null) { return; }

            if (respawn)
            {
                respawn = false;
                steerableAgent.Spawn((VectorXYZ)spawnLocation);
            }

            if (removeAvoidObstaclesFromSteeringBehaviours)
            {
                removeAvoidObstaclesFromSteeringBehaviours = false;
                RemoveAndDestroyAvoidObstacles();
            }

            if (removeArriveFromSteeringBehaviours)
            {
                removeArriveFromSteeringBehaviours = false;
                RemoveAndDestroyArrive();
            }

            if (testAvoidObstacles)
            {
                testAvoidObstacles = false;

                RemoveAndDestroyAvoidObstacles();
                avoidObstacles = AvoidObstacles.CreateInstance(steerableAgent.SteeringData);
                SetParameters(avoidObstacles);
                steerableAgent.SteeringData.AddSteeringBehaviour(avoidObstacles);
                
                RemoveAndDestroyArrive();
                arrive = Arrive.CreateInstance(steerableAgent.SteeringData, arriveTarget);
                steerableAgent.SteeringData.AddSteeringBehaviour(arrive);
            }
        }
        
        void SetParameters(AvoidObstacles sb)
        {
            sb.NoStop = noStop;
            sb.NoSlow = noSlow;
            sb.NeverCompletes = neverCompletes;
            sb.LinearStopAtSpeed = linearStopAtSpeed;
            sb.SlowEnoughLinearSpeed = slowEnoughLinearSpeed;
            sb.LinearDrag = linearDrag;
            sb.CloseEnoughDistance = closeEnoughDistance;

            sb.TargetMarker = targetMarker;
            sb.CapsuleCastVisualizer = capsuleCastVisualizer;
            sb.CastRadiusMultiplier = castRadiusMultiplier;
            sb.ShowTargetMarker = showTargetMarker;
            sb.ShowVisualizer = showVisualizer;
            sb.ShowOnlyWhenBlocked = showOnlyWhenBlocked;
            sb.LookAheadMultiplier = lookAheadMultiplier;
            sb.ForceMultiplier = forceMultiplier;
            sb.SteeringMultiplier = steeringMultiplier;
        }
        
        void RemoveAndDestroyAvoidObstacles()
        {
            if (avoidObstacles != null)
            {
                steerableAgent.SteeringData.RemoveSteeringBehaviour(avoidObstacles);
                Destroy(avoidObstacles);
                avoidObstacles = null;
            }

            RemoveAndDestroyArrive();
        }
        
        void RemoveAndDestroyArrive()
        {
            if (arrive != null)
            {
                steerableAgent.SteeringData.RemoveSteeringBehaviour(arrive);
                Destroy(arrive);
                arrive = null;
            }
        }
    }
}