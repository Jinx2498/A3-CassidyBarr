using GameBrains.Entities;
using GameBrains.Extensions.MonoBehaviours;
using GameBrains.Extensions.Vectors;
using GameBrains.Motion.Steering.VelocityBased;

namespace Testing
{
    public sealed class W20NTestPursue : ExtendedMonoBehaviour
    {
        public bool respawn;
        public bool respawnTarget;
        public bool setTargetVelocity;
        public bool testUsingTargetMovingAgent;
        public bool removePursueFromSteeringBehaviours;
        public VectorXZ spawnLocation = new VectorXZ(10f, 0f);
        public VectorXZ targetSpawnLocation = new VectorXZ(0f, 0f);
        public VectorXZ targetVelocity = new VectorXZ(0f, 2f);
        public MovingAgent targetMovingAgent;
        public SteerableAgent steerableAgent;
        Pursue pursue;
        public bool noStop;
        public bool noSlow;
        public bool neverCompletes;
        public float linearStopAtSpeed = 0.1f;
        public float slowEnoughLinearSpeed = 0.5f;
        public float linearDrag = 1.015f;
        public float closeEnoughDistance = 1.5f;

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

            if (removePursueFromSteeringBehaviours)
            {
                removePursueFromSteeringBehaviours = false;
                RemoveAndDestroyPursue();
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
                    RemoveAndDestroyPursue();
                    pursue = Pursue.CreateInstance(steerableAgent.SteeringData, targetMovingAgent.KinematicData);
                    SetParameters(pursue);
                    steerableAgent.SteeringData.AddSteeringBehaviour(pursue);
                }
            }
        }
        
        void SetParameters(Pursue sb)
        {
            sb.NoStop = noStop;
            sb.NoSlow = noSlow;
            sb.NeverCompletes = neverCompletes;
            sb.LinearStopAtSpeed = linearStopAtSpeed;
            sb.SlowEnoughLinearSpeed = slowEnoughLinearSpeed;
            sb.LinearDrag = linearDrag;
            sb.CloseEnoughDistance = closeEnoughDistance;
        }
        
        void RemoveAndDestroyPursue()
        {
            if (pursue != null)
            {
                steerableAgent.SteeringData.RemoveSteeringBehaviour(pursue);
                Destroy(pursue);
                pursue = null;
            }
        }
    }
}