using GameBrains.Entities.EntityData;
using GameBrains.Extensions.Vectors;
using NUnit.Framework;
using UnityEngine;

namespace UnitTests.PlayModeTests
{
    public sealed class TestKinematicData
    {
        [Test]
        public void TestKinematicDataConstructor()
        {
            Transform tempTransform = new GameObject().transform;
            KinematicData kinematicData = tempTransform;
            
            Assert.AreEqual(KinematicData.DefaultMaximumSpeed, kinematicData.MaximumSpeed);
            Assert.AreEqual(KinematicData.DefaultMaximumAngularSpeed, kinematicData.MaximumAngularSpeed);
            Assert.AreEqual(KinematicData.DefaultMaximumAcceleration, kinematicData.MaximumAcceleration);
            Assert.AreEqual(KinematicData.DefaultMaximumAngularAcceleration, kinematicData.MaximumAngularAcceleration);

            Assert.AreEqual(VectorXYZ.zero, kinematicData.Position);
            Assert.AreEqual(Quaternion.identity, kinematicData.Rotation);

            Assert.AreEqual(VectorXZ.zero, kinematicData.Location);
            Assert.AreEqual(0, kinematicData.Orientation);

            Assert.AreEqual(VectorXZ.zero, kinematicData.Velocity);
            Assert.AreEqual(0, kinematicData.AngularVelocity);

            Assert.AreEqual(VectorXZ.zero, kinematicData.Acceleration);
            Assert.AreEqual(0, kinematicData.AngularAcceleration);

            kinematicData = tempTransform;
            kinematicData.MaximumSpeed = 6;
            kinematicData.MaximumAngularSpeed = 360;
            kinematicData.MaximumAcceleration = 1;
            kinematicData.MaximumAngularAcceleration = 180;
            kinematicData.Location = new VectorXZ(1, 1);
            kinematicData.Orientation = 181;
            kinematicData.Velocity = new VectorXZ(3, 6);
            kinematicData.AngularVelocity = 361;
            kinematicData.Acceleration = new VectorXZ(1, 1);
            kinematicData.AngularAcceleration = 181;

            float sqrt2Over2 = Mathf.Sqrt(2.0f) / 2.0f;

            Assert.AreEqual(6, kinematicData.MaximumSpeed);
            Assert.AreEqual(360, kinematicData.MaximumAngularSpeed);
            Assert.AreEqual(1, kinematicData.MaximumAcceleration);
            Assert.AreEqual(180, kinematicData.MaximumAngularAcceleration);

            Assert.AreEqual(new VectorXZ(1, 1), kinematicData.Location);
            Assert.AreEqual(-179, kinematicData.Orientation);

            Assert.IsTrue(new VectorXZ(2.68328f, 5.36656f) == kinematicData.Velocity);
            Assert.AreEqual(360, kinematicData.AngularVelocity);

            Assert.AreEqual(new VectorXZ(sqrt2Over2, sqrt2Over2), kinematicData.Acceleration);
            Assert.AreEqual(180, kinematicData.AngularAcceleration);
        }

        [Test]
        public void TestKinematicDataCopyConstructor()
        {
            Transform tempTransform = new GameObject().transform;
            KinematicData kinematicDataSource = tempTransform;
            kinematicDataSource.MaximumSpeed = 1;
            kinematicDataSource.MaximumAngularSpeed = 1;
            kinematicDataSource.MaximumAcceleration = 1;
            kinematicDataSource.MaximumAngularAcceleration = 1;
            kinematicDataSource.Location = new VectorXZ(1, 1);
            kinematicDataSource.Orientation = 1;
            kinematicDataSource.Velocity = new VectorXZ(1, 1);
            kinematicDataSource.AngularVelocity = 1;
            kinematicDataSource.Acceleration = new VectorXZ(1, 1);
            kinematicDataSource.AngularAcceleration = 1;

            KinematicData kinematicDataCopy = kinematicDataSource;

            Assert.AreEqual(kinematicDataSource.MaximumSpeed, kinematicDataCopy.MaximumSpeed);
            Assert.AreEqual(kinematicDataSource.MaximumAngularSpeed, kinematicDataCopy.MaximumAngularSpeed);
            Assert.AreEqual(kinematicDataSource.MaximumAcceleration, kinematicDataCopy.MaximumAcceleration);
            Assert.AreEqual(kinematicDataSource.MaximumAngularAcceleration, kinematicDataCopy.MaximumAngularAcceleration);

            Assert.AreEqual(kinematicDataSource.Location, kinematicDataCopy.Location);
            Assert.AreEqual(kinematicDataSource.Orientation, kinematicDataCopy.Orientation);

            Assert.AreEqual(kinematicDataSource.Velocity, kinematicDataCopy.Velocity);
            Assert.AreEqual(kinematicDataSource.AngularVelocity, kinematicDataCopy.AngularVelocity);

            Assert.AreEqual(kinematicDataSource.Acceleration, kinematicDataCopy.Acceleration);
            Assert.AreEqual(kinematicDataSource.AngularAcceleration, kinematicDataCopy.AngularAcceleration);
        }

