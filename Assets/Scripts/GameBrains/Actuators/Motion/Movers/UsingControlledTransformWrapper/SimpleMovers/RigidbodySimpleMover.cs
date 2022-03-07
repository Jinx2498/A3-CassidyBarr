using GameBrains.Extensions.Vectors;
using UnityEngine;

namespace GameBrains.Actuators.Motion.Movers.UsingControlledTransformWrapper.SimpleMovers
{
    //[RequireComponent(typeof(Rigidbody))]
    public sealed class RigidbodySimpleMover : SimpleMover
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
            VectorXZ positionOffset = Direction * Speed;

            if (useForce)
            {
                throw new System.NotImplementedException(
                    "Homework: How can we use Agent.Rigidbody.AddForce to move properly?");
            }
            else
            {
                // Type cast from VectorXZ to Vector3 sets Y to 0. Good.
                Agent.Rigidbody.velocity = (Vector3)positionOffset;
            }
        }
    }
}