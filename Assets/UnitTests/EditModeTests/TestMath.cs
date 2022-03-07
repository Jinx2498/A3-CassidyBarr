using GameBrains.Extensions.MathExtensions;
using GameBrains.Extensions.Vectors;
using NUnit.Framework;
using UnityEngine;

namespace UnitTests.EditModeTests
{
    public sealed class TestMath
    {
        // WrapAngle(float) returns angle in range (-180,180]
        [Test]
        public void WrapAngleFloatReturnsInRange()
        {
            float angle = Math.WrapAngle(-181);
            Assert.AreEqual(179, angle);

            angle = Math.WrapAngle(-180);
            Assert.AreEqual(180, angle);

            angle = Math.WrapAngle(-179);
            Assert.AreEqual(-179, angle);

            angle = Math.WrapAngle(179);
            Assert.AreEqual(179, angle);

            angle = Math.WrapAngle(180);
            Assert.AreEqual(180, angle);

            angle = Math.WrapAngle(181);
            Assert.AreEqual(-179, angle);
        }

        // WrapAngles(VectorXYZ) returns angle components in range (-180,180]
        [Test]
        public void WrapAnglesVectorXZReturnsComponentsInRange()
        {
            VectorXZ angles = VectorXZ.one * -181;
            Assert.AreEqual(VectorXZ.one * 179, Math.WrapAngles(angles));

            angles = VectorXZ.one * -180;
            Assert.AreEqual(VectorXZ.one * 180, Math.WrapAngles(angles));

            angles = VectorXZ.one * -179;
            Assert.AreEqual(VectorXZ.one * -179, Math.WrapAngles(angles));

            angles = VectorXZ.one * 179;
            Assert.AreEqual(VectorXZ.one * 179, Math.WrapAngles(angles));

            angles = VectorXZ.one * 180;
            Assert.AreEqual(VectorXZ.one * 180, Math.WrapAngles(angles));

            angles = VectorXZ.one * 181;
            Assert.AreEqual(VectorXZ.one * -179, Math.WrapAngles(angles));
        }

        // WrapAngles(VectorXYZ) returns angle components in range (-180,180]
        [Test]
        public void WrapAnglesVectorXYZReturnsComponentsInRange()
        {
            VectorXYZ angles = VectorXYZ.one * -181;
            //Assert.AreEqual(VectorXYZ.one * 179, Math.WrapAngles(angles)); // Not exact
            Assert.IsTrue(VectorXYZ.one * 179 == (VectorXYZ)Math.WrapAngles(angles));

            angles = VectorXYZ.one * -180;
            //Assert.AreEqual(VectorXYZ.one * 180, Math.WrapAngles(angles)); // Not exact
            Assert.IsTrue(VectorXYZ.one * 180 == (VectorXYZ)Math.WrapAngles(angles));

            angles = VectorXYZ.one * -179;
            //Assert.AreEqual(VectorXYZ.one * -179, Math.WrapAngles(angles)); // Not exact
            Assert.IsTrue(VectorXYZ.one * -179 == (VectorXYZ)Math.WrapAngles(angles));

            angles = VectorXYZ.one * 179;
            //Assert.AreEqual(VectorXYZ.one * 179, Math.WrapAngles(angles)); // Not exact
            Assert.IsTrue(VectorXYZ.one * 179 == (VectorXYZ)Math.WrapAngles(angles));

            angles = VectorXYZ.one * 180;
            //Assert.AreEqual(VectorXYZ.one * 180, Math.WrapAngles(angles)); // Not exact
            Assert.IsTrue(VectorXYZ.one * 180 == (VectorXYZ)Math.WrapAngles(angles));

            angles = VectorXYZ.one * 181;
            //Assert.AreEqual(VectorXYZ.one * -179, Math.WrapAngles(angles)); // Not exact
            Assert.IsTrue(VectorXYZ.one * -179 == (VectorXYZ)Math.WrapAngles(angles));
        }

