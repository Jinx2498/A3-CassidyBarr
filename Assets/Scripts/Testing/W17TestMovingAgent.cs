using GameBrains.Entities;
using GameBrains.Extensions.MonoBehaviours;
using GameBrains.Extensions.Vectors;

namespace Testing
{
	public class W17TestMovingAgent : ExtendedMonoBehaviour
	{
		public bool testRespawning;
		public bool testSetVelocity;
		public bool testSetAcceleration;

		public VectorXZ spawnPoint;
		public VectorXZ velocity;
		public VectorXZ acceleration;

		public MovingAgent movingAgent;

		public override void Update()
		{
			base.Update();

			if (movingAgent == null) { return; }
			
			if (testRespawning)
			{
				testRespawning = false;

				TestRespawn();
			}

			if (testSetVelocity)
			{
				testSetVelocity = false;

				TestSetVelocity();
			}

			if (testSetAcceleration)
			{
				testSetAcceleration = false;

				TestSetAcceleration();
			}
		}

		void TestRespawn()
		{
			movingAgent.Spawn((VectorXYZ)spawnPoint);
		}

		void TestSetVelocity()
		{
			movingAgent.KinematicData.Velocity = velocity;
		}

		void TestSetAcceleration()
		{
			movingAgent.KinematicData.Acceleration = acceleration;
		}
	}
}