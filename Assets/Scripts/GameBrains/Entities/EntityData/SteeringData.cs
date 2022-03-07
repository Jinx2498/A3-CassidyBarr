using System.Collections.Generic;
using GameBrains.Actuators.Motion.Steering;
using GameBrains.Extensions.Vectors;
using UnityEngine;

namespace GameBrains.Entities.EntityData
{
    [System.Serializable]
    public class SteeringData : KinematicData
    {
        #region Accumulated data used to aggregate steering outputs

        VectorXZ accumulatedVelocity;
        public VectorXZ AccumulatedVelocity => accumulatedVelocity;

        VectorXZ accumulatedAcceleration;
        public VectorXZ AccumulatedAcceleration => accumulatedAcceleration;

        float accumulatedAngularVelocity;
        public float AccumulatedAngularVelocity => accumulatedAngularVelocity;

        float accumulatedAngularAcceleration;
        public float AccumulatedAngularAcceleration => accumulatedAngularAcceleration;

        int accumulationVelocityCount;
        int accumulationAngularVelocityCount;
        int accumulationAccelerationCount;
        int accumulationAngularAccelerationCount;

        bool doApplyAccumulatedVelocities;
        public bool DoApplyAccumulatedVelocities => doApplyAccumulatedVelocities;

        #endregion

        #region Creators

        public static SteeringData CreateSteeringDataInstance(Transform t)
        {
            SteeringData steeringData = CreateInstance<SteeringData>();
            InitializeSteeringData(t, steeringData);
            return steeringData;
        }

        protected static void InitializeSteeringData(Transform t, SteeringData steeringData)
        {
            InitializeKinematicData(t, steeringData);
            steeringData.SteeringBehaviours = new Dictionary<int, SteeringBehaviour>();
            steeringData.ResetSteeringData();
        }
        
        public void ResetSteeringData()
        {
            ResetAccumulatedData();
            doApplyAccumulatedVelocities = false;
            SteeringBehaviours.Clear();
        }

        #endregion Creators

        #region Casting

        public new SteerableAgent Owner => owner as SteerableAgent;

        public static implicit operator SteeringData(Transform t)
        {
            return CreateSteeringDataInstance(t);
        }

        #endregion Casting

        #region Steering
        
        public enum CombiningMethods
        {
            Weighted,
            Prioritized,
            Dithered
        }
        
        public CombiningMethods CombiningMethod { get; set; } = CombiningMethods.Weighted;

        #region Steering Behaviours
        
        public Dictionary<int, SteeringBehaviour> SteeringBehaviours { get; protected set; }

        public int AddSteeringBehaviour(SteeringBehaviour steeringBehaviour)
        {
            if (steeringBehaviour == null) { return -1; }

            if (!SteeringBehaviours.ContainsKey(steeringBehaviour.ID))
            {
                SteeringBehaviours.Add(steeringBehaviour.ID, steeringBehaviour);
            }
            return steeringBehaviour.ID;
        }

        public void RemoveSteeringBehaviour(SteeringBehaviour steeringBehaviour)
        {
            if (steeringBehaviour != null)
            {
                SteeringBehaviours.Remove(steeringBehaviour.ID);
            }
        }

        public void RemoveSteeringBehaviour(int id)
        {
            SteeringBehaviours.Remove(id);
        }

        public bool GetSteeringBehaviour(int id, out SteeringBehaviour steeringBehaviour)
        {
            return SteeringBehaviours.TryGetValue(id, out steeringBehaviour);
        }
        
        #endregion Steering Behaviours

        public void CalculateSteering(CombiningMethods combiningMethod = CombiningMethods.Weighted)
        {
            switch (combiningMethod)
            {
                case CombiningMethods.Weighted: CalculateWeightedSteering();
                    break;

                case CombiningMethods.Prioritized: CalculatePrioritizedSteering();
                    break;

                case CombiningMethods.Dithered: CalculateDitheredSteering();
                    break;
            }
        }

        public void CalculateWeightedSteering()
        {
            foreach (var steeringBehaviour in SteeringBehaviours.Values)
            {
                AccumulateSteeringOutput(steeringBehaviour.Steer());
            }
        }

        public void CalculatePrioritizedSteering()
        {
            // TODO for A3 (optional): Complete.
            throw new System.NotImplementedException();
        }

        public void CalculateDitheredSteering()
        {
            // TODO for A3 (optional): Complete.
            throw new System.NotImplementedException();
        }
        
