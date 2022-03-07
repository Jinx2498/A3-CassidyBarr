using GameBrains.Entities.EntityData;
using GameBrains.Extensions.Vectors;
using UnityEngine;

namespace GameBrains.Entities
{
    public class SteerableAgent : MovingAgent
    {
        #region Character Controller
        
        [SerializeField] bool pushObjects;
        [SerializeField] float pushForce;
        
        #endregion
        
        #region Motion

        public SteeringData SteeringData => staticData as SteeringData;

        #endregion Motion

        public override void Awake()
        {
            base.Awake();

            staticData = (SteeringData) transform;
        }

        protected override void Act(float deltaTime)
        {
            if (!IsPlayerControlled)
            {
                SteeringData.CalculateSteering();
            }

            base.Act(deltaTime);
        }
        
        // Relocate and reactive moving entity. Reset Kinematic Data.
        public override void Spawn(VectorXYZ spawnPoint)
        {
            base.Spawn(spawnPoint);

            SteeringData.ResetSteeringData();
        }

        // TODO: Set up as event??
        void OnControllerColliderHit(ControllerColliderHit other)
        {
            if (other.collider.gameObject.layer == LayerMask.NameToLayer("Ground")) { return; }

            if (!pushObjects) { return; }

            Rigidbody rb = other.collider.attachedRigidbody;

            if (rb == null || rb.isKinematic) { return; }

            // Don't push objects below
            if (other.moveDirection.y <= -0.3f) { return; }

            var pushDirection = new Vector3(other.moveDirection.x, 0f, other.moveDirection.z);

            rb.velocity += pushDirection * pushForce;
        }
    }
}