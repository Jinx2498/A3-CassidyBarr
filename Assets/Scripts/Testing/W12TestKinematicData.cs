using GameBrains.Entities.EntityData;
using GameBrains.Extensions.Vectors;
using GameBrains.Visualization;
using UnityEngine;

namespace Testing
{
    [AddComponentMenu("Scripts/Testing/W12 Test Kinematic Data")]
    public class W12TestKinematicData : W11TestStaticData
    {
        public bool checkCanMoveTo;
        public bool checkCanStepLeft;
        public bool checkCanStepRight;
        
        public CapsuleCastVisualizer capsuleCastVisualizer;
        public float castRadiusMultiplier = 1.0f;
        
        public VectorXZ velocity;
        public VectorXZ acceleration;

        public bool setVelocity;
        public bool setAcceleration;
        
        VectorXYZ lastPosition;

        public KinematicData KinematicData => (KinematicData)staticData;

        public override void Awake()
        {
            base.Awake();
            staticData = (KinematicData)agentTransform;
            capsuleCastVisualizer = ScriptableObject.CreateInstance<CapsuleCastVisualizer>();
            lastPosition = KinematicData.Position;
        }

        public override void Update()
        {
            base.Update();

            if (setVelocity)
            {
                setVelocity = false;
                KinematicData.Velocity = velocity;
            }

            if (setAcceleration)
            {
                setAcceleration = false;
                KinematicData.Acceleration = acceleration;
            }
            
            if (checkCanMoveTo)
            {
                checkCanMoveTo = false;

                Debug.Log(
                    KinematicData.CanMoveTo(
                        moveTargetPosition,
                        capsuleCastVisualizer,
                        true,
                        false,
                        castRadiusMultiplier));
            }

            if (checkCanStepLeft)
            {
                checkCanStepLeft = false;

                Debug.Log(
                    KinematicData.CanStepLeft(
                        capsuleCastVisualizer, 
                        true,
                        false,
                        castRadiusMultiplier));
            }

            if (checkCanStepRight)
            {
                checkCanStepRight = false;

                Debug.Log(
                    KinematicData.CanStepRight(
                        capsuleCastVisualizer, 
                        true,
                        false,
                        castRadiusMultiplier));
            }

            if (checkCanMoveTo)
            {
                checkCanMoveTo = false;

                Debug.Log(
                    KinematicData.CanMoveTo(
                        moveTargetPosition,
                        capsuleCastVisualizer,
                        true,
                        false,
                        castRadiusMultiplier));
            }

            KinematicData.DoUpdate(Time.deltaTime);

            if (lastPosition != KinematicData.Position)
            {
                lastPosition = KinematicData.Position;
                Debug.Log("P:" + KinematicData.Position + " V: " + KinematicData.Velocity);
            }
        }
    }
}