        public void SetSteeringOutput(SteeringOutput steeringOutput)
        {
            switch (steeringOutput.Type)
            {
                case SteeringOutput.Types.Velocities:
                    SetAccumulatedVelocities(steeringOutput.Linear, steeringOutput.Angular);
                    break;
                case SteeringOutput.Types.Accelerations:
                    SetAccumulatedAccelerations(steeringOutput.Linear, steeringOutput.Angular);
                    break;
                default:
                    throw new System.NotImplementedException(
                        $"Steering type {steeringOutput.Type} is not implemented");
            }
        }

        public void AccumulateSteeringOutput(SteeringOutput steeringOutput)
        {
            switch (steeringOutput.Type)
            {
                case SteeringOutput.Types.Velocities:
                    AccumulateVelocities(steeringOutput.Linear, steeringOutput.Angular);
                    break;
                case SteeringOutput.Types.Accelerations:
                    AccumulateAccelerations(steeringOutput.Linear, steeringOutput.Angular);
                    break;
                default:
                    throw new System.NotImplementedException(
                        $"Steering type {steeringOutput.Type} is not implemented");
            }
        }

        #region Accumulate data
        public void SetAccumulatedVelocity(VectorXZ velocity)
        {
            doApplyAccumulatedVelocities = true;

            accumulatedVelocity = velocity;

            accumulationVelocityCount++;
        }

        public void SetAccumulatedAngularVelocity(float angularVelocity)
        {
            doApplyAccumulatedVelocities = true;

            accumulatedAngularVelocity = angularVelocity;

            accumulationAngularVelocityCount++;
        }

        public void SetAccumulatedVelocities(VectorXZ velocity, float angularVelocity)
        {
            doApplyAccumulatedVelocities = true;

            accumulatedVelocity = velocity;
            accumulatedAngularVelocity = angularVelocity;

            accumulationVelocityCount++;
            accumulationAngularVelocityCount++;
        }

        public void AccumulateVelocities(VectorXZ velocity, float angularVelocity)
        {
            doApplyAccumulatedVelocities = true;

            accumulatedVelocity = AccumulatedVelocity + velocity;
            accumulatedAngularVelocity = AccumulatedAngularVelocity + angularVelocity;

            accumulationVelocityCount++;
            accumulationAngularVelocityCount++;
        }
        public void SetAccumulatedAccelerations(VectorXZ acceleration, float angularAcceleration)
        {
            accumulatedAcceleration = acceleration;
            accumulatedAngularAcceleration = angularAcceleration;

            accumulationAccelerationCount++;
            accumulationAngularAccelerationCount++;
        }

        public void AccumulateAccelerations(VectorXZ acceleration, float angularAcceleration)
        {
            accumulatedAcceleration = AccumulatedAcceleration + acceleration;
            accumulatedAngularAcceleration = AccumulatedAngularAcceleration + angularAcceleration;

            accumulationAccelerationCount++;
            accumulationAngularAccelerationCount++;
        }

        #endregion

        #endregion Steering

        #region Do Update

        // public override void Update(float deltaTime)
        // {
        //     DoUpdate(deltaTime);
        // }
        
        // TODO: Do we need an option to use fixed update?

        public override void DoUpdate(float deltaTime, bool updatePositionAndOrientation = true)
        {
            ApplyAccumulatedVelocities();

            CalculateAcceleration();
            CalculateAngularAcceleration();

            base.DoUpdate(deltaTime, updatePositionAndOrientation);

            ResetAccumulatedData();
        }

        public void ApplyAccumulatedVelocities()
        {
            if (DoApplyAccumulatedVelocities)
            {
                if (accumulationVelocityCount > 0)
                {
                    Velocity += AccumulatedVelocity / accumulationVelocityCount;
                }

                if (accumulationAngularVelocityCount > 0)
                {
                    AngularVelocity += AccumulatedAngularVelocity / accumulationAngularVelocityCount;
                }

                doApplyAccumulatedVelocities = false;
            }
        }

        public void CalculateAcceleration()
        {
            if (accumulationAccelerationCount > 0)
            {
                Acceleration += AccumulatedAcceleration / accumulationAccelerationCount;
            }
        }

        public void CalculateAngularAcceleration()
        {
            if (accumulationAngularAccelerationCount > 0)
            {
                AngularAcceleration += AccumulatedAngularAcceleration / accumulationAngularAccelerationCount;
            }
        }

        public void ResetAccumulatedData()
        {
            accumulatedVelocity = VectorXZ.zero;
            accumulatedAngularVelocity = 0;
            accumulatedAcceleration = VectorXZ.zero;
            accumulatedAngularAcceleration = 0;

            accumulationVelocityCount = 0;
            accumulationAngularVelocityCount = 0;
            accumulationAccelerationCount = 0;
            accumulationAngularAccelerationCount = 0;
        }

        #endregion Do Update
    }
}