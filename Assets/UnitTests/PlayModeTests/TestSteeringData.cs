using GameBrains.Actuators.Motion.Steering;
using GameBrains.Entities.EntityData;
using GameBrains.Extensions.Vectors;
using NUnit.Framework;
using UnityEngine;

namespace UnitTests.PlayModeTests
{
    public sealed class TestSteeringData
    {
        [Test]
        public void TestSteeringDataConstructor()
        {
            SteeringData steeringData = new GameObject().transform;

            Assert.AreEqual(KinematicData.DefaultMaximumSpeed, steeringData.MaximumSpeed);
            Assert.AreEqual(KinematicData.DefaultMaximumAngularSpeed, steeringData.MaximumAngularSpeed);
            Assert.AreEqual(KinematicData.DefaultMaximumAcceleration, steeringData.MaximumAcceleration);
            Assert.AreEqual(KinematicData.DefaultMaximumAngularAcceleration, steeringData.MaximumAngularAcceleration);

            Assert.AreEqual(VectorXZ.zero, steeringData.Location);
            Assert.AreEqual(0, steeringData.Orientation);

            Assert.AreEqual(VectorXZ.zero, steeringData.Velocity);
            Assert.AreEqual(0, steeringData.AngularVelocity);

            Assert.AreEqual(VectorXZ.zero, steeringData.Acceleration);
            Assert.AreEqual(0, steeringData.AngularAcceleration);

            SteeringData steeringData2 = new GameObject().transform;
            steeringData2.MaximumSpeed = 6;
            steeringData2.MaximumAngularSpeed = 360;
            steeringData2.MaximumAcceleration = 1;
            steeringData2.MaximumAngularAcceleration = 180;
            steeringData2.Location = new VectorXZ(1, 1);
            steeringData2.Orientation = 181;
            steeringData2.Velocity = new VectorXZ(3, 6);
            steeringData2.AngularVelocity = 361;
            steeringData2.Acceleration = new VectorXZ(1, 1);
            steeringData2.AngularAcceleration = 181;

            float sqrt2Over2 = Mathf.Sqrt(2.0f) / 2.0f;

            Assert.AreEqual(6, steeringData2.MaximumSpeed);
            Assert.AreEqual(360, steeringData2.MaximumAngularSpeed);
            Assert.AreEqual(1, steeringData2.MaximumAcceleration);
            Assert.AreEqual(180, steeringData2.MaximumAngularAcceleration);

            Assert.AreEqual(new VectorXZ(1, 1), steeringData2.Location);
            Assert.AreEqual(-179, steeringData2.Orientation);

            Assert.IsTrue(new VectorXZ(2.68328f, 5.36656f) == steeringData2.Velocity);
            Assert.AreEqual(360, steeringData2.AngularVelocity);

            Assert.AreEqual(new VectorXZ(sqrt2Over2, sqrt2Over2), steeringData2.Acceleration);
            Assert.AreEqual(180, steeringData2.AngularAcceleration);
        }

        [Test]
        public void TestSteeringDataCopyConstructor()
        {
            SteeringData steeringDataSource = new GameObject().transform;
            steeringDataSource.MaximumSpeed = 1;
            steeringDataSource.MaximumAngularSpeed = 1;
            steeringDataSource.MaximumAcceleration = 1;
            steeringDataSource.MaximumAngularAcceleration = 1;
            steeringDataSource.Location = new VectorXZ(1, 1);
            steeringDataSource.Orientation = 1;
            steeringDataSource.Velocity = new VectorXZ(1, 1);
            steeringDataSource.AngularVelocity = 1;
            steeringDataSource.Acceleration = new VectorXZ(1, 1);
            steeringDataSource.AngularAcceleration = 1;

            SteeringData steeringDataCopy = steeringDataSource;

            Assert.AreEqual(steeringDataSource.MaximumSpeed, steeringDataCopy.MaximumSpeed);
            Assert.AreEqual(steeringDataSource.MaximumAngularSpeed, steeringDataCopy.MaximumAngularSpeed);
            Assert.AreEqual(steeringDataSource.MaximumAcceleration, steeringDataCopy.MaximumAcceleration);
            Assert.AreEqual(steeringDataSource.MaximumAngularAcceleration, steeringDataCopy.MaximumAngularAcceleration);

            Assert.AreEqual(steeringDataSource.Location, steeringDataCopy.Location);
            Assert.AreEqual(steeringDataSource.Orientation, steeringDataCopy.Orientation);

            Assert.AreEqual(steeringDataSource.Velocity, steeringDataCopy.Velocity);
            Assert.AreEqual(steeringDataSource.AngularVelocity, steeringDataCopy.AngularVelocity);

            Assert.AreEqual(steeringDataSource.Acceleration, steeringDataCopy.Acceleration);
            Assert.AreEqual(steeringDataSource.AngularAcceleration, steeringDataCopy.AngularAcceleration);
        }