        [Test]
        public void TestKinematicDataPosition()
        {
            Transform tempTransform = new GameObject().transform;
            KinematicData kinematicData = tempTransform;
            kinematicData.Location = new VectorXZ(1, 1);

            Assert.AreEqual(new VectorXZ(1, 1), kinematicData.Location);
        }


        [Test]
        public void TestKinematicDataOrientation()
        {
            Transform tempTransform = new GameObject().transform;
            KinematicData kinematicData = tempTransform;

            kinematicData.Orientation = 359;
            Assert.AreEqual(-1, kinematicData.Orientation);

            kinematicData.Orientation = 360;
            Assert.AreEqual(0, kinematicData.Orientation);

            kinematicData.Orientation = 361;
            Assert.AreEqual(1, kinematicData.Orientation);

            kinematicData.Orientation = -180;
            Assert.AreEqual(180, kinematicData.Orientation);

            kinematicData.Orientation = -181;
            Assert.AreEqual(179, kinematicData.Orientation);

            kinematicData.Orientation = -179;
            Assert.AreEqual(-179, kinematicData.Orientation);
        }

        [Test]
        public void TestKinematicDataVelocity()
        {
            Transform tempTransform = new GameObject().transform;
            KinematicData kinematicData = tempTransform;

            kinematicData.Velocity = new VectorXZ(0, 0);
            Assert.AreEqual(new VectorXZ(0, 0), kinematicData.Velocity);

            kinematicData.Velocity = new VectorXZ(3, 6);
            Assert.IsTrue(new VectorXZ(2.23607f, 4.47213f) == kinematicData.Velocity);

            kinematicData.Velocity = new VectorXZ(3, 3);
            Assert.IsTrue(new VectorXZ(3, 3) == kinematicData.Velocity);
        }

        [Test]
        public void TestKinematicDataAngularVelocity()
        {
            Transform tempTransform = new GameObject().transform;
            KinematicData kinematicData = tempTransform;

            kinematicData.AngularVelocity = 0;
            Assert.AreEqual(0, kinematicData.AngularVelocity);

            kinematicData.AngularVelocity = 360;
            Assert.AreEqual(360, kinematicData.AngularVelocity);

            kinematicData.AngularVelocity = 361;
            Assert.AreEqual(360, kinematicData.AngularVelocity);

            kinematicData.AngularVelocity = -360;
            Assert.AreEqual(-360, kinematicData.AngularVelocity);

            kinematicData.AngularVelocity = -361;
            Assert.AreEqual(-360, kinematicData.AngularVelocity);
        }

