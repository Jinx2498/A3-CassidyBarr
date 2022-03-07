using GameBrains.Extensions.Vectors;

namespace GameBrains.Actuators.Motion.Steering
{
    public sealed class SteeringOutput
    {
        public enum Types { Velocities, Accelerations, Forces }

        public Types Type { get; set; }

        public VectorXZ Linear { get; set; }

        public float Angular { get; set; }
    }
}