        [Test]
        public void TestSteeringDataPosition()
        {
            SteeringData steeringData = new GameObject().transform;

            steeringData.Location = new VectorXZ(1, 1);

            Assert.AreEqual(new VectorXZ(1, 1), steeringData.Location);
        }


        [Test]
        public void TestSteeringDataOrientation()
        {
            SteeringData steeringData = new GameObject().transform;

            steeringData.Orientation = 359;
            Assert.AreEqual(-1, steeringData.Orientation);

            steeringData.Orientation = 360;
            Assert.AreEqual(0, steeringData.Orientation);

            steeringData.Orientation = 361;
            Assert.AreEqual(1, steeringData.Orientation);

            steeringData.Orientation = -180;
            Assert.AreEqual(180, steeringData.Orientation);

            steeringData.Orientation = -181;
            Assert.AreEqual(179, steeringData.Orientation);

            steeringData.Orientation = -179;
            Assert.AreEqual(-179, steeringData.Orientation);
        }

        [Test]
        public void TestSteeringDataVelocity()
        {
            SteeringData steeringData = new GameObject().transform;

            steeringData.Velocity = new VectorXZ(0, 0);
            Assert.AreEqual(new VectorXZ(0, 0), steeringData.Velocity);

            steeringData.Velocity = new VectorXZ(3, 6);
            Assert.IsTrue(new VectorXZ(2.23607f, 4.47213f) == steeringData.Velocity);

            steeringData.Velocity = new VectorXZ(3, 3);
            Assert.IsTrue(new VectorXZ(3, 3) == steeringData.Velocity);
        }

        [Test]
        public void TestSteeringDataAngularVelocity()
        {
            SteeringData steeringData = new GameObject().transform;

            steeringData.AngularVelocity = 0;
            Assert.AreEqual(0, steeringData.AngularVelocity);

            steeringData.AngularVelocity = 360;
            Assert.AreEqual(360, steeringData.AngularVelocity);

            steeringData.AngularVelocity = 361;
            Assert.AreEqual(360, steeringData.AngularVelocity);

            steeringData.AngularVelocity = -360;
            Assert.AreEqual(-360, steeringData.AngularVelocity);

            steeringData.AngularVelocity = -361;
            Assert.AreEqual(-360, steeringData.AngularVelocity);
        }

        [Test]
        public void TestSteeringDataAcceleration()
        {
            SteeringData steeringData = new GameObject().transform;

            steeringData.Acceleration = new VectorXZ(0, 0);
            Assert.AreEqual(new VectorXZ(0, 0), steeringData.Acceleration);

            steeringData.Acceleration = new VectorXZ(3, 6);
            float magnitude = Mathf.Sqrt(3 * 3 + 6 * 6);
            float x = 3 / magnitude * steeringData.MaximumAcceleration;
            float y = 6 / magnitude * steeringData.MaximumAcceleration;
            Assert.IsTrue(new VectorXZ(x, y) == steeringData.Acceleration);

            steeringData.Acceleration = new VectorXZ(3, 3);
            magnitude = Mathf.Sqrt(3 * 3 + 3 * 3);
            x = 3 / magnitude * steeringData.MaximumAcceleration;
            y = 3 / magnitude * steeringData.MaximumAcceleration;
            Assert.IsTrue(new VectorXZ(x, y) == steeringData.Acceleration);
        }

        [Test]
        public void TestSteeringDataAngularAcceleration()
        {
            SteeringData steeringData = new GameObject().transform;

            steeringData.AngularAcceleration = 0;
            Assert.AreEqual(0, steeringData.AngularAcceleration);

            steeringData.AngularAcceleration = 180;
            Assert.AreEqual(180, steeringData.AngularAcceleration);

            steeringData.AngularAcceleration = 181;
            Assert.AreEqual(180, steeringData.AngularAcceleration);

            steeringData.AngularAcceleration = -180;
            Assert.AreEqual(-180, steeringData.AngularAcceleration);

            steeringData.AngularAcceleration = -181;
            Assert.AreEqual(-180, steeringData.AngularAcceleration);
        }