        [Test]
        public void TestKinematicDataAcceleration()
        {
            Transform tempTransform = new GameObject().transform;
            KinematicData kinematicData = tempTransform;

            kinematicData.Acceleration = new VectorXZ(0, 0);
            Assert.AreEqual(new VectorXZ(0, 0), kinematicData.Acceleration);

            kinematicData.Acceleration = new VectorXZ(3, 6);
            float magnitude = Mathf.Sqrt(3 * 3 + 6 * 6);
            float x = 3 / magnitude * kinematicData.MaximumAcceleration;
            float y = 6 / magnitude * kinematicData.MaximumAcceleration;
            Assert.IsTrue(new VectorXZ(x, y) == kinematicData.Acceleration);

            kinematicData.Acceleration = new VectorXZ(3, 3);
            magnitude = Mathf.Sqrt(3 * 3 + 3 * 3);
            x = 3 / magnitude * kinematicData.MaximumAcceleration;
            y = 3 / magnitude * kinematicData.MaximumAcceleration;
            Assert.IsTrue(new VectorXZ(x, y) == kinematicData.Acceleration);
        }

        [Test]
        public void TestKinematicDataAngularAcceleration()
        {
            Transform tempTransform = new GameObject().transform;
            KinematicData kinematicData = tempTransform;

            kinematicData.AngularAcceleration = 0;
            Assert.AreEqual(0, kinematicData.AngularAcceleration);

            kinematicData.AngularAcceleration = 180;
            Assert.AreEqual(180, kinematicData.AngularAcceleration);

            kinematicData.AngularAcceleration = 181;
            Assert.AreEqual(180, kinematicData.AngularAcceleration);

            kinematicData.AngularAcceleration = -180;
            Assert.AreEqual(-180, kinematicData.AngularAcceleration);

            kinematicData.AngularAcceleration = -181;
            Assert.AreEqual(-180, kinematicData.AngularAcceleration);
        }

        [Test]
        public void TestKinematicDataSpeed()
        {
            Transform tempTransform = new GameObject().transform;
            KinematicData kinematicData = tempTransform;

            kinematicData.Velocity = new VectorXZ(0, 0);
            Assert.AreEqual(0, kinematicData.Speed);

            kinematicData.Velocity = new VectorXZ(0, 1);
            Assert.AreEqual(1, kinematicData.Speed);

            kinematicData.Velocity = new Vector2(0, -1);
            Assert.AreEqual(1, kinematicData.Speed);

            kinematicData.Velocity = new VectorXZ(1, 0);
            Assert.AreEqual(1, kinematicData.Speed);

            kinematicData.Velocity = new VectorXZ(-1, 0);
            Assert.AreEqual(1, kinematicData.Speed);


            kinematicData.Velocity = new VectorXZ(4, 3);
            Assert.AreEqual(5, kinematicData.Speed);

            kinematicData.Velocity = new VectorXZ(-4, -3);
            Assert.AreEqual(5, kinematicData.Speed);

            kinematicData.Velocity = new VectorXZ(3, 4);
            Assert.AreEqual(5, kinematicData.Speed);

            kinematicData.Velocity = new VectorXZ(-3, -4);
            Assert.AreEqual(5, kinematicData.Speed);

            kinematicData.Velocity = new VectorXZ(4, 4); // max speed is 5
            Assert.AreEqual(5, kinematicData.Speed);

            kinematicData.Velocity = new VectorXZ(-4, -4); // max speed is 5
            Assert.AreEqual(5, kinematicData.Speed);
        }

