using GameBrains.Entities;
using GameBrains.Extensions.MonoBehaviours;
using GameBrains.Extensions.Vectors;
using GameBrains.Motion.Steering.VelocityBased;
using UnityEngine;

namespace Testing
{
    public sealed class W20JTestAngularArrive : ExtendedMonoBehaviour
    {
        public bool respawn;
        public bool respawnTarget;
        public bool setAngularVelocity;
        public bool setAngularAcceleration;
        public bool setTargetAngularVelocity;
        public bool testUsingTargetOrientation;
        public bool testUsingTargetTransform;
        public bool testUsingTargetMovingAgent;
        public bool removeAngularArriveFromSteeringBehaviours;
        public VectorXZ spawnLocation = new VectorXZ(10f, 0f);
        public VectorXZ targetSpawnLocation = new VectorXZ(0f, 0f);
        public float angularVelocity = 45f;
        public float angularAcceleration = 5f;
        public float targetOrientation = 90f;
        public float targetAngularVelocity = 45f;
        public Transform targetTransform;
        public MovingAgent targetMovingAgent;
        public SteerableAgent steerableAgent;
        AngularArrive angularArrive;
        public bool noStop;
        public bool noSlow;
        public bool neverCompletes;
        public float angularStopAtSpeed = 0.1f;
        public float slowEnoughAngularVelocity = 5f;
        public float angularDrag = 1.1f;
        public float closeEnoughAngle = 5f;
        public float brakingAngle = 10f;

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

            if (setAngularVelocity)
            {
                setAngularVelocity = false;
                steerableAgent.SteeringData.AngularVelocity = angularVelocity;
            }

            if (setAngularAcceleration)
            {
                setAngularAcceleration = false;
                steerableAgent.SteeringData.AngularAcceleration = angularAcceleration;
            }

            if (removeAngularArriveFromSteeringBehaviours)
            {
                removeAngularArriveFromSteeringBehaviours = false;
                RemoveAndDestroyAngularArrive();
            }
            
            if (setTargetAngularVelocity)
            {
                setTargetAngularVelocity = false;

                if (targetMovingAgent != null)
                {
                    targetMovingAgent.KinematicData.AngularVelocity = targetAngularVelocity;
                }
            }

            if (testUsingTargetMovingAgent)
            {
                testUsingTargetMovingAgent = false;

                if (targetMovingAgent != null)
                {
                    RemoveAndDestroyAngularArrive();
                    angularArrive 
                        = AngularArrive.CreateInstance(steerableAgent.SteeringData, targetMovingAgent.KinematicData);
                    SetParameters(angularArrive);
                    steerableAgent.SteeringData.AddSteeringBehaviour(angularArrive);
                }
            }

            if (testUsingTargetTransform)
            {
                testUsingTargetTransform = false;

                if (targetTransform != null)
                {
                    RemoveAndDestroyAngularArrive();
                    angularArrive = AngularArrive.CreateInstance(steerableAgent.SteeringData, targetTransform);
                    SetParameters(angularArrive);
                    steerableAgent.SteeringData.AddSteeringBehaviour(angularArrive);
                }
            }

            if (testUsingTargetOrientation)
            {
                testUsingTargetOrientation = false;
                RemoveAndDestroyAngularArrive();
                angularArrive = AngularArrive.CreateInstance(steerableAgent.SteeringData, targetOrientation);
                SetParameters(angularArrive);
                steerableAgent.SteeringData.AddSteeringBehaviour(angularArrive);
            }
        }
        
        void SetParameters(AngularArrive sb)
        {
            sb.NoStop = noStop;
            sb.NoSlow = noSlow;
            sb.NeverCompletes = neverCompletes;
            sb.AngularStopAtSpeed = angularStopAtSpeed;
            sb.SlowEnoughAngularVelocity = slowEnoughAngularVelocity;
            sb.AngularDrag = angularDrag;
            sb.CloseEnoughAngle = closeEnoughAngle;
            sb.BrakingAngle = brakingAngle;
        }
        
        void RemoveAndDestroyAngularArrive()
        {
            if (angularArrive != null)
            {
                steerableAgent.SteeringData.RemoveSteeringBehaviour(angularArrive);
                Destroy(angularArrive);
                angularArrive = null;
            }
        }
    }
}