        [Test]
        public void TestSteeringDataSpeed()
        {
            SteeringData steeringData = new GameObject().transform;

            steeringData.Velocity = new VectorXZ(0, 0);
            Assert.AreEqual(0, steeringData.Speed);

            steeringData.Velocity = new VectorXZ(0, 1);
            Assert.AreEqual(1, steeringData.Speed);

            steeringData.Velocity = new VectorXZ(0, -1);
            Assert.AreEqual(1, steeringData.Speed);

            steeringData.Velocity = new VectorXZ(1, 0);
            Assert.AreEqual(1, steeringData.Speed);

            steeringData.Velocity = new VectorXZ(-1, 0);
            Assert.AreEqual(1, steeringData.Speed);


            steeringData.Velocity = new VectorXZ(4, 3);
            Assert.AreEqual(5, steeringData.Speed);

            steeringData.Velocity = new VectorXZ(-4, -3);
            Assert.AreEqual(5, steeringData.Speed);

            steeringData.Velocity = new VectorXZ(3, 4);
            Assert.AreEqual(5, steeringData.Speed);

            steeringData.Velocity = new VectorXZ(-3, -4);
            Assert.AreEqual(5, steeringData.Speed);

            steeringData.Velocity = new VectorXZ(4, 4); // max speed is 5
            Assert.AreEqual(5, steeringData.Speed);

            steeringData.Velocity = new VectorXZ(-4, -4); // max speed is 5
            Assert.AreEqual(5, steeringData.Speed);
        }

        [Test]
        public void TestSteeringDataHeading()
        {
            float sqrt2Over2 = Mathf.Sqrt(2) / 2;

            SteeringData steeringData = new GameObject().transform;

            steeringData.Orientation = -270;
            Assert.IsTrue(new VectorXZ(1, 0) == steeringData.HeadingVectorXZ);
            Assert.IsTrue(new VectorXYZ(1, 0, 0) == steeringData.HeadingVectorXYZ);

            steeringData.Orientation = -225;
            Assert.IsTrue(new VectorXZ(sqrt2Over2, -sqrt2Over2) == steeringData.HeadingVectorXZ);
            Assert.IsTrue(new VectorXYZ(sqrt2Over2, 0, -sqrt2Over2) == steeringData.HeadingVectorXYZ);

            steeringData.Orientation = -180;
            Assert.IsTrue(new VectorXZ(0, -1) == steeringData.HeadingVectorXZ);
            Assert.IsTrue(new VectorXYZ(0, 0, -1) == steeringData.HeadingVectorXYZ);

            steeringData.Orientation = -135;
            Assert.IsTrue(new VectorXZ(-sqrt2Over2, -sqrt2Over2) == steeringData.HeadingVectorXZ);
            Assert.IsTrue(new VectorXYZ(-sqrt2Over2, 0, -sqrt2Over2) == steeringData.HeadingVectorXYZ);

            steeringData.Orientation = -90;
            Assert.IsTrue(new VectorXZ(-1, 0) == steeringData.HeadingVectorXZ);
            Assert.IsTrue(new VectorXYZ(-1, 0, 0) == steeringData.HeadingVectorXYZ);

            steeringData.Orientation = -45;
            Assert.IsTrue(new VectorXZ(-sqrt2Over2, sqrt2Over2) == steeringData.HeadingVectorXZ);
            Assert.IsTrue(new VectorXYZ(-sqrt2Over2, 0, sqrt2Over2) == steeringData.HeadingVectorXYZ);

            steeringData.Orientation = 0;
            Assert.IsTrue(new VectorXZ(0, 1) == steeringData.HeadingVectorXZ);
            Assert.IsTrue(new VectorXYZ(0, 0, 1) == steeringData.HeadingVectorXYZ);

            steeringData.Orientation = 45;
            Assert.IsTrue(new VectorXZ(sqrt2Over2, sqrt2Over2) == steeringData.HeadingVectorXZ);
            Assert.IsTrue(new VectorXYZ(sqrt2Over2, 0, sqrt2Over2) == steeringData.HeadingVectorXYZ);

            steeringData.Orientation = 90;
            Assert.IsTrue(new VectorXZ(1, 0) == steeringData.HeadingVectorXZ);
            Assert.IsTrue(new VectorXYZ(1, 0, 0) == steeringData.HeadingVectorXYZ);

            steeringData.Orientation = 135;
            Assert.IsTrue(new VectorXZ(sqrt2Over2, -sqrt2Over2) == steeringData.HeadingVectorXZ);
            Assert.IsTrue(new VectorXYZ(sqrt2Over2, 0, -sqrt2Over2) == steeringData.HeadingVectorXYZ);

            steeringData.Orientation = 180;
            Assert.IsTrue(new VectorXZ(0, -1) == steeringData.HeadingVectorXZ);
            Assert.IsTrue(new VectorXYZ(0, 0, -1) == steeringData.HeadingVectorXYZ);

            steeringData.Orientation = 225;
            Assert.IsTrue(new VectorXZ(-sqrt2Over2, -sqrt2Over2) == steeringData.HeadingVectorXZ);
            Assert.IsTrue(new VectorXYZ(-sqrt2Over2, 0, -sqrt2Over2) == steeringData.HeadingVectorXYZ);

            steeringData.Orientation = 270;
            Assert.IsTrue(new VectorXZ(-1, 0) == steeringData.HeadingVectorXZ);
            Assert.IsTrue(new VectorXYZ(-1, 0, 0) == steeringData.HeadingVectorXYZ);
        }