        [Test]
        public void LimitFloatMagnitude()
        {
            float limit = 1.0f;
            Assert.AreEqual(limit, Math.LimitMagnitude(2, limit));

            Assert.AreEqual(limit, Math.LimitMagnitude(1.00001f, limit));
            Assert.AreEqual(limit, Math.LimitMagnitude(1, limit));

            Assert.AreEqual(0.99999f, Math.LimitMagnitude(0.99999f, limit));

            Assert.AreEqual(-0.99999f, Math.LimitMagnitude(-0.99999f, limit));
            Assert.AreEqual(-1, Math.LimitMagnitude(-1, limit));
            Assert.AreEqual(-1f, Math.LimitMagnitude(-1.00001f, limit));
        }

        [Test]
        public void LimitVectorXZMagnitude()
        {
            float limit = 1.0f;
            VectorXZ vector = new VectorXZ(1, 2);
            Assert.IsTrue(Mathf.Approximately(limit, Math.LimitMagnitude(vector, limit).magnitude));

            vector = new VectorXZ(0.3f, 0.3f);
            Assert.IsTrue(Mathf.Approximately(vector.magnitude, Math.LimitMagnitude(vector, limit).magnitude));
        }

        [Test]
        public void LimitVectorXYZMagnitude()
        {
            float limit = 1.0f;
            VectorXYZ vector = new VectorXYZ(1, 2, 3);
            Assert.IsTrue(Mathf.Approximately(limit, Math.LimitMagnitude(vector, limit).magnitude));

            vector = new VectorXYZ(0.3f, 0.3f, 0.3f);
            Assert.IsTrue(Mathf.Approximately(vector.magnitude, Math.LimitMagnitude(vector, limit).magnitude));
        }

        [Test]
        public void DegreeToVectorXZ()
        {
            float sqrt2Over2 = Mathf.Sqrt(2.0f) / 2.0f;

            Assert.IsTrue(new VectorXZ(0, 1) == Math.DegreeAngleToVectorXZ(0));

            Assert.IsTrue(new VectorXZ(sqrt2Over2, sqrt2Over2) == Math.DegreeAngleToVectorXZ(45));
            Assert.IsTrue(new VectorXZ(-sqrt2Over2, sqrt2Over2) == Math.DegreeAngleToVectorXZ(-45));

            Assert.IsTrue(new VectorXZ(1, 0) == Math.DegreeAngleToVectorXZ(90));
            Assert.IsTrue(new VectorXZ(-1, 0) == Math.DegreeAngleToVectorXZ(-90));

            Assert.IsTrue(new VectorXZ(sqrt2Over2, -sqrt2Over2) == Math.DegreeAngleToVectorXZ(135));
            Assert.IsTrue(new VectorXZ(-sqrt2Over2, -sqrt2Over2) == Math.DegreeAngleToVectorXZ(-135));

            Assert.IsTrue(new VectorXZ(0, -1) == Math.DegreeAngleToVectorXZ(180));
            Assert.IsTrue(new VectorXZ(0, -1) == Math.DegreeAngleToVectorXZ(-180));

            Assert.IsTrue(new VectorXZ(-sqrt2Over2, -sqrt2Over2) == Math.DegreeAngleToVectorXZ(225));
            Assert.IsTrue(new VectorXZ(sqrt2Over2, -sqrt2Over2) == Math.DegreeAngleToVectorXZ(-225));

            Assert.IsTrue(new VectorXZ(-1, 0) == Math.DegreeAngleToVectorXZ(270));
            Assert.IsTrue(new VectorXZ(1, 0) == Math.DegreeAngleToVectorXZ(-270));

            Assert.IsTrue(new VectorXZ(-sqrt2Over2, sqrt2Over2) == Math.DegreeAngleToVectorXZ(315));
            Assert.IsTrue(new VectorXZ(sqrt2Over2, sqrt2Over2) == Math.DegreeAngleToVectorXZ(-315));

            Assert.IsTrue(new VectorXZ(0, 1) == Math.DegreeAngleToVectorXZ(360));
            Assert.IsTrue(new VectorXZ(0, 1) == Math.DegreeAngleToVectorXZ(-360));
        }
    }
}