using GameBrains.Entities;
using GameBrains.Extensions.MonoBehaviours;
using GameBrains.Extensions.Vectors;
using GameBrains.Motion.Steering.VelocityBased;

namespace Testing
{
    public sealed class W20FTestAngularSlow : ExtendedMonoBehaviour
    {
        public bool respawn;
        public bool removeAngularSlowFromSteeringBehaviours;
        public bool setAngularVelocity;
        public bool setAngularAcceleration;
        public bool testAngularSlow;
        public VectorXZ spawnLocation = new VectorXZ(10f, 0f);
        public float angularVelocity = 45f;
        public float angularAcceleration = 5f;
        public SteerableAgent steerableAgent;
        AngularSlow angularSlow;
        public bool noStop;
        public bool noSlow;
        public bool neverCompletes;
        public float angularStopAtSpeed = 0.1f;
        public float slowEnoughAngularVelocity = 5f;
        public float angularDrag = 1.1f;

        public override void Update()
        {
            base.Update();
            
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

            if (removeAngularSlowFromSteeringBehaviours)
            {
                removeAngularSlowFromSteeringBehaviours = false;
                RemoveAndDestroyAngularSlow();
            }

            if (testAngularSlow)
            {
                testAngularSlow = false;
                RemoveAndDestroyAngularSlow();
                angularSlow = AngularSlow.CreateInstance(steerableAgent.SteeringData);
                SetParameters(angularSlow);
                steerableAgent.SteeringData.AddSteeringBehaviour(angularSlow);
            }
        }
        
        void SetParameters(AngularSlow sb)
        {
            sb.NoStop = noStop;
            sb.NoSlow = noSlow;
            sb.NeverCompletes = neverCompletes;
            sb.AngularStopAtSpeed = angularStopAtSpeed;
            sb.SlowEnoughAngularVelocity = slowEnoughAngularVelocity;
            sb.AngularDrag = angularDrag;
        }
        
        void RemoveAndDestroyAngularSlow()
        {
            if (angularSlow != null)
            {
                steerableAgent.SteeringData.RemoveSteeringBehaviour(angularSlow);
                Destroy(angularSlow);
                angularSlow = null;
            }
        }
    }
}