        [Test]
        public void TestSteeringDataSetAccumulatedVelocity()
        {
            SteeringData steeringData = new GameObject().transform;
            steeringData.SetAccumulatedVelocity(new VectorXZ(0, 0));
            Assert.IsTrue(steeringData.DoApplyAccumulatedVelocities);
            Assert.AreEqual(new VectorXZ(0,0), steeringData.AccumulatedVelocity);

            steeringData = new GameObject().transform;
            steeringData.SetAccumulatedVelocity(new VectorXZ(0, 1));
            Assert.IsTrue(steeringData.DoApplyAccumulatedVelocities);
            Assert.AreEqual(new VectorXZ(0,1), steeringData.AccumulatedVelocity);

            steeringData = new GameObject().transform;
            steeringData.SetAccumulatedVelocity(new VectorXZ(0, -1));
            Assert.IsTrue(steeringData.DoApplyAccumulatedVelocities);
            Assert.AreEqual(new VectorXZ(0,-1), steeringData.AccumulatedVelocity);

            steeringData = new GameObject().transform;
            steeringData.SetAccumulatedVelocity(new VectorXZ(3, 4));
            Assert.IsTrue(steeringData.DoApplyAccumulatedVelocities);
            Assert.AreEqual(new VectorXZ(3,4), steeringData.AccumulatedVelocity);
        }

        [Test]
        public void TestSteeringDataSetAccumulatedAngularVelocity()
        {
            SteeringData steeringData = new GameObject().transform;
            steeringData.SetAccumulatedAngularVelocity(0);
            Assert.IsTrue(steeringData.DoApplyAccumulatedVelocities);
            Assert.AreEqual(0, steeringData.AccumulatedAngularVelocity);

            steeringData = new GameObject().transform;
            steeringData.SetAccumulatedAngularVelocity(1);
            Assert.IsTrue(steeringData.DoApplyAccumulatedVelocities);
            Assert.AreEqual(1, steeringData.AccumulatedAngularVelocity);

            steeringData = new GameObject().transform;
            steeringData.SetAccumulatedAngularVelocity(-1);
            Assert.IsTrue(steeringData.DoApplyAccumulatedVelocities);
            Assert.AreEqual(-1, steeringData.AccumulatedAngularVelocity);

            steeringData = new GameObject().transform;
            steeringData.SetAccumulatedAngularVelocity(180);
            Assert.IsTrue(steeringData.DoApplyAccumulatedVelocities);
            Assert.AreEqual(180, steeringData.AccumulatedAngularVelocity);
        }

        [Test]
        public void TestSteeringDataSetAccumulatedVelocities()
        {
            SteeringData steeringData = new GameObject().transform;
            steeringData.SetAccumulatedVelocities(new VectorXZ(0, 0), 0);
            Assert.IsTrue(steeringData.DoApplyAccumulatedVelocities);
            Assert.AreEqual(new VectorXZ(0,0), steeringData.AccumulatedVelocity);
            Assert.AreEqual(0, steeringData.AccumulatedAngularVelocity);

            steeringData = new GameObject().transform;
            steeringData.SetAccumulatedVelocities(new VectorXZ(0, 1), 1);
            Assert.IsTrue(steeringData.DoApplyAccumulatedVelocities);
            Assert.AreEqual(new VectorXZ(0,1), steeringData.AccumulatedVelocity);
            Assert.AreEqual(1, steeringData.AccumulatedAngularVelocity);

            steeringData = new GameObject().transform;
            steeringData.SetAccumulatedVelocities(new VectorXZ(0, -1), -1);
            Assert.IsTrue(steeringData.DoApplyAccumulatedVelocities);
            Assert.AreEqual(new VectorXZ(0,-1), steeringData.AccumulatedVelocity);
            Assert.AreEqual(-1, steeringData.AccumulatedAngularVelocity);

            steeringData = new GameObject().transform;
            steeringData.SetAccumulatedVelocities(new VectorXZ(3, 4), 5);
            Assert.IsTrue(steeringData.DoApplyAccumulatedVelocities);
            Assert.AreEqual(new VectorXZ(3,4), steeringData.AccumulatedVelocity);
            Assert.AreEqual(5, steeringData.AccumulatedAngularVelocity);
        }

