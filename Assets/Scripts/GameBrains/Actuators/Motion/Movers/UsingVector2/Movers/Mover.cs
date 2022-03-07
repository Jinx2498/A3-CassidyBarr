using GameBrains.Actions;
using GameBrains.Actuators.Motion.Movers.UsingVector2.SimpleMovers;
using GameBrains.Extensions.Attributes;
using UnityEngine;

namespace GameBrains.Actuators.Motion.Movers.UsingVector2.Movers
{
    [HidePropertiesInInspector("speed", "direction")] // Hide so we don't try to set them
    public abstract class Mover : SimpleMover
    {
        [SerializeField] Vector2 velocity;
        [SerializeField] Vector2 acceleration;

        public virtual Vector2 Velocity
        {
            get => velocity;
            set => velocity = value;
        }

        public virtual Vector2 Acceleration
        {
            get => acceleration;
            set => acceleration = value;
        }

        public override float Speed
        {
            get => velocity.magnitude;
            set => velocity = velocity.normalized * value;
        }

        public override Vector2 Direction
        {
            get => velocity.normalized;
            set => velocity = velocity.magnitude * value;
        }

        public override void Update() { } // We'll use fixed update instead

        public override void FixedUpdate() { base.FixedUpdate(); CalculatePhysics(Time.fixedDeltaTime); }
        
        protected override void Act(Action action)
        {
            base.Act(action);
            
            switch (action)
            {
                case ChangeVelocityAction changeVelocityAction:
                    Velocity = changeVelocityAction.desiredVelocity;
                    changeVelocityAction.completionStatus = Action.CompletionsStates.Complete;
                    return;
                case ChangeAccelerationAction changeAccelerationAction:
                    Acceleration = changeAccelerationAction.desiredAcceleration;
                    changeAccelerationAction.completionStatus = Action.CompletionsStates.Complete;
                    return;
            }
        }
    }
}