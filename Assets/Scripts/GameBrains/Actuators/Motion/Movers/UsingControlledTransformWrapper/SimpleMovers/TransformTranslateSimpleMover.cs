namespace GameBrains.Actuators.Motion.Movers.UsingControlledTransformWrapper.SimpleMovers
{
    public sealed class TransformTranslateSimpleMover : SimpleMover
    {
        protected override void CalculatePhysics(float deltaTime)
        {
            // Type cast from VectorXZ to Vector3 sets Y to 0. Good.
            //transform.Translate((Vector3)Direction * (Speed * deltaTime));

            Location += Direction * (Speed * deltaTime);
        }
    }
}