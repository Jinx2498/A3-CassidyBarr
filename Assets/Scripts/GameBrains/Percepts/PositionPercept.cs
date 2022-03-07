using UnityEngine;

namespace GameBrains.Percepts
{
    public class PositionPercept : Percept
    {
        // Maybe null (e.g., Sensor failed to detect / out of range).
        public Vector3? position;
    }
}