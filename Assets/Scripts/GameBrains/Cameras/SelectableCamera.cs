using System;
using System.Collections.Generic;
using GameBrains.Extensions.MonoBehaviours;
using UnityEngine;

namespace GameBrains.Cameras
{
    public abstract class SelectableCamera : ExtendedMonoBehaviour
    {
        // Used by classes that need to update after the camera (Billboard and FadeOutLineOfSight).
        // This is essentially a LateLateUpdate callback.
        public static event Action<SelectableCamera> OnUpdated = delegate { };
        public static SelectableCamera CurrentCamera { get; set; }

        public string DisplayName => displayName;
        [SerializeField] protected string displayName;
        [SerializeField] protected bool defaultCamera;

        public static List<GameObject> SelectableCameraObjects => selectableCameraObjects;
        static List<GameObject> selectableCameraObjects;

        public static List<SelectableCamera> SelectableCameras => selectableCameras;

        static List<SelectableCamera> selectableCameras;

        public override void Awake()
        {
            base.Awake();

            selectableCameraObjects ??= new List<GameObject>();
            selectableCameras ??= new List<SelectableCamera>();

            if (!selectableCameraObjects.Contains(gameObject))
            {
                selectableCameraObjects.Add(gameObject);
            }

            selectableCameras.Add(this);

            if (defaultCamera || !CurrentCamera) { SetCurrent(this); }
        }

        public override void Start()
        {
            // At this point, all selectable cameras are available and current is set.
            // So, now we can safely disable the non-current camera gameObjects and scripts.
            if (this != CurrentCamera) { return; }

            foreach (GameObject cameraObject in SelectableCameraObjects)
            {
                if (cameraObject != CurrentCamera.gameObject) { cameraObject.SetActive(false); }
            }

            foreach (SelectableCamera selectableCamera in SelectableCameras)
            {
                if (selectableCamera != CurrentCamera) { selectableCamera.enabled = false; }
            }
        }

        public static void SetCurrent(SelectableCamera selectableCamera)
        {
            if (CurrentCamera)
            {
                CurrentCamera.gameObject.SetActive(false);
                CurrentCamera.enabled = false;
            }

            CurrentCamera = selectableCamera;
            selectableCamera.gameObject.SetActive(true);
            selectableCamera.enabled = true;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            CurrentCamera = null;
            selectableCameraObjects = null;
            selectableCameras = null;

        }

        protected void RaiseOnUpdated() => OnUpdated(this);
    }
}