using GameBrains.Actuators.Motion.Steering.VelocityBased;
using GameBrains.Entities;
using GameBrains.Extensions.MonoBehaviours;
using GameBrains.Extensions.Vectors;
using GameBrains.Motion.Steering.VelocityBased;
using UnityEngine;

namespace Testing
{
    public sealed class W20RTestAvoidWalls : ExtendedMonoBehaviour
    {
        public bool respawn;
        public bool testAvoidWalls;
        public bool removeAvoidWallsFromSteeringBehaviours;
        public bool removeArriveFromSteeringBehaviours;
        public VectorXZ spawnLocation = new VectorXZ(10f, 0f);
        public VectorXZ arriveTarget = new VectorXZ(10f, 15f);
        public SteerableAgent steerableAgent;
        public Transform targetMarker;
        AvoidWalls avoidWalls;
        Arrive arrive;
        public bool noStop = true;
        public bool noSlow = true;
        public bool neverCompletes = true;
        public float linearStopAtSpeed = 0.1f;
        public float slowEnoughLinearSpeed = 0.5f;
        public float linearDrag = 1.015f;
        public float closeEnoughDistance = 0.1f;
        public bool showTargetMarker;
        public bool showVisualizer = true;
        public bool showOnlyWhenBlocked = false;
        public float lookAheadMultiplier = 2f;
        public float forceMultiplier = 2f;
        public float steeringMultiplier = 4f;

        public override void Update()
        {
            base.Update();
            
            if (steerableAgent == null) { return; }

            if (respawn)
            {
                respawn = false;
                steerableAgent.Spawn((VectorXYZ)spawnLocation);
            }

            if (removeAvoidWallsFromSteeringBehaviours)
            {
                removeAvoidWallsFromSteeringBehaviours = false;
                RemoveAndDestroyAvoidWalls();
            }

            if (removeArriveFromSteeringBehaviours)
            {
                removeArriveFromSteeringBehaviours = false;
                RemoveAndDestroyArrive();
            }

            if (testAvoidWalls)
            {
                testAvoidWalls = false;

                RemoveAndDestroyAvoidWalls();
                avoidWalls = AvoidWalls.CreateInstance(steerableAgent.SteeringData);
                SetParameters(avoidWalls);
                steerableAgent.SteeringData.AddSteeringBehaviour(avoidWalls);

                RemoveAndDestroyArrive();
                arrive = Arrive.CreateInstance(steerableAgent.SteeringData, arriveTarget);
                steerableAgent.SteeringData.AddSteeringBehaviour(arrive);
            }
        }
        
        void SetParameters(AvoidWalls sb)
        {
            sb.NoStop = noStop;
            sb.NoSlow = noSlow;
            sb.NeverCompletes = neverCompletes;
            sb.LinearStopAtSpeed = linearStopAtSpeed;
            sb.SlowEnoughLinearSpeed = slowEnoughLinearSpeed;
            sb.LinearDrag = linearDrag;
            sb.CloseEnoughDistance = closeEnoughDistance;

            sb.TargetMarker = targetMarker;
            sb.ShowTargetMarker = showTargetMarker;
            sb.ShowVisualizer = showVisualizer;
            sb.ShowOnlyWhenBlocked = showOnlyWhenBlocked;
            sb.LookAheadMultiplier = lookAheadMultiplier;
            sb.ForceMultiplier = forceMultiplier;
            sb.SteeringMultiplier = steeringMultiplier;
        }
        
        void RemoveAndDestroyAvoidWalls()
        {
            if (avoidWalls != null)
            {
                steerableAgent.SteeringData.RemoveSteeringBehaviour(avoidWalls);
                Destroy(avoidWalls);
                avoidWalls = null;
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