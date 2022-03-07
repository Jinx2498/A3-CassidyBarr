using UnityEngine;

namespace GameBrains.Cameras
{
    [AddComponentMenu("Scripts/GameBrains/Cameras/Simple Follow Camera")]
    [RequireComponent(typeof(Camera))]
    public sealed class SimpleFollow : TargetedCamera
    {
        // The distance in the x-z plane to the target.
        [SerializeField] float distance = 7.0f;

        // The height we want the camera to be above the target.
        [SerializeField] float height = 3.0f;

        public override void LateUpdate()
        {
            base.LateUpdate();

            if (!Target) { return; }

            // Convert the angle into a rotation, by which we then reposition the camera.
            var thisRotation = Quaternion.Euler(0f, Target.eulerAngles.y, 0f);

            // Set the position of the camera on the x-z plane to:
            // distance meters behind the target.

            var thisPosition = Target.position;
            thisPosition += thisRotation * Vector3.back * distance;

            // Set the height of the camera.
            thisPosition.y += height;
            transform.position = thisPosition;

            // Always look at the target.
            transform.LookAt(Target);

            RaiseOnUpdated();
        }
    }
}