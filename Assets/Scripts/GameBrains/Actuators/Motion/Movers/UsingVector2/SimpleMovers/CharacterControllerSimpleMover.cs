using UnityEngine;

namespace GameBrains.Actuators.Motion.Movers.UsingVector2.SimpleMovers
{
    //[RequireComponent(typeof(CharacterController))] // needs to attach to parent
    public sealed class CharacterControllerSimpleMover : SimpleMover
    {
        [SerializeField] bool useSimpleMove;

        public override void Start()
        {
            base.Start();
            SetupCharacterController();
        }

        protected override void CalculatePhysics(float deltaTime)
        {
            if (Speed < minimumSpeed) { return; }
            
            // XY -> XYZ! Not what we want
            Vector3 positionOffset = Direction * Speed;
            
            if (useSimpleMove)
            {
                Agent.CharacterController.SimpleMove(positionOffset);

                // This works but is inelegant. Need a VectorXZ type.
                // var directionXYZ = new Vector3(Direction.x, 0, Direction.y);
                // Agent.CharacterController.SimpleMove(directionXYZ * Speed);
            }
            else
            {
                // XYZ -> XY! Not what we want
                Agent.CharacterController.Move(positionOffset * deltaTime);

                // This works but is inelegant. Need a VectorXZ type.
                // var directionXYZ = new Vector3(Direction.x, 0, Direction.y);
                // Agent.CharacterController.Move(directionXYZ * (Speed * deltaTime));

                //TODO: Handle gravity
            }
        }
    }
}