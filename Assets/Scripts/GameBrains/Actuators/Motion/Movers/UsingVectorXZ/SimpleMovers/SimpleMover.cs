using GameBrains.Actions;
using GameBrains.Extensions.Attributes;
using GameBrains.Extensions.Vectors;
using UnityEngine;

namespace GameBrains.Actuators.Motion.Movers.UsingVectorXZ.SimpleMovers
{
    public abstract class SimpleMover : Actuator
    {
#pragma warning disable 0414
        // We have multiple versions. This helps tell them apart in the Inspector.
        // ReSharper disable once NotAccessedField.Local
        [ReadOnly] [SerializeField] string version = "UsingVectorXZ";
#pragma warning restore 0414
        
        #region Actuator Limits
        
        [SerializeField] protected float maximumSpeed = 5f;
        [SerializeField] protected float minimumSpeed = 0.01f;
        
        #endregion Actuator Limits

        #region Speed and Direction

        [SerializeField] float speed;
        [SerializeField] VectorXZ direction;

        public virtual float Speed { get => speed; set => speed = value; }
        public virtual VectorXZ Direction { get => direction; set => direction = value; }

        #endregion Speed and Direction

        protected abstract void CalculatePhysics(float deltaTime);

        public override void Update() { base.Update(); CalculatePhysics(Time.deltaTime); }

        protected override void Act(Action action)
        {
            switch (action)
            {
                // TODO: Differentiate kinematic versus dynamic change
                case ChangeSpeedAction changeSpeedAction:
                    Speed = Mathf.Min(changeSpeedAction.desiredSpeed, maximumSpeed);
                    changeSpeedAction.completionStatus = Action.CompletionsStates.Complete;
                    return;
                case ChangeDirectionAction changeDirectionAction:
                    // TODO: Limit direction change?
                    Direction = (VectorXZ)changeDirectionAction.desiredDirection;
                    changeDirectionAction.completionStatus = Action.CompletionsStates.Complete;
                    return;
            }
        }
    }
}