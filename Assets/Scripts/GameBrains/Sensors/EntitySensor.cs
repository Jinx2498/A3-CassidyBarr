using GameBrains.Entities;
using GameBrains.Percepts;
using UnityEngine;

namespace GameBrains.Sensors
{
    public class EntitySensor : Sensor
    {
        [SerializeField] float sensorRange = 20.0f;
        public Entity targetEntity;

        public override Percept Sense()
        {
            if (!SensorUpdateRegulator.IsReady || targetEntity == null) { return null; }
            
            var entityPercept = new EntityPercept();
            var agentPosition = Agent.transform.position;

            var targetDistance = Vector3.Distance(agentPosition, targetEntity.transform.position);

            // Are we within range?
            if (targetDistance <= sensorRange)
            {
                entityPercept.targetEntity = targetEntity;
                if (VerbosityDebug)
                {
                    Debug.Log(
                        $"New closest target entity. Position is: {entityPercept.targetEntity.transform.position}");
                }

                return entityPercept;
            }

            return null;
        }
    }
}