using UnityEngine;

namespace GameBrains.Cameras
{
    [AddComponentMenu("Scripts/GameBrains/Cameras/Overhead Zoom And Pan Camera")]
    [RequireComponent(typeof(Camera))]
    public sealed class OverheadZoomAndPan : SelectableCamera
    {
        [SerializeField] Camera overheadCamera;

        //TODO-3: Convert to new Input System
        [SerializeField] string zoomAxis = "Mouse ScrollWheel";
        [SerializeField] string sideMoveAxis = "Horizontal";
        [SerializeField] string forwardMoveAxis = "Vertical";
        [SerializeField] KeyCode homeKey = KeyCode.Tab;
        [SerializeField] float moveSpeed = 0.02f;
        Vector3 homePosition;
        float maximumZoomOut;

        public override void Awake()
        {
            base.Awake();
            maximumZoomOut = overheadCamera.orthographicSize;
            homePosition = overheadCamera.transform.position;
        }

        public override void LateUpdate()
        {
            base.LateUpdate();

            var zoomDirection = Input.GetAxis(zoomAxis);
            var size = overheadCamera.orthographicSize;

            if (zoomDirection < 0f) { size /= 1f - 0.01f * zoomDirection; }
            else if (zoomDirection > 0f) { size *= 1f + 0.01f * zoomDirection; }

            size = overheadCamera.orthographicSize = Mathf.Clamp(size, 1f, maximumZoomOut);

            var moveX = Input.GetAxis(sideMoveAxis);
            var moveY = Input.GetAxis(forwardMoveAxis);

            var position = overheadCamera.transform.position;
            position.x += moveX * moveSpeed * size;
            position.z += moveY * moveSpeed * size;

            overheadCamera.transform.position = Input.GetKeyDown(homeKey) ? homePosition : position;

            RaiseOnUpdated();
        }
    }
}