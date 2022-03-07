namespace GameBrains.Actuators.Motion.Movers.UsingVector2.SimpleMovers
{
    public sealed class TransformTranslateSimpleMover : SimpleMover
    {
        protected override void CalculatePhysics(float deltaTime)
        {
            if (Speed < minimumSpeed) { return; }

            // XY -> XYZ! Not what we want
            transform.Translate(Direction * (Speed * deltaTime));

            // This works but is inelegant. Need a VectorXZ type.
            // var directionXYZ = new Vector3(Direction.x, 0, Direction.y);
            // transform.Translate(directionXYZ * (Speed * deltaTime));
        }
    }
}