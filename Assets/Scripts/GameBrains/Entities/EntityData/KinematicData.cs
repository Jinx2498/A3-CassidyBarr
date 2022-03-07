using GameBrains.Extensions.MathExtensions;
using GameBrains.Extensions.Vectors;
using GameBrains.Visualization;
using UnityEngine;

namespace GameBrains.Entities.EntityData
{
	[System.Serializable]
	public class KinematicData : StaticData
	{
		#region Creators

		public static KinematicData CreateKinematicDataInstance(Transform t)
		{
			KinematicData kinematicData = CreateInstance<KinematicData>();
			InitializeKinematicData(t, kinematicData);
			return kinematicData;
		}

		protected static void InitializeKinematicData(Transform t, KinematicData kinematicData)
		{
			InitializeStaticData(t, kinematicData);

			kinematicData.MaximumSpeed = DefaultMaximumSpeed;
			kinematicData.MaximumAngularSpeed = DefaultMaximumAngularSpeed;
			kinematicData.MaximumAcceleration = DefaultMaximumAcceleration;
			kinematicData.MaximumAngularAcceleration = DefaultMaximumAngularAcceleration;

			kinematicData.ResetKinematicData();
		}
		
		public void ResetKinematicData()
		{
			Velocity = VectorXZ.zero;
			AngularVelocity = 0;
		
			Acceleration = VectorXZ.zero;
			AngularAcceleration = 0;
		}

		#endregion Creators

		#region Casting

		public new MovingAgent Owner => owner as MovingAgent;

		public static implicit operator KinematicData(Transform t)
		{
			return CreateKinematicDataInstance(t);
		}

		#endregion Casting

		#region Kinematic Data

		protected VectorXZ velocity;

		public VectorXZ Velocity
		{
			get => velocity;

			set => velocity = Math.LimitMagnitude(value, MaximumSpeed);
		}

		protected float angularVelocity;

		public float AngularVelocity
		{
			get => angularVelocity;

			set => angularVelocity = Math.LimitMagnitude(value, MaximumAngularSpeed);
		}

		protected VectorXZ acceleration;

		public VectorXZ Acceleration
		{
			get => acceleration;

			set => acceleration = Math.LimitMagnitude(value, MaximumAcceleration);
		}

		protected float angularAcceleration;

		public float AngularAcceleration
		{
			get => angularAcceleration;

			set => angularAcceleration = Math.LimitMagnitude(value, MaximumAngularAcceleration);
		}

		public float Speed => velocity.magnitude;

		public VectorXYZ VelocityXYZ => (VectorXYZ)Velocity;

		#endregion Kinematic Data

		#region Default limits

		public const float DefaultMaximumSpeed = 5f;
		public float MaximumSpeed { get; set; }

		public const float DefaultMaximumAngularSpeed = 45f;
		public float MaximumAngularSpeed { get; set; }

		public const float DefaultMaximumAcceleration = 0.5f;
		public float MaximumAcceleration { get; set; }

		public const float DefaultMaximumAngularAcceleration = 0.5f;
		public float MaximumAngularAcceleration { get; set; }

		#endregion Default limits

		#region Update

		// public virtual void Update(float deltaTime)
		// {
		// 	CalculatePosition(deltaTime);
		// 	CalculateOrientation(deltaTime);
		//
		// 	UpdateVelocities(deltaTime);
		// }

		public virtual void DoUpdate(float deltaTime, bool updatePosition = true)
		{
			if (updatePosition) CalculatePosition(deltaTime);

			CalculateOrientation(deltaTime);

			UpdateVelocities(deltaTime);
		}

		public void CalculatePosition(float deltaTime)
		{

			// Use average of Vinitial and Vfinal
			// deltaP = (Vinital + Vfinal) / 2 * t
			// Vfinal = Vinitial + A * t
			// deltaP = (Vinitial + Vinitial + A * t) / 2 * t
			// deltaP = (2 * Vinitial + A * t) / 2 * t
			// deltaP = Vinitial * t + A * t * t / 2
			float halfDeltaTimeSquared = (deltaTime * deltaTime) / 2;
			VectorXZ positionOffset = (Velocity * deltaTime) + (Acceleration * halfDeltaTimeSquared);
			Location += positionOffset;
		}

