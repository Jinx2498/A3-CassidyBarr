using GameBrains.Extensions.Vectors;
using UnityEngine;

namespace GameBrains.Extensions.MathExtensions
{
    public static partial class Math
    {
        // WrapAngle returns angle in range (-180,180]
        public static float WrapAngle(float angle)
        {
            while (angle <= -180f) { angle += 360f; }
            while (angle > 180f) { angle += -360f; }
            return angle;
        }
        
        // Vector angles in range (-180,180]
        public static VectorXZ WrapAngles(Vector2 v)
        {
            return new VectorXZ(WrapAngle(v.x), WrapAngle(v.y));
        }

        // Vector angles in range (-180,180]
        public static Vector3 WrapAngles(Vector3 v)
        {
            return new Vector3(WrapAngle(v.x), WrapAngle(v.y), WrapAngle(v.z));
        }

        public static float ClampAngle(float angle, float min, float max)
        {
            return Mathf.Clamp(WrapAngle(angle), min, max);
        }
    
    }
}