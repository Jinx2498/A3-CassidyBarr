using GameBrains.Actions;
using GameBrains.Actuators.Motion.Movers.UsingVector3.SimpleMovers;
using GameBrains.Extensions.Attributes;
using UnityEngine;

namespace GameBrains.Actuators.Motion.Movers.UsingVector3.Movers
{
    [HidePropertiesInInspector("speed", "direction")] // Hide so we don't try to set them
    public abstract class Mover : SimpleMover
    {
        [SerializeField] Vector3 velocity;
        [SerializeField] Vector3 acceleration;

        public virtual Vector3 Velocity
        {
            get => velocity;
            set => velocity = value;
        }

        public virtual Vector3 Acceleration
        {
            get => acceleration;
            set => acceleration = value;
        }

        public override float Speed
        {
            get => velocity.magnitude;
            set => velocity = velocity.normalized * value;
        }

        public override Vector3 Direction
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