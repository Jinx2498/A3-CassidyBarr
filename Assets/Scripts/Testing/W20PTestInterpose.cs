using GameBrains.Actuators.Motion.Steering.VelocityBased;
using GameBrains.Entities;
using GameBrains.Extensions.MonoBehaviours;
using GameBrains.Extensions.Vectors;
using UnityEngine;

namespace Testing
{
    public sealed class W20PTestInterpose : ExtendedMonoBehaviour
    {
        public bool respawn;
        public bool respawnTarget;
        public bool setTargetVelocity;
        public bool testUsingTargetMovingAgent;
        public bool removeInterposeFromSteeringBehaviours;
        public VectorXZ spawnLocation = new VectorXZ(10f, 0f);
        public VectorXZ targetSpawnLocation = new VectorXZ(0f, 0f);
        public VectorXZ targetVelocity = new VectorXZ(0f, 2f);
        public MovingAgent targetMovingAgent;
        public Transform secondTargetTransform;
        public SteerableAgent steerableAgent;
        Interpose interpose;
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

            if (removeInterposeFromSteeringBehaviours)
            {
                removeInterposeFromSteeringBehaviours = false;
                RemoveAndDestroyInterpose();
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
                    RemoveAndDestroyInterpose();
                    interpose = Interpose.CreateInstance(
                        steerableAgent.SteeringData,
                        targetMovingAgent.KinematicData,
                        secondTargetTransform);
                    SetParameters(interpose);
                    steerableAgent.SteeringData.AddSteeringBehaviour(interpose);
                }
            }
        }
        
        void SetParameters(Interpose sb)
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
        
        void RemoveAndDestroyInterpose()
        {
            if (interpose != null)
            {
                steerableAgent.SteeringData.RemoveSteeringBehaviour(interpose);
                Destroy(interpose);
                interpose = null;
            }
        }
    }
}