		public void CalculateOrientation(float deltaTime)
		{
			// Use average of AVinitial and AVfinal
			// deltaO = (AVinital + AVfinal) / 2 * t
			// AVfinal = AVinitial + AA * t
			// deltaO = (AVinitial + AVinitial + AA * t) / 2 * t
			// deltaO = (2 * AVinitial + AA * t) / 2 * t
			// deltaO = AVinitial * t + AA * t * t / 2
			float halfDeltaTimeSquared = (deltaTime * deltaTime) / 2;
			float orientationOffset = (AngularVelocity * deltaTime) + (AngularAcceleration * halfDeltaTimeSquared);
			Orientation += orientationOffset;
		}

		public void UpdateVelocities(float deltaTime)
		{
			CalculateVelocity(deltaTime);
			CalculateAngularVelocity(deltaTime);
		}

		public void CalculateVelocity(float deltaTime)
		{
			Velocity += Acceleration * deltaTime;
		}

		public void CalculateAngularVelocity(float deltaTime)
		{
			AngularVelocity += AngularAcceleration * deltaTime;
		}

		#endregion Update

		#region Can move checks

		#region CanMoveTo

		public bool CanMoveTo(
			VectorXYZ endPosition,
			CapsuleCastVisualizer capsuleCastVisualizer = null,
			bool showVisualizer = false,
			bool showOnlyWhenBlocked = false,
			float castRadiusMultiplier = 1.5f,
			Color? clearColor = null,
			Color? blockedColor = null)
		{
			return CanMoveTo(
				endPosition,
				out RaycastHit hitInfo,
				capsuleCastVisualizer,
				showVisualizer,
				showOnlyWhenBlocked,
				castRadiusMultiplier,
				clearColor,
				blockedColor);
		}

		public bool CanMoveTo(
			VectorXYZ endPosition,
			out RaycastHit hitInfo,
			CapsuleCastVisualizer capsuleCastVisualizer = null,
			bool showVisualizer = false,
			bool showOnlyWhenBlocked = false,
			float castRadiusMultiplier = 1.5f,
			Color? clearColor = null,
			Color? blockedColor = null)
		{
			return CanMoveBetween(
				Position,
				endPosition,
				out hitInfo,
				capsuleCastVisualizer,
				showVisualizer,
				showOnlyWhenBlocked,
				castRadiusMultiplier,
				clearColor,
				blockedColor);
		}

		public bool CanMoveTo(
			VectorXYZ endPosition,
			LayerMask obstacleLayers,
			CapsuleCastVisualizer capsuleCastVisualizer = null,
			bool showVisualizer = false,
			bool showOnlyWhenBlocked = false,
			float castRadiusMultiplier = 1.5f,
			Color? clearColor = null,
			Color? blockedColor = null)
		{
			return CanMoveTo(
				endPosition,
				obstacleLayers,
				out RaycastHit hitInfo,
				capsuleCastVisualizer,
				showVisualizer,
				showOnlyWhenBlocked,
				castRadiusMultiplier,
				clearColor,
				blockedColor);
		}

		public bool CanMoveTo(
			VectorXYZ endPosition,
			LayerMask obstacleLayers,
			out RaycastHit hitInfo,
			CapsuleCastVisualizer capsuleCastVisualizer = null,
			bool showVisualizer = false,
			bool showOnlyWhenBlocked = false,
			float castRadiusMultiplier = 1.5f,
			Color? clearColor = null,
			Color? blockedColor = null)
		{
			return CanMoveBetween(
				Position,
				endPosition,
				obstacleLayers,
				out hitInfo,
				capsuleCastVisualizer,
				showVisualizer,
				showOnlyWhenBlocked,
				castRadiusMultiplier,
				clearColor,
				blockedColor);
		}

		#endregion CanMoveTo

		#region CanMoveBetween

		public bool CanMoveBetween(
			VectorXYZ startPosition,
			VectorXYZ endPosition,
			CapsuleCastVisualizer capsuleCastVisualizer = null,
			bool showVisualizer = false,
			bool showOnlyWhenBlocked = false,
			float castRadiusMultiplier = 1.5f,
			Color? clearColor = null,
			Color? blockedColor = null)
		{
			return CanMoveBetween(
				startPosition,
				endPosition,
				out RaycastHit hitInfo,
				capsuleCastVisualizer,
				showVisualizer,
				showOnlyWhenBlocked,
				castRadiusMultiplier,
				clearColor,
				blockedColor);
		}

