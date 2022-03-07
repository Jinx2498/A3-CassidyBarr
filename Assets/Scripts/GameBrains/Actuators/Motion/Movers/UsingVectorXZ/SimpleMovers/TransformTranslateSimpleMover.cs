using UnityEngine;

namespace GameBrains.Actuators.Motion.Movers.UsingVectorXZ.SimpleMovers
{
    public sealed class TransformTranslateSimpleMover : SimpleMover
    {
        protected override void CalculatePhysics(float deltaTime)
        {
            if (Speed < minimumSpeed) { return; }
            
            // Type cast from VectorXZ to Vector3 sets Y to 0. Good.
            transform.Translate((Vector3)Direction * (Speed * deltaTime));
        }
    }
}