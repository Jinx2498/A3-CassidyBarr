using GameBrains.Extensions.Vectors;
using UnityEngine;

namespace GameBrains.Actuators.Motion.Movers.UsingTransformWrapper.SimpleMovers
{
    //[RequireComponent(typeof(CharacterController))]
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
            VectorXZ positionOffset;
            if (useSimpleMove)
            {
                positionOffset = Direction * Speed;

                // Type cast from VectorXZ to Vector3 sets Y to 0. Good.
                Agent.CharacterController.SimpleMove((Vector3)positionOffset);
            }
            else
            {
                positionOffset = Direction * (Speed * deltaTime);
                // Type cast from VectorXZ to Vector3 sets Y to 0. Good.
                Agent.CharacterController.Move((Vector3)positionOffset);
                //TODO: Handle gravity
            }
        }
    }
}