using GameBrains.Percepts;
using UnityEngine;

namespace GameBrains.Sensors
{
    public class PositionSensor : Sensor
    {
        [SerializeField] float sensorRange = 20.0f;
        [SerializeField] Transform targetTransform;

        public override Percept Sense()
        {
            if (!SensorUpdateRegulator.IsReady) { return null; }
            
            var positionPercept = new PositionPercept { position = null };
            var agentPosition = Agent.transform.position;
                
            if (targetTransform != null)
            {
                var targetPosition = targetTransform.position;

                if (Vector3.Distance(agentPosition, targetPosition) <= sensorRange)
                {
                    positionPercept.position = targetPosition;
                }
            }

            return positionPercept;
        }
    }
}