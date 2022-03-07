using UnityEngine;

namespace GameBrains.Actuators.Motion.Movers.UsingVectorXZ.SimpleMovers
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
            
            // Type cast from VectorXZ to Vector3 sets Y to 0. Good.
            Vector3 positionOffset = (Vector3)Direction * Speed;
            
            if (useSimpleMove)
            {
                Agent.CharacterController.SimpleMove(positionOffset);
            }
            else
            {
                Agent.CharacterController.Move(positionOffset * deltaTime);
                //TODO: Handle gravity
            }
        }
    }
}