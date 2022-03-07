using GameBrains.Entities;
using GameBrains.Extensions.MonoBehaviours;
using GameBrains.Extensions.Vectors;
using GameBrains.Motion.Steering.VelocityBased;

namespace Testing
{
    public sealed class W20ETestAngularStop : ExtendedMonoBehaviour
    {
        public bool respawn;
        public bool removeAngularStopFromSteeringBehaviours;
        public bool setAngularVelocity;
        public bool setAngularAcceleration;
        public bool testAngularStop;
        public VectorXZ spawnLocation = new VectorXZ(10f, 0f);
        public float angularVelocity = 45f;
        public float angularAcceleration = 5f;
        public SteerableAgent steerableAgent;
        AngularStop angularStop;
        public bool noStop;
        public bool neverCompletes;
        public float angularStopAtSpeed = 0.1f;

        public override void Update()
        {
            if (steerableAgent == null) { return; }

            if (respawn)
            {
                respawn = false;
                steerableAgent.Spawn((VectorXYZ)spawnLocation);
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

            if (removeAngularStopFromSteeringBehaviours)
            {
                removeAngularStopFromSteeringBehaviours = false;
                RemoveAndDestroyAngularStop();
            }

            if (testAngularStop)
            {
                testAngularStop = false;
                RemoveAndDestroyAngularStop();
                angularStop = AngularStop.CreateInstance(steerableAgent.SteeringData);
                SetParameters(angularStop);
                steerableAgent.SteeringData.AddSteeringBehaviour(angularStop);
            }
        }
        
        void SetParameters(AngularStop sb)
        {
            sb.NoStop = noStop;
            sb.NeverCompletes = neverCompletes;
            sb.AngularStopAtSpeed = angularStopAtSpeed;
        }
        
        void RemoveAndDestroyAngularStop()
        {
            if (angularStop != null)
            {
                steerableAgent.SteeringData.RemoveSteeringBehaviour(angularStop);
                Destroy(angularStop);
                angularStop = null;
            }
        }
    }
}