		public bool CanMoveBetween(
			VectorXYZ startPosition,
			VectorXYZ endPosition,
			out RaycastHit hitInfo,
			CapsuleCastVisualizer capsuleCastVisualizer = null,
			bool showVisualizer = false,
			bool showOnlyWhenBlocked = false,
			float castRadiusMultiplier = 1.5f,
			Color? clearColor = null,
			Color? blockedColor = null)
		{
			return CanMoveBetween(
				startPosition,
				endPosition,
				ObstacleLayerMask,
				out hitInfo,
				capsuleCastVisualizer,
				showVisualizer,
				showOnlyWhenBlocked,
				castRadiusMultiplier,
				clearColor,
				blockedColor);
		}

		public bool CanMoveBetween(
			VectorXYZ startPosition,
			VectorXYZ endPosition,
			LayerMask obstacleLayers,
			CapsuleCastVisualizer capsuleCastVisualizer = null,
			bool showVisualizer = false,
			bool showOnlyWhenBlocked = false,
			float castRadiusMultiplier = 1.5f,
			Color? clearColor = null,
			Color? blockedColor = null)
		{
			return CanMoveBetween(
				startPosition,
				endPosition,
				obstacleLayers,
				out RaycastHit hitInfo,
				capsuleCastVisualizer,
				showVisualizer,
				showOnlyWhenBlocked,
				castRadiusMultiplier,
				clearColor,
				blockedColor);
		}

		public bool CanMoveBetween(
			VectorXYZ startPosition,
			VectorXYZ endPosition,
			LayerMask obstacleLayers,
			out RaycastHit hitInfo,
			CapsuleCastVisualizer capsuleCastVisualizer = null,
			bool showVisualizer = false,
			bool showOnlyWhenBlocked = false,
			float castRadiusMultiplier = 1.5f,
			Color? clearColor = null,
			Color? blockedColor = null)
		{
			if (clearColor == null) clearColor = ClearColor;
			if (blockedColor == null) blockedColor = BlockedColor;

			float distance
				= VectorXYZ.Distance(startPosition, endPosition);
			VectorXYZ direction
				= (endPosition - startPosition) / distance;
			float radius = Radius;

			// VectorXYZ capsuleBottomSphereCenterPosition
			// 	= Bottom + radius * VectorXYZ.up;
			// VectorXYZ capsuleTopSphereCenterPosition
			// 	= Top - radius * VectorXYZ.up;

			VectorXYZ capsuleBottomSphereCenterPosition
				= startPosition + Radius * VectorXYZ.up;
			VectorXYZ capsuleTopSphereCenterPosition
				= startPosition + (Height - Radius) * VectorXYZ.up;

			float castRadius = Radius * castRadiusMultiplier;

			bool blocked = Physics.CapsuleCast(
				capsuleBottomSphereCenterPosition,
				capsuleTopSphereCenterPosition,
				castRadius,
				direction,
				out hitInfo,
				distance,
				obstacleLayers);

			if (capsuleCastVisualizer)
			{
				if (showVisualizer && (!showOnlyWhenBlocked || blocked))
				{
					capsuleCastVisualizer.SetColor(blocked ? blockedColor.Value : clearColor.Value);
					capsuleCastVisualizer.castRadiusMultiplier = castRadiusMultiplier;

					capsuleCastVisualizer.Draw(
						startPosition,
						direction,
						blocked ? hitInfo.distance : distance);
				}
				else
				{
					capsuleCastVisualizer.Hide(true);
				}
			}

			return !blocked;
		}

		#endregion CanMoveBetween

		#region CanStepRight

		public bool CanStepRight(
			CapsuleCastVisualizer capsuleCastVisualizer = null,
			bool showVisualizer = false,
			bool showOnlyWhenBlocked = false,
			float castRadiusMultiplier = 1.5f,
			Color? clearColor = null,
			Color? blockedColor = null)
		{
			return CanStepRight(
				ObstacleLayerMask,
				capsuleCastVisualizer,
				showVisualizer,
				showOnlyWhenBlocked,
				castRadiusMultiplier,
				clearColor,
				blockedColor);
		}

