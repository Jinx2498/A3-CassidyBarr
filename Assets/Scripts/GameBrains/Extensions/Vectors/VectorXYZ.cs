using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Internal;

namespace GameBrains.Extensions.Vectors
{
  /// <summary>
  ///     <para>Representation of 3D directions and points.</para>
  /// </summary>
  [Serializable]
    public struct VectorXYZ : IEquatable<VectorXYZ>, IFormattable
    {
        public const float kEpsilon = 1E-05f;
        public const float kEpsilonNormalSqrt = 1E-15f;

        /// <summary>
        ///     <para>X component of the VectorXYZ.</para>
        /// </summary>
        public float x;

        /// <summary>
        ///     <para>Y component of the VectorXYZ.</para>
        /// </summary>
        public float y;

        /// <summary>
        ///     <para>Z component of the VectorXYZ.</para>
        /// </summary>
        public float z;

        /// <summary>
        ///     <para>Creates a new VectorXYZ with given x, y, z components.</para>
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public VectorXYZ(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        ///     <para>Creates a new VectorXYZ with given x, y components and sets z to zero.</para>
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public VectorXYZ(float x, float y)
        {
            this.x = x;
            this.y = y;
            z = 0.0f;
        }

        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return x;
                    case 1:
                        return y;
                    case 2:
                        return z;
                    default:
                        throw new IndexOutOfRangeException("Invalid VectorXYZ index!");
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        x = value;
                        break;
                    case 1:
                        y = value;
                        break;
                    case 2:
                        z = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException("Invalid VectorXYZ index!");
                }
            }
        }

        /// <summary>
        ///     <para>Returns this VectorXYZ with a magnitude of 1 (Read Only).</para>
        /// </summary>
        public VectorXYZ normalized => Normalize(this);

        /// <summary>
        ///     <para>Returns the length of this VectorXYZ (Read Only).</para>
        /// </summary>
        public float magnitude => (float) System.Math.Sqrt(x * (double) x +
                                                    y * (double) y +
                                                    z * (double) z);

        /// <summary>
        ///     <para>Returns the squared length of this VectorXYZ (Read Only).</para>
        /// </summary>
        public float sqrMagnitude => (float) (x * (double) x +
                                              y * (double) y +
                                              z * (double) z);

        /// <summary>
        ///     <para>Shorthand for writing VectorXYZ(0, 0, 0).</para>
        /// </summary>
        public static VectorXYZ zero { get; } = new VectorXYZ(0.0f, 0.0f, 0.0f);

        /// <summary>
        ///     <para>Shorthand for writing VectorXYZ(1, 1, 1).</para>
        /// </summary>
        public static VectorXYZ one { get; } = new VectorXYZ(1f, 1f, 1f);

        /// <summary>
        ///     <para>Shorthand for writing VectorXYZ(0, 0, 1).</para>
        /// </summary>
        public static VectorXYZ forward { get; } = new VectorXYZ(0.0f, 0.0f, 1f);

        /// <summary>
        ///     <para>Shorthand for writing VectorXYZ(0, 0, -1).</para>
        /// </summary>
        public static VectorXYZ back { get; } = new VectorXYZ(0.0f, 0.0f, -1f);

        /// <summary>
        ///     <para>Shorthand for writing VectorXYZ(0, 1, 0).</para>
        /// </summary>
        public static VectorXYZ up { get; } = new VectorXYZ(0.0f, 1f, 0.0f);

        /// <summary>
        ///     <para>Shorthand for writing VectorXYZ(0, -1, 0).</para>
        /// </summary>
        public static VectorXYZ down { get; } = new VectorXYZ(0.0f, -1f, 0.0f);

        /// <summary>
        ///     <para>Shorthand for writing VectorXYZ(-1, 0, 0).</para>
        /// </summary>
        public static VectorXYZ left { get; } = new VectorXYZ(-1f, 0.0f, 0.0f);

        /// <summary>
        ///     <para>Shorthand for writing VectorXYZ(1, 0, 0).</para>
        /// </summary>
        public static VectorXYZ right { get; } = new VectorXYZ(1f, 0.0f, 0.0f);

        /// <summary>
        ///     <para>
        ///         Shorthand for writing VectorXYZ(float.PositiveInfinity, float.PositiveInfinity,
        ///         float.PositiveInfinity).
        ///     </para>
        /// </summary>
        public static VectorXYZ positiveInfinity { get; } = new VectorXYZ(float.PositiveInfinity,
            float.PositiveInfinity, float.PositiveInfinity);

        /// <summary>
        ///     <para>
        ///         Shorthand for writing VectorXYZ(float.NegativeInfinity, float.NegativeInfinity,
        ///         float.NegativeInfinity).
        ///     </para>
        /// </summary>
        public static VectorXYZ negativeInfinity { get; } = new VectorXYZ(float.NegativeInfinity,
            float.NegativeInfinity, float.NegativeInfinity);

        [Obsolete("Use VectorXYZ.forward instead.")]
        public static VectorXYZ fwd => new VectorXYZ(0.0f, 0.0f, 1f);

        public bool Equals(VectorXYZ other)
        {
            return x == (double) other.x &&
                   y == (double) other.y &&
                   z == (double) other.z;
        }

        /// <summary>
        ///     <para>Returns a formatted string for this VectorXYZ.</para>
        /// </summary>
        /// <param name="format">A numeric format string.</param>
        /// <param name="formatProvider">An object that specifies culture-specific formatting.</param>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format))
                format = "F1";
            return string.Format("({0}, {1}, {2})", x.ToString(format, formatProvider),
                y.ToString(format, formatProvider), z.ToString(format, formatProvider));
        }

        /// <summary>
        ///     <para>Spherically interpolates between two VectorXYZ.</para>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        public static VectorXYZ Slerp(VectorXYZ a, VectorXYZ b, float t)
        {
            return Vector3.Slerp(a, b, t);
        }

        /// <summary>
        ///     <para>Spherically interpolates between two VectorXYZ.</para>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        public static VectorXYZ SlerpUnclamped(VectorXYZ a, VectorXYZ b, float t)
        {
            return Vector3.SlerpUnclamped(a, b, t);
        }

        public static void OrthoNormalize(ref VectorXYZ normal, ref VectorXYZ tangent)
        {
            Vector3 vNormal = normal;
            Vector3 vTangent = tangent;
            Vector3.OrthoNormalize(ref vNormal, ref vTangent);
            normal = vNormal;
            tangent = vTangent;
        }

        public static void OrthoNormalize(
            ref VectorXYZ normal,
            ref VectorXYZ tangent,
            ref VectorXYZ binormal)
        {
            Vector3 vNormal = normal;
            Vector3 vTangent = tangent;
            Vector3 vBinormal = binormal;
            Vector3.OrthoNormalize(ref vNormal, ref vTangent, ref vBinormal);
            normal = vNormal;
            tangent = vTangent;
            binormal = vBinormal;
        }

        /// <summary>
        ///     <para>Rotates a VectorXYZ current towards target.</para>
        /// </summary>
        /// <param name="current">The VectorXYZ being managed.</param>
        /// <param name="target">The VectorXYZ.</param>
        /// <param name="maxRadiansDelta">The maximum angle in radians allowed for this rotation.</param>
        /// <param name="maxMagnitudeDelta">The maximum allowed change in position magnitude for this rotation.</param>
        /// <returns>
        ///     <para>The VectorXYZ that RotateTowards generates.</para>
        /// </returns>
        public static VectorXYZ RotateTowards(
            VectorXYZ current,
            VectorXYZ target,
            float maxRadiansDelta,
            float maxMagnitudeDelta)
        {
            return Vector3.RotateTowards(current, target, maxRadiansDelta, maxMagnitudeDelta);
        }

        /// <summary>
        ///     <para>Linearly interpolates between two points.</para>
        /// </summary>
        /// <param name="a">Start value, returned when t = 0.</param>
        /// <param name="b">End value, returned when t = 1.</param>
        /// <param name="t">Value used to interpolate between a and b.</param>
        /// <returns>
        ///     <para>Interpolated value, equals to a + (b - a) * t.</para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXYZ Lerp(VectorXYZ a, VectorXYZ b, float t)
        {
            t = Mathf.Clamp01(t);
            return new VectorXYZ(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t,
                a.z + (b.z - a.z) * t);
        }

        /// <summary>
        ///     <para>Linearly interpolates between two VectorXYZs.</para>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXYZ LerpUnclamped(VectorXYZ a, VectorXYZ b, float t)
        {
            return new VectorXYZ(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t,
                a.z + (b.z - a.z) * t);
        }

        /// <summary>
        ///     <para>
        ///         Calculate a position between the points specified by current and target, moving no
        ///         farther than the distance specified by maxDistanceDelta.
        ///     </para>
        /// </summary>
        /// <param name="current">The position to move from.</param>
        /// <param name="target">The position to move towards.</param>
        /// <param name="maxDistanceDelta">Distance to move current per call.</param>
        /// <returns>
        ///     <para>The new position.</para>
        /// </returns>
        public static VectorXYZ MoveTowards(
            VectorXYZ current,
            VectorXYZ target,
            float maxDistanceDelta)
        {
            var num1 = target.x - current.x;
            var num2 = target.y - current.y;
            var num3 = target.z - current.z;
            var num4 = (float) (num1 * (double) num1 +
                                num2 * (double) num2 +
                                num3 * (double) num3);
            if (num4 == 0.0 ||
                maxDistanceDelta >= 0.0 && num4 <= maxDistanceDelta * (double) maxDistanceDelta)
                return target;
            var num5 = (float) System.Math.Sqrt(num4);
            return new VectorXYZ(current.x + num1 / num5 * maxDistanceDelta,
                current.y + num2 / num5 * maxDistanceDelta,
                current.z + num3 / num5 * maxDistanceDelta);
        }

        [ExcludeFromDocs]
        public static VectorXYZ SmoothDamp(
            VectorXYZ current,
            VectorXYZ target,
            ref VectorXYZ currentVelocity,
            float smoothTime,
            float maxSpeed)
        {
            var deltaTime = Time.deltaTime;
            return SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed,
                deltaTime);
        }

        [ExcludeFromDocs]
        public static VectorXYZ SmoothDamp(
            VectorXYZ current,
            VectorXYZ target,
            ref VectorXYZ currentVelocity,
            float smoothTime)
        {
            var deltaTime = Time.deltaTime;
            var maxSpeed = float.PositiveInfinity;
            return SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed,
                deltaTime);
        }

        public static VectorXYZ SmoothDamp(
            VectorXYZ current,
            VectorXYZ target,
            ref VectorXYZ currentVelocity,
            float smoothTime,
            [DefaultValue("Mathf.Infinity")] float maxSpeed,
            [DefaultValue("Time.deltaTime")] float deltaTime)
        {
            smoothTime = Mathf.Max(0.0001f, smoothTime);
            var num1 = 2f / smoothTime;
            var num2 = num1 * deltaTime;
            var num3 = (float) (1.0 /
                                (1.0 +
                                 num2 +
                                 0.47999998927116394 * num2 * num2 +
                                 0.23499999940395355 * num2 * num2 * num2));
            var num4 = current.x - target.x;
            var num5 = current.y - target.y;
            var num6 = current.z - target.z;
            VectorXYZ vectorXYZ = target;
            var num7 = maxSpeed * smoothTime;
            var num8 = num7 * num7;
            var num9 = (float) (num4 * (double) num4 +
                                num5 * (double) num5 +
                                num6 * (double) num6);
            if (num9 > (double) num8)
            {
                var num10 = (float) System.Math.Sqrt(num9);
                num4 = num4 / num10 * num7;
                num5 = num5 / num10 * num7;
                num6 = num6 / num10 * num7;
            }

            target.x = current.x - num4;
            target.y = current.y - num5;
            target.z = current.z - num6;
            var num11 = (currentVelocity.x + num1 * num4) * deltaTime;
            var num12 = (currentVelocity.y + num1 * num5) * deltaTime;
            var num13 = (currentVelocity.z + num1 * num6) * deltaTime;
            currentVelocity.x = (currentVelocity.x - num1 * num11) * num3;
            currentVelocity.y = (currentVelocity.y - num1 * num12) * num3;
            currentVelocity.z = (currentVelocity.z - num1 * num13) * num3;
            var x = target.x + (num4 + num11) * num3;
            var y = target.y + (num5 + num12) * num3;
            var z = target.z + (num6 + num13) * num3;
            var num14 = vectorXYZ.x - current.x;
            var num15 = vectorXYZ.y - current.y;
            var num16 = vectorXYZ.z - current.z;
            var num17 = x - vectorXYZ.x;
            var num18 = y - vectorXYZ.y;
            var num19 = z - vectorXYZ.z;
            if (num14 * (double) num17 + num15 * (double) num18 + num16 * (double) num19 > 0.0)
            {
                x = vectorXYZ.x;
                y = vectorXYZ.y;
                z = vectorXYZ.z;
                currentVelocity.x = (x - vectorXYZ.x) / deltaTime;
                currentVelocity.y = (y - vectorXYZ.y) / deltaTime;
                currentVelocity.z = (z - vectorXYZ.z) / deltaTime;
            }

            return new VectorXYZ(x, y, z);
        }

        /// <summary>
        ///     <para>Set x, y and z components of an existing VectorXYZ.</para>
        /// </summary>
        /// <param name="newX"></param>
        /// <param name="newY"></param>
        /// <param name="newZ"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Set(float newX, float newY, float newZ)
        {
            x = newX;
            y = newY;
            z = newZ;
        }

        /// <summary>
        ///     <para>Multiplies two VectorXYZs component-wise.</para>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXYZ Scale(VectorXYZ a, VectorXYZ b)
        {
            return new VectorXYZ(a.x * b.x, a.y * b.y, a.z * b.z);
        }

        /// <summary>
        ///     <para>Multiplies every component of this VectorXYZ by the same component of scale.</para>
        /// </summary>
        /// <param name="scale"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Scale(VectorXYZ scale)
        {
            x *= scale.x;
            y *= scale.y;
            z *= scale.z;
        }

        /// <summary>
        ///     <para>Cross Product of two VectorXYZ.</para>
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        public static VectorXYZ Cross(VectorXYZ lhs, VectorXYZ rhs)
        {
            return new VectorXYZ(
                (float) (lhs.y * (double) rhs.z - lhs.z * (double) rhs.y),
                (float) (lhs.z * (double) rhs.x - lhs.x * (double) rhs.z),
                (float) (lhs.x * (double) rhs.y - lhs.y * (double) rhs.x));
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ (y.GetHashCode() << 2) ^ (z.GetHashCode() >> 2);
        }

        /// <summary>
        ///     <para>Returns true if the given VectorXYZ is exactly equal to this VectorXYZ.</para>
        /// </summary>
        /// <param name="other"></param>
        public override bool Equals(object other)
        {
            return other is VectorXYZ other1 && Equals(other1);
        }

        /// <summary>
        ///     <para>Reflects a VectorXYZ off the plane defined by a normal.</para>
        /// </summary>
        /// <param name="inDirection"></param>
        /// <param name="inNormal"></param>
        public static VectorXYZ Reflect(VectorXYZ inDirection, VectorXYZ inNormal)
        {
            var num = -2f * Dot(inNormal, inDirection);
            return new VectorXYZ(num * inNormal.x + inDirection.x, num * inNormal.y + inDirection.y,
                num * inNormal.z + inDirection.z);
        }

        /// <summary>
        ///     <para>Makes this VectorXYZ have a magnitude of 1.</para>
        /// </summary>
        /// <param name="value"></param>
        public static VectorXYZ Normalize(VectorXYZ value)
        {
            var num = Magnitude(value);
            return num > 9.999999747378752E-06 ? value / num : zero;
        }

        public void Normalize()
        {
            var num = Magnitude(this);
            if (num > 9.999999747378752E-06)
                this = this / num;
            else
                this = zero;
        }

        /// <summary>
        ///     <para>Dot Product of two VectorXYZ.</para>
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Dot(VectorXYZ lhs, VectorXYZ rhs)
        {
            return (float) (lhs.x * (double) rhs.x +
                            lhs.y * (double) rhs.y +
                            lhs.z * (double) rhs.z);
        }

        /// <summary>
        ///     <para>Projects a VectorXYZ onto another position.</para>
        /// </summary>
        /// <param name="vectorXYZ"></param>
        /// <param name="onNormal"></param>
        public static VectorXYZ Project(VectorXYZ vectorXYZ, VectorXYZ onNormal)
        {
            var num1 = Dot(onNormal, onNormal);
            if (num1 < (double) Mathf.Epsilon)
                return zero;
            var num2 = Dot(vectorXYZ, onNormal);
            return new VectorXYZ(onNormal.x * num2 / num1, onNormal.y * num2 / num1,
                onNormal.z * num2 / num1);
        }

        /// <summary>
        ///     <para>Projects a VectorXYZ onto a plane defined by a normal orthogonal to the plane.</para>
        /// </summary>
        /// <param name="planeNormal">The direction from the VectorXYZ towards the plane.</param>
        /// <param name="vectorXYZ">The location of the VectorXYZ above the plane.</param>
        /// <returns>
        ///     <para>The location of the VectorXYZ on the plane.</para>
        /// </returns>
        public static VectorXYZ ProjectOnPlane(VectorXYZ vectorXYZ, VectorXYZ planeNormal)
        {
            var num1 = Dot(planeNormal, planeNormal);
            if (num1 < (double) Mathf.Epsilon)
                return vectorXYZ;
            var num2 = Dot(vectorXYZ, planeNormal);
            return new VectorXYZ(vectorXYZ.x - planeNormal.x * num2 / num1,
                vectorXYZ.y - planeNormal.y * num2 / num1,
                vectorXYZ.z - planeNormal.z * num2 / num1);
        }

        /// <summary>
        ///     <para>Returns the angle in degrees between from and to.</para>
        /// </summary>
        /// <param name="from">The VectorXYZ from which the angular difference is measured.</param>
        /// <param name="to">The VectorXYZ to which the angular difference is measured.</param>
        /// <returns>
        ///     <para>The angle in degrees between the two VectorXYZs.</para>
        /// </returns>
        public static float Angle(VectorXYZ from, VectorXYZ to)
        {
            var num = (float) System.Math.Sqrt(from.sqrMagnitude * (double) to.sqrMagnitude);
            return num < 1.0000000036274937E-15
                ? 0.0f
                : (float) System.Math.Acos(Mathf.Clamp(Dot(from, to) / num, -1f, 1f)) * 57.29578f;
        }

        /// <summary>
        ///     <para>Returns the signed angle in degrees between from and to.</para>
        /// </summary>
        /// <param name="from">The VectorXYZ from which the angular difference is measured.</param>
        /// <param name="to">The VectorXYZ to which the angular difference is measured.</param>
        /// <param name="axis">A VectorXYZ around which the other VectorXYZs are rotated.</param>
        public static float SignedAngle(VectorXYZ from, VectorXYZ to, VectorXYZ axis)
        {
            var num1 = Angle(from, to);
            var num2 = (float) (from.y * (double) to.z - from.z * (double) to.y);
            var num3 = (float) (from.z * (double) to.x - from.x * (double) to.z);
            var num4 = (float) (from.x * (double) to.y - from.y * (double) to.x);
            var num5 = Mathf.Sign((float) (axis.x * (double) num2 +
                                           axis.y * (double) num3 +
                                           axis.z * (double) num4));
            return num1 * num5;
        }

        /// <summary>
        ///     <para>Returns the distance between a and b.</para>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static float Distance(VectorXYZ a, VectorXYZ b)
        {
            var num1 = a.x - b.x;
            var num2 = a.y - b.y;
            var num3 = a.z - b.z;
            return (float) System.Math.Sqrt(num1 * (double) num1 +
                                            num2 * (double) num2 +
                                            num3 * (double) num3);
        }

        /// <summary>
        ///     <para>Returns a copy of VectorXYZ with its magnitude clamped to maxLength.</para>
        /// </summary>
        /// <param name="vectorXYZ"></param>
        /// <param name="maxLength"></param>
        public static VectorXYZ ClampMagnitude(VectorXYZ vectorXYZ, float maxLength)
        {
            var sqrMagnitude = vectorXYZ.sqrMagnitude;
            if (sqrMagnitude <= maxLength * (double) maxLength)
                return vectorXYZ;
            var num1 = (float) System.Math.Sqrt(sqrMagnitude);
            var num2 = vectorXYZ.x / num1;
            var num3 = vectorXYZ.y / num1;
            var num4 = vectorXYZ.z / num1;
            return new VectorXYZ(num2 * maxLength, num3 * maxLength, num4 * maxLength);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Magnitude(VectorXYZ vectorXYZ)
        {
            return (float) System.Math.Sqrt(vectorXYZ.x * (double) vectorXYZ.x +
                                            vectorXYZ.y * (double) vectorXYZ.y +
                                            vectorXYZ.z * (double) vectorXYZ.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SqrMagnitude(VectorXYZ vectorXYZ)
        {
            return (float) (vectorXYZ.x * (double) vectorXYZ.x +
                            vectorXYZ.y * (double) vectorXYZ.y +
                            vectorXYZ.z * (double) vectorXYZ.z);
        }

        /// <summary>
        ///     <para>Returns a VectorXYZ that is made from the smallest components of two VectorXYZs.</para>
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXYZ Min(VectorXYZ lhs, VectorXYZ rhs)
        {
            return new VectorXYZ(Mathf.Min(lhs.x, rhs.x), Mathf.Min(lhs.y, rhs.y),
                Mathf.Min(lhs.z, rhs.z));
        }

        /// <summary>
        ///     <para>Returns a VectorXYZ that is made from the largest components of two VectorXYZs.</para>
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXYZ Max(VectorXYZ lhs, VectorXYZ rhs)
        {
            return new VectorXYZ(Mathf.Max(lhs.x, rhs.x), Mathf.Max(lhs.y, rhs.y),
                Mathf.Max(lhs.z, rhs.z));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXYZ operator +(VectorXYZ a, VectorXYZ b)
        {
            return new VectorXYZ(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXYZ operator -(VectorXYZ a, VectorXYZ b)
        {
            return new VectorXYZ(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXYZ operator -(VectorXYZ a) { return new VectorXYZ(-a.x, -a.y, -a.z); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXYZ operator *(VectorXYZ a, float d)
        {
            return new VectorXYZ(a.x * d, a.y * d, a.z * d);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXYZ operator *(float d, VectorXYZ a)
        {
            return new VectorXYZ(a.x * d, a.y * d, a.z * d);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXYZ operator /(VectorXYZ a, float d)
        {
            return new VectorXYZ(a.x / d, a.y / d, a.z / d);
        }

        public static bool operator ==(VectorXYZ lhs, VectorXYZ rhs)
        {
            var num1 = lhs.x - rhs.x;
            var num2 = lhs.y - rhs.y;
            var num3 = lhs.z - rhs.z;
            return num1 * (double) num1 + num2 * (double) num2 + num3 * (double) num3 <
                   9.999999439624929E-11;
        }

        public static bool operator !=(VectorXYZ lhs, VectorXYZ rhs) { return !(lhs == rhs); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator VectorXYZ(Vector3 v3)
        {
            return new VectorXYZ(v3.x, v3.y, v3.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Vector3(VectorXYZ vXYZ) { return new Vector3(vXYZ.x, vXYZ.y, vXYZ.z); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator VectorXYZ(Vector2 v2)
        {
            return new VectorXYZ(v2.x, 0.0f, v2.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Vector2(VectorXYZ vXYZ) { return new Vector2(vXYZ.x, vXYZ.z); }

        /// <summary>
        ///     <para>Returns a formatted string for this VectorXYZ.</para>
        /// </summary>
        /// <param name="format">A numeric format string.</param>
        /// <param name="formatProvider">An object that specifies culture-specific formatting.</param>
        public override string ToString()
        {
            return ToString(null, CultureInfo.InvariantCulture.NumberFormat);
        }

        /// <summary>
        ///     <para>Returns a formatted string for this VectorXYZ.</para>
        /// </summary>
        /// <param name="format">A numeric format string.</param>
        /// <param name="formatProvider">An object that specifies culture-specific formatting.</param>
        public string ToString(string format)
        {
            return ToString(format, CultureInfo.InvariantCulture.NumberFormat);
        }

        [Obsolete(
            "Use Position.Angle instead. AngleBetween uses radians instead of degrees and was deprecated for this reason")]
        public static float AngleBetween(VectorXYZ from, VectorXYZ to)
        {
            return (float) System.Math.Acos(Mathf.Clamp(Dot(from.normalized, to.normalized), -1f, 1f));
        }

        [Obsolete("Use Position.ProjectOnPlane instead.")]
        public static VectorXYZ Exclude(VectorXYZ excludeThis, VectorXYZ fromThat)
        {
            return ProjectOnPlane(fromThat, excludeThis);
        }
    }
}