        [Test]
        public void TestKinematicDataHeading()
        {
            float sqrt2Over2 = Mathf.Sqrt(2) / 2;

            Transform tempTransform = new GameObject().transform;
            KinematicData kinematicData = tempTransform;

            kinematicData.Orientation = -270;
            Assert.IsTrue(new VectorXZ(1, 0) == kinematicData.HeadingVectorXZ);
            Assert.IsTrue(new VectorXYZ(1, 0, 0) == kinematicData.HeadingVectorXYZ);

            kinematicData.Orientation = -225;
            Assert.IsTrue(new VectorXZ(sqrt2Over2, -sqrt2Over2) == kinematicData.HeadingVectorXZ);
            Assert.IsTrue(new VectorXYZ(sqrt2Over2, 0, -sqrt2Over2) == kinematicData.HeadingVectorXYZ);

            kinematicData.Orientation = -180;
            Assert.IsTrue(new VectorXZ(0, -1) == kinematicData.HeadingVectorXZ);
            Assert.IsTrue(new VectorXYZ(0, 0, -1) == kinematicData.HeadingVectorXYZ);

            kinematicData.Orientation = -135;
            Assert.IsTrue(new VectorXZ(-sqrt2Over2, -sqrt2Over2) == kinematicData.HeadingVectorXZ);
            Assert.IsTrue(new VectorXYZ(-sqrt2Over2, 0, -sqrt2Over2) == kinematicData.HeadingVectorXYZ);

            kinematicData.Orientation = -90;
            Assert.IsTrue(new VectorXZ(-1, 0) == kinematicData.HeadingVectorXZ);
            Assert.IsTrue(new VectorXYZ(-1, 0, 0) == kinematicData.HeadingVectorXYZ);

            kinematicData.Orientation = -45;
            Assert.IsTrue(new VectorXZ(-sqrt2Over2, sqrt2Over2) == kinematicData.HeadingVectorXZ);
            Assert.IsTrue(new VectorXYZ(-sqrt2Over2, 0, sqrt2Over2) == kinematicData.HeadingVectorXYZ);

            kinematicData.Orientation = 0;
            Assert.IsTrue(new VectorXZ(0, 1) == kinematicData.HeadingVectorXZ);
            Assert.IsTrue(new VectorXYZ(0, 0, 1) == kinematicData.HeadingVectorXYZ);

            kinematicData.Orientation = 45;
            Assert.IsTrue(new VectorXZ(sqrt2Over2, sqrt2Over2) == kinematicData.HeadingVectorXZ);
            Assert.IsTrue(new VectorXYZ(sqrt2Over2, 0, sqrt2Over2) == kinematicData.HeadingVectorXYZ);

            kinematicData.Orientation = 90;
            Assert.IsTrue(new VectorXZ(1, 0) == kinematicData.HeadingVectorXZ);
            Assert.IsTrue(new VectorXYZ(1, 0, 0) == kinematicData.HeadingVectorXYZ);

            kinematicData.Orientation = 135;
            Assert.IsTrue(new VectorXZ(sqrt2Over2, -sqrt2Over2) == kinematicData.HeadingVectorXZ);
            Assert.IsTrue(new VectorXYZ(sqrt2Over2, 0, -sqrt2Over2) == kinematicData.HeadingVectorXYZ);

            kinematicData.Orientation = 180;
            Assert.IsTrue(new VectorXZ(0, -1) == kinematicData.HeadingVectorXZ);
            Assert.IsTrue(new VectorXYZ(0, 0, -1) == kinematicData.HeadingVectorXYZ);

            kinematicData.Orientation = 225;
            Assert.IsTrue(new VectorXZ(-sqrt2Over2, -sqrt2Over2) == kinematicData.HeadingVectorXZ);
            Assert.IsTrue(new VectorXYZ(-sqrt2Over2, 0, -sqrt2Over2) == kinematicData.HeadingVectorXYZ);

            kinematicData.Orientation = 270;
            Assert.IsTrue(new VectorXZ(-1, 0) == kinematicData.HeadingVectorXZ);
            Assert.IsTrue(new VectorXYZ(-1, 0, 0) == kinematicData.HeadingVectorXYZ);
        }

        [Test]
        public void TestKinematicDataCalculateVelocity()
        {
            Transform tempTransform = new GameObject().transform;
            KinematicData kinematicData = tempTransform;

            float deltaTime = 1;

            kinematicData.Velocity = new VectorXZ(1, 1);
            kinematicData.Acceleration = new VectorXZ(0, 0.1f);
            kinematicData.CalculateVelocity(deltaTime);

            Assert.AreEqual(new VectorXZ(1, 1.1f), kinematicData.Velocity);

            kinematicData.Acceleration = new VectorXZ(0.5f, 0);
            kinematicData.CalculateVelocity(deltaTime);

            Assert.AreEqual(new VectorXZ(1.5f, 1.1f),kinematicData.Velocity);

        }

        [Test]
        public void TestKinematicDataCalculateAngularVelocity()
        {
            Transform tempTransform = new GameObject().transform;
            KinematicData kinematicData = tempTransform;

            float deltaTime = 1;

            kinematicData.AngularVelocity = 90;
            kinematicData.AngularAcceleration = 45;

            kinematicData.CalculateAngularVelocity(deltaTime);

            // 90 + 45 * 1
            Assert.AreEqual(135, kinematicData.AngularVelocity);
        }

