using GameBrains.Actions;
using GameBrains.Actuators.Motion.Movers.UsingVectorXZ.SimpleMovers;
using GameBrains.Extensions.Attributes;
using GameBrains.Extensions.Vectors;
using UnityEngine;

namespace GameBrains.Actuators.Motion.Movers.UsingVectorXZ.Movers
{
    [HidePropertiesInInspector("speed", "direction")] // Hide so we don't try to set them
    public abstract class Mover : SimpleMover
    {
        [SerializeField] VectorXZ velocity;
        [SerializeField] VectorXZ acceleration;

        public virtual VectorXZ Velocity
        {
            get => velocity;
            set => velocity = value;
        }

        public virtual VectorXZ Acceleration
        {
            get => acceleration;
            set => acceleration = value;
        }

        public override float Speed
        {
            get => velocity.magnitude;
            set => velocity = velocity.normalized * value;
        }

        public override VectorXZ Direction
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
                    Velocity = (VectorXZ)changeVelocityAction.desiredVelocity;
                    changeVelocityAction.completionStatus = Action.CompletionsStates.Complete;
                    return;
                case ChangeAccelerationAction changeAccelerationAction:
                    Acceleration = (VectorXZ)changeAccelerationAction.desiredAcceleration;
                    changeAccelerationAction.completionStatus = Action.CompletionsStates.Complete;
                    return;
            }
        }
    }
}