        [Test]
        public void TestSteeringDataAccumulateVelocities()
        {
            SteeringData steeringData = new GameObject().transform;
            steeringData.Velocity = new VectorXZ(0, 1);
            steeringData.AngularVelocity = 1;
            steeringData.AccumulateVelocities(new VectorXZ(0, 0), 0);
            Assert.AreEqual(new VectorXZ(0, 1), steeringData.Velocity);
            Assert.AreEqual(1, steeringData.AngularVelocity);
            Assert.IsTrue(steeringData.DoApplyAccumulatedVelocities);
            Assert.AreEqual(new VectorXZ(0,0), steeringData.AccumulatedVelocity);
            Assert.AreEqual(0, steeringData.AccumulatedAngularVelocity);

            steeringData.AccumulateVelocities(new VectorXZ(1, 1), 1);
            Assert.IsTrue(steeringData.DoApplyAccumulatedVelocities);
            Assert.AreEqual(new VectorXZ(1,1), steeringData.AccumulatedVelocity);
            Assert.AreEqual(1, steeringData.AccumulatedAngularVelocity);

            steeringData.AccumulateVelocities(new VectorXZ(1, 1), 1);
            Assert.IsTrue(steeringData.DoApplyAccumulatedVelocities);
            Assert.AreEqual(new VectorXZ(2,2), steeringData.AccumulatedVelocity);
            Assert.AreEqual(2, steeringData.AccumulatedAngularVelocity);

            steeringData.AccumulateVelocities(new VectorXZ(2, 2), 360);
            Assert.IsTrue(steeringData.DoApplyAccumulatedVelocities);
            Assert.AreEqual(new VectorXZ(4 ,4), steeringData.AccumulatedVelocity);
            Assert.AreEqual(362, steeringData.AccumulatedAngularVelocity);
        }

        [Test]
        public void TestSteeringDataSetAccumulatedAccelerations()
        {
            SteeringData steeringData = new GameObject().transform;
            steeringData.SetAccumulatedAccelerations(new VectorXZ(0, 0), 0);
            Assert.AreEqual(new VectorXZ(0,0), steeringData.AccumulatedAcceleration);
            Assert.AreEqual(0, steeringData.AccumulatedAngularAcceleration);

            steeringData = new GameObject().transform;
            steeringData.SetAccumulatedAccelerations(new VectorXZ(1, 1), 1);
            Assert.AreEqual(new VectorXZ(1,1), steeringData.AccumulatedAcceleration);
            Assert.AreEqual(1, steeringData.AccumulatedAngularAcceleration);

            steeringData = new GameObject().transform;
            steeringData.SetAccumulatedAccelerations(new VectorXZ(3, 3), 180);
            Assert.AreEqual(new VectorXZ(3,3), steeringData.AccumulatedAcceleration);
            Assert.AreEqual(180, steeringData.AccumulatedAngularAcceleration);
        }

        [Test]
        public void TestSteeringDataAccumulateAccelerations()
        {
            SteeringData steeringData = new GameObject().transform;
            steeringData.AccumulateAccelerations(new VectorXZ(0, 0), 0);
            Assert.AreEqual(new VectorXZ(0,0), steeringData.AccumulatedAcceleration);
            Assert.AreEqual(0, steeringData.AccumulatedAngularAcceleration);

            steeringData.AccumulateAccelerations(new VectorXZ(1, 1), 1);
            Assert.AreEqual(new VectorXZ(1,1), steeringData.AccumulatedAcceleration);
            Assert.AreEqual(1, steeringData.AccumulatedAngularAcceleration);

            steeringData.AccumulateAccelerations(new VectorXZ(3, 3), 180);
            Assert.AreEqual(new VectorXZ(4,4), steeringData.AccumulatedAcceleration);
            Assert.AreEqual(181, steeringData.AccumulatedAngularAcceleration);
        }