		public bool CanStepRight(
			LayerMask obstacleLayers,
			CapsuleCastVisualizer capsuleCastVisualizer = null,
			bool showVisualizer = false,
			bool showOnlyWhenBlocked = false,
			float castRadiusMultiplier = 1.5f,
			Color? clearColor = null,
			Color? blockedColor = null)
		{
			return CanStepRight(obstacleLayers,
				out VectorXYZ positionOfStep,
				capsuleCastVisualizer,
				showVisualizer,
				showOnlyWhenBlocked,
				castRadiusMultiplier,
				clearColor,
				blockedColor);
		}

		public bool CanStepRight(
			out VectorXYZ positionOfStep,
			CapsuleCastVisualizer capsuleCastVisualizer = null,
			bool showVisualizer = false,
			bool showOnlyWhenBlocked = false,
			float castRadiusMultiplier = 1.5f,
			Color? clearColor = null,
			Color? blockedColor = null)
		{
			return CanStepRight(
				ObstacleLayerMask,
				out positionOfStep,
				capsuleCastVisualizer,
				showVisualizer,
				showOnlyWhenBlocked,
				castRadiusMultiplier,
				clearColor,
				blockedColor);
		}

		public bool CanStepRight(
			LayerMask obstacleLayers,
			out VectorXYZ positionOfStep,
			CapsuleCastVisualizer capsuleCastVisualizer = null,
			bool showVisualizer = false,
			bool showOnlyWhenBlocked = false,
			float castRadiusMultiplier = 1.5f,
			Color? clearColor = null,
			Color? blockedColor = null)
		{
			float stepDistance = Radius * 2;

			positionOfStep
				= Position + stepDistance * (VectorXYZ)OwnerTransform.right;

			return CanMoveTo(
				positionOfStep,
				obstacleLayers,
				capsuleCastVisualizer,
				showVisualizer,
				showOnlyWhenBlocked,
				castRadiusMultiplier,
				clearColor,
				blockedColor);
		}

		#endregion CanStepRight

		#region CanStepLeft

		public bool CanStepLeft(
			CapsuleCastVisualizer capsuleCastVisualizer = null,
			bool showVisualizer = false,
			bool showOnlyWhenBlocked = false,
			float castRadiusMultiplier = 1.5f,
			Color? clearColor = null,
			Color? blockedColor = null)
		{
			return CanStepLeft(
				ObstacleLayerMask,
				capsuleCastVisualizer,
				showVisualizer,
				showOnlyWhenBlocked,
				castRadiusMultiplier,
				clearColor,
				blockedColor);
		}

		public bool CanStepLeft(
			LayerMask obstacleLayers,
			CapsuleCastVisualizer capsuleCastVisualizer = null,
			bool showVisualizer = false,
			bool showOnlyWhenBlocked = false,
			float castRadiusMultiplier = 1.5f,
			Color? clearColor = null,
			Color? blockedColor = null)
		{
			return CanStepLeft(
				obstacleLayers,
				out VectorXYZ positionOfStep,
				capsuleCastVisualizer,
				showVisualizer,
				showOnlyWhenBlocked,
				castRadiusMultiplier,
				clearColor,
				blockedColor);
		}

		public bool CanStepLeft(
			out VectorXYZ positionOfStep,
			CapsuleCastVisualizer capsuleCastVisualizer = null,
			bool showVisualizer = false,
			bool showOnlyWhenBlocked = false,
			float castRadiusMultiplier = 1.5f,
			Color? clearColor = null,
			Color? blockedColor = null)
		{
			return CanStepLeft(
				ObstacleLayerMask,
				out positionOfStep,
				capsuleCastVisualizer,
				showVisualizer,
				showOnlyWhenBlocked,
				castRadiusMultiplier,
				clearColor,
				blockedColor);
		}

		public bool CanStepLeft(
			LayerMask obstacleLayers,
			out VectorXYZ positionOfStep,
			CapsuleCastVisualizer capsuleCastVisualizer = null,
			bool showVisualizer = false,
			bool showOnlyWhenBlocked = false,
			float castRadiusMultiplier = 1.5f,
			Color? clearColor = null,
			Color? blockedColor = null)
		{
			float stepDistance = Radius * 2;

			positionOfStep
				= Position - (stepDistance * (VectorXYZ)OwnerTransform.right);

			return CanMoveTo(
				positionOfStep,
				obstacleLayers,
				capsuleCastVisualizer,
				showVisualizer,
				showOnlyWhenBlocked,
				castRadiusMultiplier,
				clearColor,
				blockedColor);
		}

		#endregion CanStepLeft

		#endregion Can move checks
	}
}