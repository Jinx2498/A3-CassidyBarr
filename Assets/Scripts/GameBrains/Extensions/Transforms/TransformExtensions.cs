using GameBrains.Extensions.Vectors;
using UnityEngine;

namespace GameBrains.Extensions.Transforms
{
    public static class TransformExtensions
    {
        public static (Vector3, float) DirectionToAndMagnitude(
            this Transform sourceTransform,
            Transform destinationTransform)
        {
            (Vector3 direction, var magnitude)
                = sourceTransform.position.DirectionToAndMagnitude(destinationTransform.position);
            return (direction, magnitude);
        }

        public static Vector3 DirectionVectorTo(this Transform sourceTransform, Transform destinationTransform)
        {
            var directionVector
                = sourceTransform.position.DirectionVectorFrom(destinationTransform.position);
            return directionVector;
        }

        public static Vector3 DirectionTo(
            this Transform sourceTransform,
            Transform destinationTransform)
        {
            Vector3 direction
                = sourceTransform.position.DirectionTo(destinationTransform.position);
            return direction;
        }

        public static (Vector3, float) DirectionFromAndMagnitude(
            this Transform sourceTransform,
            Transform destinationTransform)
        {
            (Vector3 direction, var magnitude)
                = sourceTransform.DirectionToAndMagnitude(destinationTransform);
            return (-direction, magnitude);
        }

        public static Vector3 DirectionVectorFrom(
            this Transform sourceTransform,
            Transform destinationTransform)
        {
            Vector3 directionVector = -sourceTransform.DirectionVectorTo(destinationTransform);
            return directionVector;
        }

        public static Vector3 DirectionFrom(
            this Transform sourceTransform,
            Transform destinationTransform)
        {
            Vector3 direction = -sourceTransform.DirectionTo(destinationTransform);
            return direction;
        }

        public static void DestroyChildren(this Transform transform)
        {
            foreach (Transform child in transform) { Object.Destroy(child.gameObject); }
        }
    }
}