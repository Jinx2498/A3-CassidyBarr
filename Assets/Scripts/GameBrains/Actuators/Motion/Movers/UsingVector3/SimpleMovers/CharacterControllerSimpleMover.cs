using UnityEngine;

namespace GameBrains.Actuators.Motion.Movers.UsingVector3.SimpleMovers
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
            
            Vector3 positionOffset = Direction * Speed;
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