        [Test]
        public void TestSteeringDataSetSteeringOutput()
        {
            SteeringData steeringData = new GameObject().transform;
            SteeringOutput steeringOutput = new SteeringOutput();
            steeringOutput.Type = SteeringOutput.Types.Velocities;
            steeringOutput.Linear = new VectorXZ(1, 1);
            steeringOutput.Angular = 1;

            steeringData.SetSteeringOutput(steeringOutput);

            Assert.IsTrue(steeringData.DoApplyAccumulatedVelocities);
            Assert.AreEqual(new VectorXZ(1,1), steeringData.AccumulatedVelocity);
            Assert.AreEqual(1, steeringData.AccumulatedAngularVelocity);

            steeringData = new GameObject().transform;
            steeringOutput = new SteeringOutput();
            steeringOutput.Type = SteeringOutput.Types.Accelerations;
            steeringOutput.Linear = new VectorXZ(1, 1);
            steeringOutput.Angular = 1;

            steeringData.SetSteeringOutput(steeringOutput);

            Assert.AreEqual(new VectorXZ(1,1), steeringData.AccumulatedAcceleration);
            Assert.AreEqual(1, steeringData.AccumulatedAngularAcceleration);
        }

        [Test]
        public void TestSteeringDataAccumulateSteeringOutput()
        {
            SteeringData steeringData = new GameObject().transform;
            SteeringOutput steeringOutput = new SteeringOutput();
            steeringOutput.Type = SteeringOutput.Types.Velocities;
            steeringOutput.Linear = new VectorXZ(1, 1);
            steeringOutput.Angular = 1;

            steeringData.AccumulateSteeringOutput(steeringOutput);

            Assert.IsTrue(steeringData.DoApplyAccumulatedVelocities);
            Assert.AreEqual(new VectorXZ(1,1), steeringData.AccumulatedVelocity);
            Assert.AreEqual(1, steeringData.AccumulatedAngularVelocity);

            steeringData.AccumulateSteeringOutput(steeringOutput);

            Assert.IsTrue(steeringData.DoApplyAccumulatedVelocities);
            Assert.AreEqual(new VectorXZ(2,2), steeringData.AccumulatedVelocity);
            Assert.AreEqual(2, steeringData.AccumulatedAngularVelocity);

            steeringData = new GameObject().transform;
            steeringOutput = new SteeringOutput();
            steeringOutput.Type = SteeringOutput.Types.Accelerations;
            steeringOutput.Linear = new VectorXZ(1, 1);
            steeringOutput.Angular = 1;

            steeringData.AccumulateSteeringOutput(steeringOutput);

            Assert.AreEqual(new VectorXZ(1,1), steeringData.AccumulatedAcceleration);
            Assert.AreEqual(1, steeringData.AccumulatedAngularAcceleration);

            steeringData.AccumulateSteeringOutput(steeringOutput);

            Assert.AreEqual(new VectorXZ(2,2), steeringData.AccumulatedAcceleration);
            Assert.AreEqual(2, steeringData.AccumulatedAngularAcceleration);
        }

        [Test]
        public void TestSteeringDataApplyAccumulatedVelocities()
        {
            SteeringData steeringData = new GameObject().transform;
            steeringData.Velocity = new VectorXZ(0, 1);
            steeringData.AngularVelocity = 90;
            steeringData.SetAccumulatedVelocities(new VectorXZ(1, 1), 45);

            Assert.True(steeringData.DoApplyAccumulatedVelocities);

            steeringData.ApplyAccumulatedVelocities();

            Assert.False(steeringData.DoApplyAccumulatedVelocities);
            Assert.AreEqual(new VectorXZ(1, 2), steeringData.Velocity);
            Assert.AreEqual(135, steeringData.AngularVelocity);
        }

        [Test]
        public void TestSteeringDataCalculateAcceleration()
        {
            SteeringData steeringData = new GameObject().transform;
            steeringData.SetAccumulatedAccelerations(new VectorXZ(3, 4), 360);

            steeringData.CalculateAcceleration();

            Assert.AreEqual(new VectorXZ(0.3f, 0.4f), steeringData.Acceleration);

            steeringData = new GameObject().transform;
            steeringData.Acceleration = new VectorXZ(0, 0.1f);
            steeringData.AngularAcceleration = 45;
            steeringData.SetAccumulatedAccelerations(new VectorXZ(0.1f, 0.1f), 90);

            steeringData.CalculateAcceleration();

            Assert.AreEqual(new VectorXZ(0.1f, 0.2f), steeringData.Acceleration);
        }

