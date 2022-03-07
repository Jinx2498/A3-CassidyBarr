using GameBrains.Entities;
using GameBrains.Extensions.MonoBehaviours;
using GameBrains.Extensions.Vectors;
using GameBrains.Motion.Steering.VelocityBased;

namespace Testing
{
    public sealed class W20ATestLinearStop : ExtendedMonoBehaviour
    {
        public bool respawn;
        public bool removeLinearStopFromSteeringBehaviours;
        public bool setVelocity;
        public bool setAcceleration;
        public bool testLinearStop;
        public VectorXZ spawnLocation = new VectorXZ(10f, 0f);
        public VectorXZ velocity = new VectorXZ(2f, 5f);
        public VectorXZ acceleration = new VectorXZ(0.5f, 0.5f);
        public SteerableAgent steerableAgent;
        LinearStop linearStop;
        public bool noStop;
        public bool neverCompletes;
        public float linearStopAtSpeed = 0.1f;

        public override void Update()
        {
            base.Update();
            
            if (steerableAgent == null) { return; }

            if (respawn)
            {
                respawn = false;
                steerableAgent.Spawn((VectorXYZ)spawnLocation);
            }

            if (removeLinearStopFromSteeringBehaviours)
            {
                removeLinearStopFromSteeringBehaviours = false;
                RemoveAndDestroyLinearStop();
            }

            if (setVelocity)
            {
                setVelocity = false;
                steerableAgent.SteeringData.Velocity = velocity;
            }

            if (setAcceleration)
            {
                setAcceleration = false;
                steerableAgent.SteeringData.Acceleration = acceleration;
            }

            if (testLinearStop)
            {
                testLinearStop = false;
                RemoveAndDestroyLinearStop(); // prevent multiple adds
                linearStop = LinearStop.CreateInstance(steerableAgent.SteeringData);
                SetParameters(linearStop);
                steerableAgent.SteeringData.AddSteeringBehaviour(linearStop);
            }
        }
        
        void SetParameters(LinearStop sb)
        {
            sb.NoStop = noStop;
            sb.NeverCompletes = neverCompletes;
            sb.LinearStopAtSpeed = linearStopAtSpeed;
        }
        
        void RemoveAndDestroyLinearStop()
        {
            if (linearStop != null)
            {
                steerableAgent.SteeringData.RemoveSteeringBehaviour(linearStop);
                Destroy(linearStop);
                linearStop = null;
            }
        }
    }
}