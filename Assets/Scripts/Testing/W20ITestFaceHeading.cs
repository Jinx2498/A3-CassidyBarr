using GameBrains.Actuators.Motion.Steering.VelocityBased;
using GameBrains.Entities;
using GameBrains.Extensions.MonoBehaviours;
using GameBrains.Extensions.Vectors;

namespace Testing
{
    public sealed class W20ITestFaceHeading : ExtendedMonoBehaviour
    {
        public bool respawn;
        public bool removeFaceHeadingFromSteeringBehaviours;
        public bool testFaceHeading;
        public bool setVelocity;
        public bool setAcceleration;
        public VectorXZ spawnLocation = new VectorXZ(10f, 0f);
        public VectorXZ velocity = new VectorXZ(2f,5f);
        public VectorXZ acceleration = new VectorXZ(0.5f, 0.5f);
        public SteerableAgent steerableAgent;
        FaceHeading faceHeading;
        public bool noStop;
        public bool noSlow;
        public bool neverCompletes;
        public float angularStopAtSpeed = 0.1f;
        public float slowEnoughAngularVelocity = 5f;
        public float angularDrag = 1.1f;
        public float closeEnoughAngle = 5f;

        public override void Update()
        {
            base.Update();
            
            if (steerableAgent == null) { return; }

            if (respawn)
            {
                respawn = false;
                steerableAgent.Spawn((VectorXYZ)spawnLocation);
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

            if (removeFaceHeadingFromSteeringBehaviours)
            {
                removeFaceHeadingFromSteeringBehaviours = false;
                RemoveAndDestroyFaceHeading();
            }

            if (testFaceHeading)
            {
                testFaceHeading = false;
                RemoveAndDestroyFaceHeading();
                faceHeading = FaceHeading.CreateInstance(steerableAgent.SteeringData);
                SetParameters(faceHeading);
                steerableAgent.SteeringData.AddSteeringBehaviour(faceHeading);
            }
        }
        
        void SetParameters(FaceHeading sb)
        {
            sb.NoStop = noStop;
            sb.NoSlow = noSlow;
            sb.NeverCompletes = neverCompletes;
            sb.AngularStopAtSpeed = angularStopAtSpeed;
            sb.SlowEnoughAngularVelocity = slowEnoughAngularVelocity;
            sb.AngularDrag = angularDrag;
            sb.CloseEnoughAngle = closeEnoughAngle;
        }
        
        void RemoveAndDestroyFaceHeading()
        {
            if (faceHeading != null)
            {
                steerableAgent.SteeringData.RemoveSteeringBehaviour(faceHeading);
                Destroy(faceHeading);
                faceHeading = null;
            }
        }
    }
}