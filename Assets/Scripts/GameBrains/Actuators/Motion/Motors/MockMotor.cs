using GameBrains.Entities.EntityData;

namespace GameBrains.Actuators.Motion.Motors
{
    public sealed class MockMotor : Motor
    {
        public override void CalculatePhysics(KinematicData kinematicData, float deltaTime)
        {
            kinematicData.DoUpdate(deltaTime);
        }
    }
}