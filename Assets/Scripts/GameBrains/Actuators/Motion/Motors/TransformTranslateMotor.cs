using GameBrains.Entities.EntityData;

namespace GameBrains.Actuators.Motion.Motors
{
    public sealed class TransformTranslateMotor : Motor
    {
        public override void CalculatePhysics(KinematicData kinematicData, float deltaTime)
        {
            kinematicData.DoUpdate(deltaTime);
        }
    }
}