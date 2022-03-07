namespace GameBrains.Actuators.Motion.Movers.UsingVector3.SimpleMovers
{
    public sealed class TransformTranslateSimpleMover : SimpleMover
    {
        protected override void CalculatePhysics(float deltaTime)
        {
            if (Speed < minimumSpeed) { return; }
            
            transform.Translate(Direction * (Speed * deltaTime));
        }
    }
}