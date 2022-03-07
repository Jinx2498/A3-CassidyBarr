using UnityEngine;

namespace GameBrains.Extensions.Vectors
{
    public static class Vector3Extensions
    {
        public static (Vector3, float) DirectionToAndMagnitude(
            this Vector3 source,
            Vector3 destination)
        {
            Vector3 directionVector = source.DirectionVectorTo(destination);
            Vector3 direction = directionVector.normalized;
            var magnitude = directionVector.magnitude;
            return (direction, magnitude);
        }

        public static Vector3 DirectionVectorTo(this Vector3 source, Vector3 destination)
        {
            var directionVector = destination - source;
            return directionVector;
        }

        public static Vector3 DirectionTo(this Vector3 source, Vector3 destination)
        {
            (Vector3 direction, _) = source.DirectionToAndMagnitude(destination);
            return direction;
        }

        public static (Vector3, float) DirectionFromAndMagnitude(
            this Vector3 source,
            Vector3 destination)
        {
            (Vector3 direction, var magnitude) = source.DirectionToAndMagnitude(destination);
            return (-direction, magnitude);
        }

        public static Vector3 DirectionVectorFrom(this Vector3 source, Vector3 destination)
        {
            Vector3 directionVector = -source.DirectionVectorTo(destination);
            return directionVector;
        }

        public static Vector3 DirectionFrom(this Vector3 source, Vector3 destination)
        {
            Vector3 direction = -source.DirectionTo(destination);
            return direction;
        }

        public static Vector3 With(
            this Vector3 source,
            float? x = null,
            float? y = null,
            float? z = null)
        {
            return new Vector3(x ?? source.x, y ?? source.y, z ?? source.z);
        }

        public static Vector3 WithX(this Vector3 source, float newValue = 0)
        {
            return source.With(x: newValue);
        }

        public static Vector3 WithY(this Vector3 source, float newValue = 0)
        {
            return source.With(y: newValue);
        }

        public static Vector3 WithZ(this Vector3 source, float newValue = 0)
        {
            return source.With(z: newValue);
        }

        public static Vector3 WithZeroX(this Vector3 source)
        {
            return source.With(x: 0);
        }

        public static Vector3 WithZeroY(this Vector3 source)
        {
            return source.With(y: 0);
        }

        public static Vector3 WithZeroZ(this Vector3 source)
        {
            return source.With(z: 0);
        }

        public static float DistanceWithX(
            this Vector3 source,
            Vector3 destination ,
            float newValue = 0)
        {
            var distance
                = Vector3.Distance(source.WithX(newValue), destination.WithX(newValue));
            return distance;
        }

        public static float DistanceWithY(
            this Vector3 source,
            Vector3 destination ,
            float newValue = 0)
        {
            var distance
                = Vector3.Distance(source.WithY(newValue), destination.WithY(newValue));
            return distance;
        }

        public static float DistanceWithZ(
            this Vector3 source,
            Vector3 destination ,
            float newValue = 0)
        {
            var distance
                = Vector3.Distance(source.WithZ(newValue), destination.WithZ(newValue));
            return distance;
        }

        // axisDirection - Unit vector in direction of an axis
        // (e.g., defines a line that passes through zero).
        // point - The point for which we seek the nearest on the axis.
        public static Vector3 NearestPointOnAxis(this Vector3 axisDirection, Vector3 point, bool isNormalized = false)
        {
            if (!isNormalized) { axisDirection.Normalize(); }
            var d = Vector3.Dot(point, axisDirection);
            return axisDirection * d;
        }

        // lineDirection - Unit vector in direction of line.
        // pointOnLine - A point on the line (allowing us to define an actual line in space).
        // point - The point for which we seek the nearest on the line.
        public static Vector3 NearestPointOnLine(
            this Vector3 lineDirection, Vector3 point, Vector3 pointOnLine, bool isNormalized = false)
        {
            if (!isNormalized) { lineDirection.Normalize(); }
            var d = Vector3.Dot(point - pointOnLine, lineDirection);
            return pointOnLine + (lineDirection * d);
        }
    }
}