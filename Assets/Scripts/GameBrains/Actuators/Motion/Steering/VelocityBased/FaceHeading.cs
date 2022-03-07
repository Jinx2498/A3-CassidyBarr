using GameBrains.Entities.EntityData;

namespace GameBrains.Actuators.Motion.Steering.VelocityBased
{
    [System.Serializable]
    public class FaceHeading : Face
    {
        #region Creators

        public new static FaceHeading CreateInstance(SteeringData steeringData)
        {
            var steeringBehaviour = CreateInstance<FaceHeading>(steeringData);
            Initialize(steeringBehaviour);
            return steeringBehaviour;
        }

        protected static void Initialize(FaceHeading steeringBehaviour)
        {
            Face.Initialize(steeringBehaviour);
            steeringBehaviour.NoStop = false;
            steeringBehaviour.NoSlow = true;
            steeringBehaviour.NeverCompletes = true;
        }

        #endregion Creators

        #region Members and Properties
        public virtual bool FaceHeadingActive { get; protected set; } = true;

        #endregion Members and Properties

        #region Steering

        public override SteeringOutput Steer()
        {
            // TODO for A3 (optional): Replace

            // no effect
            return new SteeringOutput { Type = SteeringOutput.Types.Velocities };
        }

        #endregion Steering
    }
}