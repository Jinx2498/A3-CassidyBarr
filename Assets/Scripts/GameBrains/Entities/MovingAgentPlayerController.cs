using GameBrains.Extensions.MonoBehaviours;
using GameBrains.Extensions.Vectors;
using UnityEngine;

namespace GameBrains.Entities
{
    public class MovingAgentPlayerController : ExtendedMonoBehaviour
    {
        //TODO: There should be a motor maximum speed and a player maximum speed
        [SerializeField] protected float maximumSpeed = 5;

        protected MovingAgent movingAgent;
        
        //TODO: Encapsulate these fields
        [SerializeField] protected string sideAxis = "Horizontal";
        [SerializeField] protected string forwardAxis = "Vertical";

        public override void Awake()
        {
            base.Awake();
            
            movingAgent = GetComponent<MovingAgent>();

            if (movingAgent == null)
            {
                Debug.LogError("MovingAgent required");
            }
        }
        
        public override void Update()
        {
            base.Update();
            
            if (movingAgent == null || !movingAgent.IsPlayerControlled) { return; }

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
                movingAgent.KinematicData.Velocity
                    = (VectorXZ)(transform.rotation * (Vector3)directionVectorXZ) * speed;
            }
        }
    }
}