        [Test]
        public void TestSteeringDataCalculateAngularAcceleration()
        {
            SteeringData steeringData = new GameObject().transform;
            steeringData.SetAccumulatedAccelerations(new VectorXZ(3, 4), 360);

            steeringData.CalculateAngularAcceleration();

            Assert.AreEqual(180, steeringData.AngularAcceleration);

            steeringData = new GameObject().transform;
            steeringData.Acceleration = new VectorXZ(0, 0.1f);
            steeringData.AngularAcceleration = 45;
            steeringData.SetAccumulatedAccelerations(new VectorXZ(0.1f, 0.1f), 90);

            steeringData.CalculateAngularAcceleration();

            Assert.AreEqual(135, steeringData.AngularAcceleration);
        }

        [Test]
        public void TestSteeringDataCalculateVelocity()
        {
            SteeringData steeringData = new GameObject().transform;
            steeringData.SetAccumulatedVelocity(new VectorXZ(3, 4));
            steeringData.ApplyAccumulatedVelocities();

            float deltaTime = 1;
            steeringData.CalculateVelocity(deltaTime);

            Assert.AreEqual(new VectorXZ(3f, 4f), steeringData.Velocity);

            steeringData.Acceleration = new VectorXZ(0.5f, 0);
            steeringData.CalculateVelocity(deltaTime); // max speed is 5

            Assert.IsTrue(new VectorXZ(3.292523f, 3.76288f) == steeringData.Velocity);


            steeringData = new GameObject().transform;
            steeringData.Velocity = new VectorXZ(1, 1);
            steeringData.Acceleration = new VectorXZ(0, 0.1f);
            steeringData.SetAccumulatedVelocity(new VectorXZ(1, 1));
            steeringData.ApplyAccumulatedVelocities();
            steeringData.CalculateVelocity(deltaTime);

            Assert.AreEqual(new VectorXZ(2, 2.1f), steeringData.Velocity);

            steeringData.Acceleration = new VectorXZ(0.5f, 0);
            steeringData.CalculateVelocity(deltaTime); // max speed is 5

            Assert.AreEqual(new VectorXZ(2.5f, 2.1f),steeringData.Velocity);

        }

        [Test]
        public void TestSteeringDataCalculateAngularVelocity()
        {
            SteeringData steeringData = new GameObject().transform;
            steeringData.SetAccumulatedAngularVelocity(360);
            steeringData.ApplyAccumulatedVelocities();

            float deltaTime = 1;
            steeringData.CalculateAngularVelocity(deltaTime);

            Assert.AreEqual(360, steeringData.AngularVelocity);

            steeringData.AngularAcceleration = 360; // max angular acceleration is 180
            steeringData.CalculateAngularVelocity(deltaTime); // max angular speed is 360

            // 360 + 180 * 1 > 360
            Assert.AreEqual(360, steeringData.AngularVelocity);

            steeringData = new GameObject().transform;
            steeringData.SetAccumulatedAngularVelocity(90);
            steeringData.ApplyAccumulatedVelocities();

            steeringData.CalculateAngularVelocity(deltaTime);

            Assert.AreEqual(90, steeringData.AngularVelocity);

            steeringData.AngularAcceleration = 45; // max angular acceleration is 180
            steeringData.CalculateAngularVelocity(deltaTime); // max angular speed is 360

            // 90 + 45 * 1 = 135
            Assert.AreEqual(135, steeringData.AngularVelocity);


            steeringData = new GameObject().transform;
            steeringData.AngularVelocity = 90;
            steeringData.AngularAcceleration = 45;
            steeringData.SetAccumulatedAngularVelocity(90);
            steeringData.ApplyAccumulatedVelocities();

            steeringData.CalculateAngularVelocity(deltaTime);

            Assert.AreEqual(225, steeringData.AngularVelocity);
        }

        [Test]
        public void TestSteeringDataCalculatePosition()
        {
            SteeringData steeringData = new GameObject().transform;
            steeringData.Velocity = new VectorXZ(0, 10); // max speed is 5

            float deltaTime = 2;

            steeringData.CalculatePosition(deltaTime);

            Assert.AreEqual(new VectorXZ(0, 10), steeringData.Location);

            steeringData = new GameObject().transform;
            steeringData.Velocity = new VectorXZ(0, 4);

            deltaTime = 4;

            steeringData.CalculatePosition(deltaTime);

            Assert.AreEqual(new VectorXZ(0, 16), steeringData.Location);

            steeringData = new GameObject().transform;
            steeringData.Location = new VectorXZ(1, 1);
            steeringData.Velocity = new VectorXZ(0, 4);

            deltaTime = 4;

            steeringData.CalculatePosition(deltaTime);

            Assert.AreEqual(new VectorXZ(1, 17), steeringData.Location);
        }

