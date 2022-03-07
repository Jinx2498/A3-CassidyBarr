using GameBrains.Entities;
using GameBrains.Extensions.MonoBehaviours;
using GameBrains.Extensions.Vectors;
using GameBrains.Motion.Steering.VelocityBased;
using UnityEngine;

namespace Testing
{
    public sealed class W20MTestDepart : ExtendedMonoBehaviour
    {
        public bool respawn;
        public bool respawnTarget;
        public bool setTargetVelocity;
        public bool testUsingTargetLocation;
        public bool testUsingTargetTransform;
        public bool testUsingTargetMovingAgent;
        public bool removeDepartFromSteeringBehaviours;
        public VectorXZ spawnLocation = new VectorXZ(10f, 0f);
        public VectorXZ targetSpawnLocation = new VectorXZ(0f, 0f);
        public VectorXZ targetLocation = new VectorXZ(0f, 10f);
        public VectorXZ targetVelocity = new VectorXZ(0f, 2f);
        public Transform targetTransform;
        public MovingAgent targetMovingAgent;
        public SteerableAgent steerableAgent;
        Depart depart;
        public bool noStop;
        public bool noSlow;
        public bool neverCompletes;
        public float linearStopAtSpeed = 0.1f;
        public float slowEnoughLinearSpeed = 0.5f;
        public float linearDrag = 1.015f;
        public float escapeDistance = 20f;
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

            if (removeDepartFromSteeringBehaviours)
            {
                removeDepartFromSteeringBehaviours = false;
                RemoveAndDestroyDepart();
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
                    RemoveAndDestroyDepart();
                    depart = Depart.CreateInstance(steerableAgent.SteeringData, targetMovingAgent.KinematicData);
                    SetParameters(depart);
                    steerableAgent.SteeringData.AddSteeringBehaviour(depart);
                }
            }

            if (testUsingTargetTransform)
            {
                testUsingTargetTransform = false;

                if (targetTransform != null)
                {
                    RemoveAndDestroyDepart();
                    depart = Depart.CreateInstance(steerableAgent.SteeringData, targetTransform);
                    SetParameters(depart);
                    steerableAgent.SteeringData.AddSteeringBehaviour(depart);
                }
            }

            if (testUsingTargetLocation)
            {
                testUsingTargetLocation = false;
                RemoveAndDestroyDepart();
                depart = Depart.CreateInstance(steerableAgent.SteeringData, targetLocation);
                SetParameters(depart);
                steerableAgent.SteeringData.AddSteeringBehaviour(depart);
            }
        }
        
        void SetParameters(Depart sb)
        {
            sb.NoStop = noStop;
            sb.NoSlow = noSlow;
            sb.NeverCompletes = neverCompletes;
            sb.LinearStopAtSpeed = linearStopAtSpeed;
            sb.SlowEnoughLinearSpeed = slowEnoughLinearSpeed;
            sb.LinearDrag = linearDrag;
            sb.EscapeDistance = escapeDistance;
            sb.BrakingDistance = brakingDistance;
        }
        
        void RemoveAndDestroyDepart()
        {
            if (depart != null)
            {
                steerableAgent.SteeringData.RemoveSteeringBehaviour(depart);
                Destroy(depart);
                depart = null;
            }
        }
    }
}