using GameBrains.Entities;
using GameBrains.Extensions.MonoBehaviours;
using GameBrains.Extensions.Vectors;
using GameBrains.Motion.Steering.VelocityBased;

namespace Testing
{
    public sealed class W20BTestLinearSlow : ExtendedMonoBehaviour
    {
        public bool respawn;
        public bool removeLinearSlowFromSteeringBehaviours;
        public bool setVelocity;
        public bool setAcceleration;
        public bool testLinearSlow;
        public VectorXZ spawnLocation = new VectorXZ(10f, 0f);
        public VectorXZ velocity = new VectorXZ(2f, 5f);
        public VectorXZ acceleration = new VectorXZ(0.5f, 0.5f);
        public SteerableAgent steerableAgent;
        LinearSlow linearSlow;
        public bool noStop;
        public bool noSlow;
        public bool neverCompletes;
        public float linearStopAtSpeed = 0.1f;
        public float slowEnoughLinearSpeed = 0.5f;
        public float linearDrag = 1.015f;

        public override void Update()
        {
            base.Update();
            
            if (steerableAgent == null) { return; }

            if (respawn)
            {
                respawn = false;
                steerableAgent.Spawn((VectorXYZ)spawnLocation);
            }

            if (removeLinearSlowFromSteeringBehaviours)
            {
                removeLinearSlowFromSteeringBehaviours = false;
                RemoveAndDestroyLinearSlow();
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

            if (testLinearSlow)
            {
                testLinearSlow = false;
                RemoveAndDestroyLinearSlow(); // prevent multiple adds
                linearSlow = LinearSlow.CreateInstance(steerableAgent.SteeringData);
                SetParameters(linearSlow);
                steerableAgent.SteeringData.AddSteeringBehaviour(linearSlow);
            }
        }

        void SetParameters(LinearSlow sb)
        {
            sb.NoStop = noStop;
            sb.NoSlow = noSlow;
            sb.NeverCompletes = neverCompletes;
            sb.LinearStopAtSpeed = linearStopAtSpeed;
            sb.SlowEnoughLinearSpeed = slowEnoughLinearSpeed;
            sb.LinearDrag = linearDrag;
        }

        void RemoveAndDestroyLinearSlow()
        {
            if (linearSlow != null)
            {
                steerableAgent.SteeringData.RemoveSteeringBehaviour(linearSlow);
                Destroy(linearSlow);
                linearSlow = null;
            }
        }
    }
}