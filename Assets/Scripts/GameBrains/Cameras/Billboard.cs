using GameBrains.Extensions.MonoBehaviours;
using UnityEngine;

namespace GameBrains.Cameras
{
    public class Billboard : ExtendedMonoBehaviour
    {
        public override void OnEnable()
        {
            base.OnEnable();

            SelectableCamera.OnUpdated += HandleCameraUpdate;
        }

        public override void OnDisable()
        {
            base.OnDisable();

            SelectableCamera.OnUpdated -= HandleCameraUpdate;
        }

        public void HandleCameraUpdate(SelectableCamera updatedCamera)
        {
            var updatedCameraTransform = updatedCamera.transform;
            var rotation = updatedCameraTransform.rotation;
            transform.LookAt(transform.position + rotation * Vector3.forward, rotation * Vector3.up);
        }
    }
}