        [Test]
        public void TestKinematicDataCalculatePosition()
        {
            Transform tempTransform = new GameObject().transform;
            KinematicData kinematicData = tempTransform;
            kinematicData.Velocity = new VectorXZ(0, 10); // max speed is 5

            float deltaTime = 2;

            kinematicData.CalculatePosition(deltaTime);

            Assert.AreEqual(new VectorXZ(0, 10), kinematicData.Location);

            kinematicData = tempTransform;
            kinematicData.Velocity = new VectorXZ(0, 4);

            deltaTime = 4;

            kinematicData.CalculatePosition(deltaTime);

            Assert.AreEqual(new VectorXZ(0, 26), kinematicData.Location);

            kinematicData = tempTransform;
            kinematicData.Location = new VectorXZ(1, 1);
            kinematicData.Velocity = new VectorXZ(0, 4);

            deltaTime = 4;

            kinematicData.CalculatePosition(deltaTime);

            Assert.AreEqual(new VectorXZ(1, 17), kinematicData.Location);
        }

        [Test]
        public void TestKinematicDataCalculateOrientation()
        {
            Transform tempTransform = new GameObject().transform;
            KinematicData kinematicData = tempTransform;
            kinematicData.Orientation = 360; // wrap angle to 0

            float deltaTime = 2;

            kinematicData.CalculateOrientation(deltaTime);

            Assert.AreEqual(0, kinematicData.Orientation);

            kinematicData = tempTransform;
            kinematicData.Orientation = 90;

            kinematicData.CalculateOrientation(deltaTime);

            Assert.AreEqual(90, kinematicData.Orientation);

            kinematicData = tempTransform;
            kinematicData.Orientation = 90;
            kinematicData.AngularVelocity = 450;
            // max angular speed is 360

            kinematicData.CalculateOrientation(deltaTime);

            Assert.AreEqual(90, kinematicData.Orientation);

            kinematicData = tempTransform;
            kinematicData.Orientation = 90;
            kinematicData.AngularVelocity = 90;

            kinematicData.CalculateOrientation(deltaTime);

            // 90 + 90 * 2 = 270 wrap angle is -90
            Assert.AreEqual(-90, kinematicData.Orientation);
        }

        [Test]
        public void TestKinematicDataUpdate()
        {
            Transform tempTransform = new GameObject().transform;
            KinematicData kinematicData = tempTransform;

            kinematicData.Location = new VectorXZ(1, 1);
            kinematicData.Orientation = 90;
            kinematicData.Velocity = new VectorXZ(0, 1);
            kinematicData.AngularVelocity = 90;
            kinematicData.Acceleration = new VectorXZ(0, 0.1f);
            kinematicData.AngularAcceleration = 45f;

            float deltaTime = 1;

            kinematicData.DoUpdate(deltaTime);

            // Acceleration = (0, 0.1)
            Assert.AreEqual(new VectorXZ(0,0.1f), kinematicData.Acceleration);
            // AngularAcceleration = 45
            Assert.AreEqual(45, kinematicData.AngularAcceleration);
            // Velocity = (0, 1) + (0, 0.1) * 1 = (0, 1.1)
            Assert.AreEqual(new VectorXZ(0, 1.1f), kinematicData.Velocity);
            // AngularVelocity = 90 + 45 * 1 = 135
            Assert.AreEqual(135, kinematicData.AngularVelocity);
            // Acceleration = (0, 0.1)
            // Velocity = (0, 1)
            // Position = (1, 1) + (0, 1) * 1 + 0.1 * 1 * 1 / 2 = (1, 2.05)
            Assert.AreEqual(new VectorXZ(1, 2.05f), kinematicData.Location);
            // AngularAcceleration = 45
            // AngularVelocity = 90
            // Orientation = 90 + 90 * 1 + 45 * 1 * 1 / 2 = 202.5 wrap angle is -157.5
            Assert.AreEqual(-157.5f, kinematicData.Orientation);
        }
    }
}