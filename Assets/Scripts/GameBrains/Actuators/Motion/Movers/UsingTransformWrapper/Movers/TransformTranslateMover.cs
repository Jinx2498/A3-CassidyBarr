namespace GameBrains.Actuators.Motion.Movers.UsingTransformWrapper.Movers
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
            //VectorXZ positionOffset = (Velocity * deltaTime) + (Acceleration * halfDeltaTimeSquared);
            Location += (Velocity * deltaTime) + (Acceleration * halfDeltaTimeSquared);
            Velocity += Acceleration * deltaTime;
            // Type cast from VectorXZ to Vector3 sets Y to 0. Good.
            // transform.Translate((Vector3)positionOffset);

            //Location += positionOffset;
        }
    }
}