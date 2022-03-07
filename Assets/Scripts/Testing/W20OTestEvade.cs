using GameBrains.Entities;
using GameBrains.Extensions.MonoBehaviours;
using GameBrains.Extensions.Vectors;
using GameBrains.Motion.Steering.VelocityBased;

namespace Testing
{
    public sealed class W20OTestEvade : ExtendedMonoBehaviour
    {
        public bool respawn;
        public bool respawnTarget;
        public bool setTargetVelocity;
        public bool testUsingTargetMovingEvade;
        public bool removeEvadeFromSteeringBehaviours;
        public VectorXZ spawnLocation = new VectorXZ(10f, 0f);
        public VectorXZ targetSpawnLocation = new VectorXZ(0f, 0f);
        public VectorXZ targetVelocity = new VectorXZ(0f, 2f);
        public MovingAgent targetMovingAgent;
        public SteerableAgent steerableAgent;
        Evade evade;
        public bool noStop;
        public bool noSlow;
        public bool neverCompletes;
        public float linearStopAtSpeed = 0.1f;
        public float slowEnoughLinearSpeed = 0.5f;
        public float linearDrag = 1.015f;
        public float escapeDistance = 20f;

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

            if (removeEvadeFromSteeringBehaviours)
            {
                removeEvadeFromSteeringBehaviours = false;
                RemoveAndDestroyEvade();
            }

            if (setTargetVelocity)
            {
                setTargetVelocity = false;

                if (targetMovingAgent != null)
                {
                    targetMovingAgent.KinematicData.Velocity = targetVelocity;
                }
            }

            if (testUsingTargetMovingEvade)
            {
                testUsingTargetMovingEvade = false;

                if (targetMovingAgent != null)
                {
                    RemoveAndDestroyEvade();
                    evade = Evade.CreateInstance(steerableAgent.SteeringData, targetMovingAgent.KinematicData);
                    SetParameters(evade);
                    steerableAgent.SteeringData.AddSteeringBehaviour(evade);
                }
            }
        }
        
        void SetParameters(Evade sb)
        {
            sb.NoStop = noStop;
            sb.NoSlow = noSlow;
            sb.NeverCompletes = neverCompletes;
            sb.LinearStopAtSpeed = linearStopAtSpeed;
            sb.SlowEnoughLinearSpeed = slowEnoughLinearSpeed;
            sb.LinearDrag = linearDrag;
            sb.EscapeDistance = escapeDistance;
        }
        
        void RemoveAndDestroyEvade()
        {
            if (evade != null)
            {
                steerableAgent.SteeringData.RemoveSteeringBehaviour(evade);
                Destroy(evade);
                evade = null;
            }
        }
    }
}