using GameBrains.Actuators.Motion.Movers.UsingControlledTransformWrapper.SimpleMovers;
using GameBrains.Extensions.Vectors;
using UnityEngine;

namespace GameBrains.Actuators.Motion.Movers.Controllers
{
    public class SimpleMoverPlayerController : PlayerController
    {

        protected SimpleMover SimpleMover;

        public override void Awake()
        {
            base.Awake();
            
            SimpleMover = GetComponent<SimpleMover>();

            if (SimpleMover == null)
            {
                Debug.LogError("SimpleMover required");
            }
        }

        public override void Update()
        {
            base.Update();
            
            if (SimpleMover == null || !isPlayerControlled) return;

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
                // This makes it easier to control slow speeds when using analog sticks
                directionLength = directionLength * directionLength;

                //TODO: Maximum speed should be a property (limitation) of the mover.
                SimpleMover.Speed
                    = Mathf.Clamp(
                        directionLength * maximumSpeed,
                        -maximumSpeed,
                        maximumSpeed);

                // Apply the direction to the motor
                //TODO: Figure out how to do the rotation on the XZ plane.
                SimpleMover.Direction = (VectorXZ)(transform.rotation * (Vector3)directionVectorXZ);
            }
        }
    }
}