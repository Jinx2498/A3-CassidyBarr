using GameBrains.Extensions.Attributes;
using GameBrains.Extensions.Transforms;
using GameBrains.Extensions.Vectors;
using UnityEngine;

namespace GameBrains.Actuators.Motion.Movers.UsingTransformWrapper.SimpleMovers
{
    public abstract class SimpleMover : Actuator
    {
        #region Version
#pragma warning disable 0414
        // We have multiple versions. This helps tell them apart in the Inspector.
        // ReSharper disable once NotAccessedField.Local
        [ReadOnly] [SerializeField] string version = "UsingTransformWrapper";
#pragma warning restore 0414

        #endregion Version

        #region Transform Wrapper

        [SerializeField] TransformWrapper transformWrapper;
        public TransformWrapper TransformWrapper => transformWrapper;

        public VectorXZ Location
        {
            get => transformWrapper.Location;
            set => transformWrapper.Location = value;
        }

        public VectorXYZ Position
        {
            get => transformWrapper.Position;
            set => transformWrapper.Position = value;
        }

        #endregion Transform Wrapper

        [SerializeField] float speed;
        [SerializeField] VectorXZ direction;

        public virtual float Speed { get => speed; set => speed = value; }
        public virtual VectorXZ Direction { get => direction; set => direction = value; }

        public override void Awake() { base.Awake(); transformWrapper = transform; }
        protected abstract void CalculatePhysics(float deltaTime);

        public override void Update() { base.Update(); CalculatePhysics(Time.deltaTime); }
    }
}