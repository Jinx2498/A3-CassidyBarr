using UnityEngine;

namespace GameBrains.Actuators.Motion.Movers.UsingControlledTransformWrapper.Movers
{
    //[RequireComponent(typeof(Rigidbody))]
    public sealed class RigidbodyMover : Mover
    {
        //TODO: Encapsulate field. Currently always false.
        [HideInInspector] [SerializeField] bool useForce;

        public override void Start()
        {
            base.Start();
            SetupRigidbody();
        }

        protected override void CalculatePhysics(float deltaTime)
        {
            // Use average of Vinitial and Vfinal
            // deltaP = (Vinital + Vfinal) / 2 * t
            // Vfinal = Vinitial + A * t
            // deltaP = (Vinitial + Vinitial + A * t) / 2 * t
            // deltaP = (2 * Vinitial + A * t) / 2 * t
            // deltaP = Vinitial * t + A * t * t / 2
            //float halfDeltaTimeSquared = (deltaTime * deltaTime) / 2;
            //VectorXZ positionOffset = (Velocity * deltaTime) + (Acceleration * halfDeltaTimeSquared);
            Velocity += Acceleration * deltaTime;

            if (useForce)
            {
                throw new System.NotImplementedException(
                    "Homework: How can we use Agent.Rigidbody.AddForce to move properly?");
            }
            else
            {
                // Type cast from VectorXZ to Vector3 sets Y to 0. Good.
                Agent.Rigidbody.velocity = (Vector3)Velocity;
            }
        }
    }
}