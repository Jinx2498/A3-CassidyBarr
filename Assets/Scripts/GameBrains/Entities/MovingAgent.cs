using GameBrains.Actuators.Motion.Motors;
using GameBrains.Entities.EntityData;
using GameBrains.Extensions.Vectors;
using UnityEngine;

namespace GameBrains.Entities
{
    public class MovingAgent : Agent
    {
        #region Kinematic Data
        
        public KinematicData KinematicData => staticData as KinematicData;

        #endregion Kinematic Data
        
        #region Motor
        
        protected Motor motor;
        
        public Motor Motor
        {
            get => motor;
            set => motor = value;
        }
        
        #endregion Motor

        #region Awake
        
        public override void Awake()
        {
            base.Awake();

            staticData = (KinematicData)transform;
            
            motor = GetComponentInChildren<Motor>();
        }
        
        #endregion Awake
        
        #region Act

        protected override void Act(float deltaTime)
        {
            base.Act(deltaTime);
            
            if (motor != null && motor.enabled)
            {
                motor.CalculatePhysics(KinematicData, deltaTime);
            }
        }
        
        #endregion Act
        
        #region Spawn

        // Relocate and reactive moving entity. Reset Kinematic Data.
        public override void Spawn(VectorXYZ spawnPoint)
        {
            base.Spawn(spawnPoint);

            KinematicData.ResetKinematicData();

            var characterController = GetComponent<CharacterController>();

            if (characterController != null)
            {
                StaticData.CenterOffset = characterController.center;
                StaticData.Radius = characterController.radius;
                StaticData.Height = characterController.height;
            }
        }

        #endregion
    }
}