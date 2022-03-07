using GameBrains.Actuators.Motion.Movers.UsingControlledTransformWrapper.Movers;
using GameBrains.Extensions.Vectors;
using UnityEngine;

namespace GameBrains.Actuators.Motion.Movers.Controllers
{
    public class MoverPlayerController : PlayerController
    {
        protected Mover Mover;

        public override void Awake()
        {
            base.Awake();
            
            Mover = GetComponent<Mover>();

            if (Mover == null)
            {
                Debug.LogError("Mover required");
            }
        }

        public override void Update()
        {
            base.Update();
            
            if (Mover == null || !isPlayerControlled) return;

            // Get the input vector from keyboard or analog stick
            var directionVectorXZ
                = new VectorXZ(Input.GetAxis(sideAxis), Input.GetAxis(forwardAxis));

            if (directionVectorXZ != VectorXZ.zero)
            {
                // Get the length of the direction vector and then normalize it
                // Dividing by the length is cheaper than normalizing. We already have the length.
                var directionLength = directionVectorXZ.magnitude;
                directionVectorXZ = directionVectorXZ / directionLength;

                // Make sure the length is no bigger than 1
                directionLength = Mathf.Min(1, directionLength);

                // Make the input vector more sensitive towards the extremes
                // and less sensitive in the middle.
                // This makes it easier to control slow speeds when using analog sticks.
                directionLength = directionLength * directionLength;

                //TODO: Maximum speed should be a property (limitation) of the mover.
                float speed
                    = Mathf.Clamp(
                        directionLength * maximumSpeed,
                        -maximumSpeed,
                        maximumSpeed);

                // Apply the direction to the motor
                //TODO: Figure out how to do the rotation on the XZ plane.
                Mover.Velocity = (VectorXZ)(transform.rotation * (Vector3)directionVectorXZ) * speed;
            }
        }
    }
}