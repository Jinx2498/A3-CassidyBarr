using GameBrains.Extensions.Vectors;
using UnityEngine;

namespace GameBrains.Extensions.MathExtensions
{
	public static partial class Math
	{
		public static float LimitMagnitude(float value, float maximumMagnitude)
		{
			return Mathf.Clamp(value, -maximumMagnitude, maximumMagnitude);
		}

		// limit the magnitude of the vector to maximumMagnitude
		public static Vector2 LimitMagnitude(Vector2 vector, float maximumMagnitude)
		{
			float magnitude = vector.magnitude;

			if (magnitude > maximumMagnitude)
			{
				// vector.normalized * maximumMagnitude
				vector *= maximumMagnitude / magnitude;
			}

			return vector;
		}
		
		public static VectorXZ LimitMagnitude(VectorXZ vector, float maximumMagnitude)
		{
			float magnitude = vector.magnitude;

			if (magnitude > maximumMagnitude)
			{
				// vector.normalized * maximumMagnitude
				vector *= maximumMagnitude / magnitude;
			}

			return vector;
		}

		// limit the magnitude of the vector to maximumMagnitude (includes y component)
		public static Vector3 LimitMagnitude(Vector3 vector, float maximumMagnitude)
		{
			float magnitude = vector.magnitude;
			
			if (magnitude > maximumMagnitude)
			{
				// vector.normalized * maximumMagnitude
				vector *= maximumMagnitude / magnitude;
			}
			
			return vector;
		}
		
		// limit the magnitude of the vector to maximumMagnitude (includes y component)
		public static VectorXYZ LimitMagnitude(VectorXYZ vector, float maximumMagnitude)
		{
			float magnitude = vector.magnitude;
			
			if (magnitude > maximumMagnitude)
			{
				// vector.normalized * maximumMagnitude
				vector *= maximumMagnitude / magnitude;
			}
			
			return vector;
		}

		// convert orientation to heading vector
		public static Vector2 DegreeAngleToVector2(float degree)
		{
			float radian = degree * Mathf.Deg2Rad;
			return new Vector2(Mathf.Sin(radian), Mathf.Cos(radian));
		}
		
		// convert heading vector to orientation (z-axis relative)
		public static float Vector2ToDegreeAngle(Vector2 heading)
		{
			float orientation
				= WrapAngle(90 + Mathf.Atan2(-heading.y, heading.x) * Mathf.Rad2Deg);
			return orientation;
		}
		
		public static VectorXZ DegreeAngleToVectorXZ(float degree)
		{
			float radian = degree * Mathf.Deg2Rad;
			return new VectorXZ(Mathf.Sin(radian), Mathf.Cos(radian));
		}
		
		// convert heading vector to orientation (z-axis relative)
		public static float VectorXZToDegreeAngle(VectorXZ headingXZ)
		{
			Vector2 heading = headingXZ;
			float orientation
				= WrapAngle(90 + Mathf.Atan2(-heading.y, heading.x) * Mathf.Rad2Deg);
			return orientation;
		}
	}
}