        [Test]
        public void TestSteeringDataCalculateOrientation()
        {
            SteeringData steeringData = new GameObject().transform;
            steeringData.Orientation = 360; // wrap angle to 0

            float deltaTime = 2;

            steeringData.CalculateOrientation(deltaTime);

            Assert.AreEqual(0, steeringData.Orientation);

            steeringData = new GameObject().transform;
            steeringData.Orientation = 90;

            steeringData.CalculateOrientation(deltaTime);

            Assert.AreEqual(90, steeringData.Orientation);

            steeringData = new GameObject().transform;
            steeringData.Orientation = 90;
            steeringData.AngularVelocity = 450; // max angular speed is 360

            steeringData.CalculateOrientation(deltaTime);

            Assert.AreEqual(90, steeringData.Orientation);

            steeringData = new GameObject().transform;
            steeringData.Orientation = 90;
            steeringData.AngularVelocity = 90;

            steeringData.CalculateOrientation(deltaTime);

            // 90 + 90 * 2 = 270 wrap angle is -90
            Assert.AreEqual(-90, steeringData.Orientation);
        }

        [Test]
        public void TestSteeringDataResetAccumulatedData()
        {
            SteeringData steeringData = new GameObject().transform;
            steeringData.SetAccumulatedVelocities(new VectorXZ(1, 1), 1);
            steeringData.SetAccumulatedAccelerations(new VectorXZ(1, 1), 1);

            Assert.AreEqual(new VectorXZ(1, 1), steeringData.AccumulatedVelocity);
            Assert.AreEqual(1, steeringData.AccumulatedAngularVelocity);
            Assert.AreEqual(new VectorXZ(1, 1), steeringData.AccumulatedAcceleration);
            Assert.AreEqual(1, steeringData.AccumulatedAngularAcceleration);

            steeringData.ResetAccumulatedData();

            Assert.AreEqual(new VectorXZ(0, 0), steeringData.AccumulatedVelocity);
            Assert.AreEqual(0, steeringData.AccumulatedAngularVelocity);
            Assert.AreEqual(new VectorXZ(0, 0), steeringData.AccumulatedAcceleration);
            Assert.AreEqual(0, steeringData.AccumulatedAngularAcceleration);
        }

        [Test]
        public void TestSteeringDataUpdate()
        {
            SteeringData steeringData = new GameObject().transform;

            steeringData.Location = new VectorXZ(1, 1);
            steeringData.Orientation = 90;
            steeringData.Velocity = new VectorXZ(0, 1);
            steeringData.AngularVelocity = 90;
            steeringData.Acceleration = new VectorXZ(0, 0.1f);
            steeringData.AngularAcceleration = 45f;

            steeringData.SetAccumulatedVelocities(new VectorXZ(0, 1), 45);
            steeringData.SetAccumulatedAccelerations(new VectorXZ(0, 0.1f), 45);

            float deltaTime = 1;

            steeringData.DoUpdate(deltaTime);

            // Acceleration = (0, 0.1) + (0, 0.1) = (0, 0.2)
            Assert.AreEqual(new VectorXZ(0,0.2f), steeringData.Acceleration);
            // AngularAcceleration = 45 + 45 * 1 = 90
            Assert.AreEqual(90, steeringData.AngularAcceleration);
            // Velocity = (0, 1) + (0, 1) + (0, 0.2) * 1 = (0, 2.2)
            Assert.AreEqual(new VectorXZ(0, 2.2f), steeringData.Velocity);
            // AngularVelocity = 90 + 45 + 90 * 1 = 225
            Assert.AreEqual(225, steeringData.AngularVelocity);
            // Acceleration = (0, 0.1) + (0, 0.1) = (0, 0.2)
            // Velocity = (0, 1) + (0, 1) = (0, 2)
            // Position = (1, 1) + (0, 2) * 1 + (0, 0.2) * 1 * 1 / 2 = (1, 3.1)
            Assert.AreEqual(new VectorXZ(1, 3.1f), steeringData.Location);
            // AngularAcceleration = 45 + 45 = 90
            // AngularVelocity = 90 + 45 = 135
            // Orientation = 90 + 135 * 1 + 90 * 1 * 1 / 2 = 270 wrap angle is -90
            Assert.AreEqual(-90, steeringData.Orientation);
            
            Assert.AreEqual(new VectorXZ(0, 0), steeringData.AccumulatedVelocity);
            Assert.AreEqual(0, steeringData.AccumulatedAngularVelocity);
            Assert.AreEqual(new VectorXZ(0, 0), steeringData.AccumulatedAcceleration);
            Assert.AreEqual(0, steeringData.AccumulatedAngularAcceleration);
        }
    }
}