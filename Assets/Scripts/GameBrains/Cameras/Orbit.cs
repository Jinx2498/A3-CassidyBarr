using GameBrains.Extensions.MathExtensions;
using UnityEngine;

namespace GameBrains.Cameras
{
    [AddComponentMenu("Scripts/GameBrains/Cameras/Orbit Camera")]
    [RequireComponent(typeof(Camera))]
    public sealed class Orbit : TargetedCamera
    {
        // Whether the camera is controllable by the player.
        [SerializeField] bool isControllable = true;

        // The axes used to control the camera orientation.
        [SerializeField] string sideLookAxis = "Mouse X";
        [SerializeField] string verticalAxis = "Mouse Y";

        [SerializeField] Vector3 targetOffset = new Vector3(0f, 2f, 0f);
        [SerializeField] float distance = 4.0f;

        [SerializeField] LayerMask lineOfSightMask = 0;
        [SerializeField] float closerRadius = 0.2f;
        [SerializeField] float closerSnapLag = 0.2f;

        [SerializeField] float xSpeed = 200.0f;
        [SerializeField] float ySpeed = 80.0f;

        [SerializeField] float yMinLimit = -20;
        [SerializeField] float yMaxLimit = 80;

        float currentDistance = 10.0f;
        float distanceVelocity;
        float x;
        float y;

        bool active; // If active is set, Axes are polled.

        public override void Start()
        {
            base.Start();

            Vector3 angles = transform.eulerAngles;
            x = angles.y;
            y = angles.x;
            currentDistance = distance;

            // Keep the rigid body from changing rotation.
            if (GetComponent<Rigidbody>()) { GetComponent<Rigidbody>().freezeRotation = true; }
        }

        public override void Update()
        {
            base.Update();

            active = Input.GetMouseButton(0);
        }

        public override void LateUpdate()
        {
            base.LateUpdate();

            if (!Target) { return; }

            if (isControllable)
            {
                if (active)
                {
                    x += Input.GetAxis(sideLookAxis) * xSpeed * 0.02f;
                    y -= Input.GetAxis(verticalAxis) * ySpeed * 0.02f;
                }
            }
            else
            {
                // TODO: Add ability for AI control?
                x = 0;
                y = 0;
            }

            y = Math.ClampAngle(y, yMinLimit, yMaxLimit);

            Quaternion rotation = Quaternion.Euler(y, x, 0f);
            Vector3 targetPosition = Target.position + targetOffset;
            Vector3 direction = rotation * -Vector3.forward;

            var targetDistance = AdjustLineOfSight(targetPosition, direction);
            currentDistance
                = Mathf.SmoothDamp(
                    currentDistance,
                    targetDistance,
                    ref distanceVelocity,
                    closerSnapLag);

            transform.rotation = rotation;
            transform.position = targetPosition + direction * currentDistance;

            RaiseOnUpdated();
        }

        float AdjustLineOfSight(Vector3 losTarget, Vector3 direction)
        {
            if (Physics.Raycast(
                losTarget,
                direction,
                out RaycastHit hit,
                distance,
                lineOfSightMask))
                return hit.distance - closerRadius;

            return distance;
        }
    }
}