using UnityEngine;

namespace GameBrains.Cameras
{
    /*
    This camera smooths out rotation around the y-axis and height.
    Horizontal Distance to the target is always fixed.

    There are many different ways to smooth the rotation but doing it this way gives you
    a lot of control over how the camera behaves.

    For every of those smoothed values we calculate the wanted value and the current value.
    Then we smooth it using the Lerp function.
    Then we apply the smoothed values to the transform's position.
    */

    [AddComponentMenu("Scripts/GameBrains/Cameras/Smooth Follow Camera")]
    [RequireComponent(typeof(Camera))]
    public sealed class SmoothFollow : TargetedCamera
    {
        // The distance in the x-z plane to the target.
        [SerializeField] float distance = 7.0f;

        // The height we want the camera to be above the target.
        [SerializeField] float height = 3.0f;

        [SerializeField] float heightDamping = 2.0f;
        [SerializeField] float rotationDamping = 3.0f;

        public override void LateUpdate()
        {
            base.LateUpdate();

            // Early out if we don't have a target,
            if (!Target) { return; }

            // Calculate the current rotation angles.
            var wantedAngle = Target.eulerAngles.y;
            var targetPosition = Target.position;
            var wantedHeight = targetPosition.y + height;

            var thisTransform = transform;
            var thisAngle = thisTransform.eulerAngles.y;
            var thisPosition = thisTransform.position;
            var thisHeight = thisPosition.y;

            // Damp the rotation around the y-axis.
            thisAngle = Mathf.LerpAngle(thisAngle, wantedAngle, rotationDamping * Time.deltaTime);

            // Damp the height.
            thisHeight = Mathf.Lerp(thisHeight, wantedHeight, heightDamping * Time.deltaTime);

            // Convert the angle into a rotation.
            var thisRotation = Quaternion.Euler(0f, thisAngle, 0f);

            // Set the position of the camera on the x-z plane to:
            // distance meters behind the target.
            thisPosition = targetPosition;
            thisPosition += thisRotation * Vector3.back * distance;

            // Set the height of the camera.
            thisPosition.y = thisHeight;
            transform.position = thisPosition;

            // Always look at the target.
            transform.LookAt(Target);

            RaiseOnUpdated();
        }
    }
}