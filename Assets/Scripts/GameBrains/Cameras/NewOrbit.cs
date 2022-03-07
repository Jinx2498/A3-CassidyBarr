using UnityEngine;

namespace GameBrains.Cameras
{
	[RequireComponent(typeof(Camera))]
	public class NewOrbit : TargetedCamera
	{
		[SerializeField, Range(1f, 20f)] float distance = 5f;
		[SerializeField, Min(0f)] float focusRadius = 5f;
		[SerializeField, Range(0f, 1f)] float focusCentering = 0.5f;
		[SerializeField, Range(1f, 360f)] float rotationSpeed = 90f;
		[SerializeField, Range(-89f, 89f)] float minVerticalAngle = -45f, maxVerticalAngle = 45f;
		[SerializeField, Min(0f)] float alignDelay = 5f;
		[SerializeField, Range(0f, 90f)] float alignSmoothRange = 45f;
		[SerializeField] LayerMask obstructionMask = -1;
		Camera regularCamera;

		Vector3 focusPoint, previousFocusPoint, targetPosition;
		Vector2 orbitAngles = new Vector2(45f, 0f);
		float lastManualRotationTime;

		Vector3 CameraHalfExtends
		{
			get
			{
				Vector3 halfExtents;
				halfExtents.y =
					regularCamera.nearClipPlane *
					Mathf.Tan(0.5f * Mathf.Deg2Rad * regularCamera.fieldOfView);
				halfExtents.x = halfExtents.y * regularCamera.aspect;
				halfExtents.z = 0f;
				return halfExtents;
			}
		}

		void OnValidate()
		{
			if (maxVerticalAngle < minVerticalAngle) { maxVerticalAngle = minVerticalAngle; }
		}

		public override void Awake()
		{
			base.Awake();
			regularCamera = GetComponent<Camera>();
			if (target) { focusPoint = target.position; }
			transform.localRotation = Quaternion.Euler(orbitAngles);
		}

		public override void LateUpdate()
		{
			base.LateUpdate();
			UpdateFocusPoint();
			Quaternion lookRotation;
			if (ManualRotation() || AutomaticRotation())
			{
				ConstrainAngles();
				lookRotation = Quaternion.Euler(orbitAngles);
			}
			else { lookRotation = transform.localRotation; }

			Vector3 lookDirection = lookRotation * Vector3.forward;
			Vector3 lookPosition = focusPoint - lookDirection * distance;

			Vector3 rectOffset = lookDirection * regularCamera.nearClipPlane;
			Vector3 rectPosition = lookPosition + rectOffset;
			Vector3 castFrom = targetPosition;
			Vector3 castLine = rectPosition - castFrom;
			float castDistance = castLine.magnitude;
			Vector3 castDirection = castLine / castDistance;

			bool hit = Physics.BoxCast(
				castFrom,
				CameraHalfExtends,
				castDirection,
				out RaycastHit hitInfo,
				lookRotation,
				castDistance,
				obstructionMask);
			if (hit)
			{
				rectPosition = castFrom + castDirection * hitInfo.distance;
				lookPosition = rectPosition - rectOffset;
			}

			transform.SetPositionAndRotation(lookPosition, lookRotation);

			RaiseOnUpdated();
		}

		void UpdateFocusPoint()
		{
			previousFocusPoint = focusPoint;
			if (target) { targetPosition = target.position; }
			if (focusRadius > 0f)
			{
				var distanceFromFocus = Vector3.Distance(targetPosition, focusPoint);
				var t = 1f;
				if (distanceFromFocus > 0.01f && focusCentering > 0f)
				{
					t = Mathf.Pow(1f - focusCentering, Time.unscaledDeltaTime);
				}

				if (distanceFromFocus > focusRadius)
				{
					t = Mathf.Min(t, focusRadius / distanceFromFocus);
				}

				focusPoint = Vector3.Lerp(targetPosition, focusPoint, t);
			}
			else { focusPoint = targetPosition; }
		}

		bool ManualRotation()
		{
			var input = new Vector2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
			const float e = 0.001f;

			bool isInvalid(float value) => !(value < -e) && !(value > e);
			if (isInvalid(input.x) && isInvalid(input.y)) { return false; }

			orbitAngles += rotationSpeed * Time.unscaledDeltaTime * input;
			lastManualRotationTime = Time.unscaledTime;
			return true;
		}

		bool AutomaticRotation()
		{
			if (Time.unscaledTime - lastManualRotationTime < alignDelay) { return false; }

			Vector2 movement
				= new Vector2(
					focusPoint.x - previousFocusPoint.x,
					focusPoint.z - previousFocusPoint.z);
			var movementDeltaSqr = movement.sqrMagnitude;
			if (movementDeltaSqr < 0.0001f) { return false; }

			var headingAngle = GetAngle(movement / Mathf.Sqrt(movementDeltaSqr));
			var deltaAbs = Mathf.Abs(Mathf.DeltaAngle(orbitAngles.y, headingAngle));
			var rotationChange =
				rotationSpeed * Mathf.Min(Time.unscaledDeltaTime, movementDeltaSqr);
			if (deltaAbs < alignSmoothRange)
			{
				rotationChange *= deltaAbs / alignSmoothRange;
			}
			else if (180f - deltaAbs < alignSmoothRange)
			{
				rotationChange *= (180f - deltaAbs) / alignSmoothRange;
			}

			orbitAngles.y = Mathf.MoveTowardsAngle(orbitAngles.y, headingAngle, rotationChange);
			return true;
		}

		void ConstrainAngles()
		{
			orbitAngles.x = Mathf.Clamp(orbitAngles.x, minVerticalAngle, maxVerticalAngle);

			if (orbitAngles.y < 0f) { orbitAngles.y += 360f; }
			else if (orbitAngles.y >= 360f) { orbitAngles.y -= 360f; }
		}

		static float GetAngle(Vector2 direction)
		{
			var angle = Mathf.Acos(direction.y) * Mathf.Rad2Deg;
			return direction.x < 0f ? 360f - angle : angle;
		}
	}
}