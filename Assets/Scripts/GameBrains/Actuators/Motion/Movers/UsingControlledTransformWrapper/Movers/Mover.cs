using GameBrains.Actuators.Motion.Movers.UsingControlledTransformWrapper.SimpleMovers;
using GameBrains.Extensions.Attributes;
using GameBrains.Extensions.Vectors;
using UnityEngine;

namespace GameBrains.Actuators.Motion.Movers.UsingControlledTransformWrapper.Movers
{
    [HidePropertiesInInspector("speed", "direction")] // Hide so we don't try to set them
    public abstract class Mover : SimpleMover
    {
        [HideInInspector] [SerializeField] VectorXZ velocity;
        [HideInInspector] [SerializeField] VectorXZ acceleration;

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


        public override void Update() { }

        public override void FixedUpdate() { base.FixedUpdate(); CalculatePhysics(Time.fixedDeltaTime); }
    }
}