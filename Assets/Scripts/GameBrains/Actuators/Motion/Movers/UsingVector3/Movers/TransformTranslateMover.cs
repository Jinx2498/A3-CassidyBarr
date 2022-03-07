using UnityEngine;

namespace GameBrains.Actuators.Motion.Movers.UsingVector3.Movers
{
    public sealed class TransformTranslateMover : Mover
    {
        protected override void CalculatePhysics(float deltaTime)
        {
            // Use average of Vinitial and Vfinal
            // deltaP = (Vinital + Vfinal) / 2 * t
            // Vfinal = Vinitial + A * t
            // deltaP = (Vinitial + Vinitial + A * t) / 2 * t
            // deltaP = (2 * Vinitial + A * t) / 2 * t
            // deltaP = Vinitial * t + A * t * t / 2
            float halfDeltaTimeSquared = (deltaTime * deltaTime) / 2;
            Vector3 positionOffset = (Velocity * deltaTime) + (Acceleration * halfDeltaTimeSquared);
            Velocity += Acceleration * deltaTime;
            transform.Translate(positionOffset);
        }
    }
}