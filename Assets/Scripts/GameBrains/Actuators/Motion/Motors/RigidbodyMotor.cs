using GameBrains.Entities.EntityData;
using UnityEngine;

namespace GameBrains.Actuators.Motion.Motors
{
    public sealed class RigidbodyMotor : Motor
    {
        public override void Start()
        {
            base.Start();
            SetupRigidbody();
        }

        public override void CalculatePhysics(KinematicData kinematicData, float deltaTime)
        {
            kinematicData.DoUpdate(deltaTime, false);
            Agent.Rigidbody.velocity = (Vector3)kinematicData.Velocity;
        }
    }
}