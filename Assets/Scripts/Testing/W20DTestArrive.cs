using GameBrains.Entities;
using GameBrains.Extensions.MonoBehaviours;
using GameBrains.Extensions.Vectors;
using GameBrains.Motion.Steering.VelocityBased;
using UnityEngine;

namespace Testing
{
    public sealed class W20DTestArrive : ExtendedMonoBehaviour
    {
        public bool respawn;
        public bool respawnTarget;
        public bool setTargetVelocity;
        public bool testUsingTargetLocation;
        public bool testUsingTargetTransform;
        public bool testUsingTargetMovingAgent;
        public bool removeArriveFromSteeringBehaviours;
        public VectorXZ spawnLocation = new VectorXZ(10f, 0f);
        public VectorXZ targetSpawnLocation = new VectorXZ(0f, 0f);
        public VectorXZ targetLocation = new VectorXZ(0f, 10f);
        public VectorXZ targetVelocity = new VectorXZ(0f, 2f);
        public Transform targetTransform;
        public MovingAgent targetMovingAgent;
        public SteerableAgent steerableAgent;
        Arrive arrive;
        public bool noStop;
        public bool noSlow;
        public bool neverCompletes;
        public float linearStopAtSpeed = 0.1f;
        public float slowEnoughLinearSpeed = 0.5f;
        public float linearDrag = 1.015f;
        public float closeEnoughDistance = 1.5f;
        public float brakingDistance = 5f;

        public override void Update()
        {
            base.Update();
            
            if (steerableAgent == null) { return; }

            if (respawn)
            {
                respawn = false;
                steerableAgent.Spawn((VectorXYZ)spawnLocation);
            }
            
            if (respawnTarget)
            {
                respawnTarget = false;
                targetMovingAgent.Spawn((VectorXYZ)targetSpawnLocation);
            }

            if (removeArriveFromSteeringBehaviours)
            {
                removeArriveFromSteeringBehaviours = false;
                RemoveAndDestroyArrive();
            }
            
            if (setTargetVelocity)
            {
                setTargetVelocity = false;

                if (targetMovingAgent != null)
                {
                    targetMovingAgent.KinematicData.Velocity = targetVelocity;
                }
            }

            if (testUsingTargetMovingAgent)
            {
                testUsingTargetMovingAgent = false;

                if (targetMovingAgent != null)
                {
                    RemoveAndDestroyArrive();
                    arrive = Arrive.CreateInstance(steerableAgent.SteeringData, targetMovingAgent.KinematicData);
                    SetParameters(arrive);
                    steerableAgent.SteeringData.AddSteeringBehaviour(arrive);
                }
            }

            if (testUsingTargetTransform)
            {
                testUsingTargetTransform = false;

                if (targetTransform != null)
                {
                    RemoveAndDestroyArrive();
                    arrive = Arrive.CreateInstance(steerableAgent.SteeringData, targetTransform);
                    SetParameters(arrive);
                    steerableAgent.SteeringData.AddSteeringBehaviour(arrive);
                }
            }

            if (testUsingTargetLocation)
            {
                testUsingTargetLocation = false;
                RemoveAndDestroyArrive();
                arrive = Arrive.CreateInstance(steerableAgent.SteeringData, targetLocation);
                SetParameters(arrive);
                steerableAgent.SteeringData.AddSteeringBehaviour(arrive);
            }
        }
        
        void SetParameters(Arrive sb)
        {
            sb.NoStop = noStop;
            sb.NoSlow = noSlow;
            sb.NeverCompletes = neverCompletes;
            sb.LinearStopAtSpeed = linearStopAtSpeed;
            sb.SlowEnoughLinearSpeed = slowEnoughLinearSpeed;
            sb.LinearDrag = linearDrag;
            sb.CloseEnoughDistance = closeEnoughDistance;
            sb.BrakingDistance = brakingDistance;
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