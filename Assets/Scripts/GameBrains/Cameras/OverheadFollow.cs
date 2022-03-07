using UnityEngine;

namespace GameBrains.Cameras
{
    [AddComponentMenu("Scripts/GameBrains/Cameras/Overhead Follow Camera")]
    [RequireComponent(typeof(Camera))]
    public sealed class OverheadFollow : TargetedCamera
    {
        [SerializeField] Camera overheadCamera;
        //TODO-3: Convert to new Input System
        [SerializeField] string zoomAxis = "Mouse ScrollWheel";
        float maximumZoomOut;

        public override void Awake()
        {
            base.Awake();
            maximumZoomOut = overheadCamera.orthographicSize;
        }
        public override void LateUpdate()
        {
            base.LateUpdate();

            var zoomDirection = Input.GetAxis(zoomAxis);
            var size = overheadCamera.orthographicSize;

            if (zoomDirection < 0f) { size /= 1f - 0.01f * zoomDirection; }
            else if (zoomDirection > 0f) { size *= 1f + 0.01f * zoomDirection; }

            overheadCamera.orthographicSize = Mathf.Clamp(size, 1f, maximumZoomOut);

            if (Target != null)
            {
                var cachedTransform = overheadCamera.transform;
                Vector3 position = Target.position;
                position.y = cachedTransform.position.y;
                cachedTransform.position = position;
            }

            RaiseOnUpdated();
        }
    }
}