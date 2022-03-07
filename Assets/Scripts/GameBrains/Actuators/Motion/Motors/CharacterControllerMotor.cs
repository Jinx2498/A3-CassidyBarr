using GameBrains.Entities.EntityData;
using UnityEngine;

namespace GameBrains.Actuators.Motion.Motors
{
    public sealed class CharacterControllerMotor : Motor
    {
        public override void Start()
        {
            base.Start();
            SetupCharacterController();
        }

        public override void CalculatePhysics(KinematicData kinematicData, float deltaTime)
        {
            kinematicData.DoUpdate(deltaTime, false);
            Agent.CharacterController.SimpleMove((Vector3)kinematicData.Velocity);
        }

        // TODO: handle here instead of in agent??
        // void OnControllerColliderHit(ControllerColliderHit hit)
        // {
        //     if (!pushObjects) { return; }
        //     
        //     Rigidbody rb = hit.collider.attachedRigidbody;
        //     
        //     if (rb == null || rb.isKinematic) { return; }
        //
        //     // Don't push objects below
        //     if (hit.moveDirection.y < -0.3) { return; }
        //
        //     var pushDirection = new Vector3(hit.moveDirection.x, 0f, hit.moveDirection.z);
        //
        //     rb.velocity = pushDirection * pushForce;
        // }
    }
}