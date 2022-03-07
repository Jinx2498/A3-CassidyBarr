using GameBrains.Cameras;
using GameBrains.Entities;
using UnityEngine;

namespace GameBrains.GUI
{
    [AddComponentMenu("Scripts/GameBrains/GUI/Entity Console")]
    public class EntityConsole : MessageViewer
    {
        [SerializeField] Entity entity;
        [SerializeField] int commandColumnsPerRow = 3;
        bool linesForButtonsAdded;

        public override void Awake()
        {
            base.Awake();

            if (!entity) { entity = GetComponent<Entity>(); }
        }

        public override void Start()
        {
            base.Start();

            if (entity)
            {
                var entityColor = ColorUtility.ToHtmlStringRGBA(entity.Color);
                windowTitle = $"<color=#{entityColor}>{entity.ShortName}'s Console</color>";
            }
            else
            {
                windowTitle = $"{gameObject.name}'s Console";
            }
        }

        protected override void SetMinimumWindowSize()
        {
            base.SetMinimumWindowSize();

            // Add lines for buttons.
            if (!linesForButtonsAdded && SelectableCamera.SelectableCameras != null)
            {
                linesForButtonsAdded = true;
                
                minimumWindowSize.y
                    += SelectableCamera.SelectableCameras.Count *
                       (UnityEngine.GUI.skin.button.lineHeight);
            }
        }

        // This creates the GUI inside the window.
        // It requires the id of the window it's currently making GUI for.
        protected override void WindowFunction(int windowID)
        {
            // Purposely not calling base.WindowFunction here.

            // Draw any Controls inside the window here.

            Color savedColor = UnityEngine.GUI.color;
            UnityEngine.GUI.color = defaultContentColor;

            if (SelectableCamera.SelectableCameras != null &&
                SelectableCamera.SelectableCameras.Count > 0)
            {
                GUILayout.BeginVertical();

                int commandIndex = 0;

                while (commandIndex < SelectableCamera.SelectableCameras.Count)
                {
                    GUILayout.BeginHorizontal();

                    int commandColumn = 0;

                    while (commandColumn < commandColumnsPerRow &&
                           commandIndex < SelectableCamera.SelectableCameras.Count)
                    {
                        var selectableCamera = SelectableCamera.SelectableCameras[commandIndex];
                        if (GUILayout.Button(selectableCamera.DisplayName))
                        {
                            SelectableCamera.SetCurrent(selectableCamera);

                            if (SelectableCamera.CurrentCamera is TargetedCamera targetedCamera)
                            {
                                targetedCamera.Target = transform;
                            }
                        }

                        commandColumn++;
                        commandIndex++;
                    }

                    GUILayout.EndHorizontal();
                }

                GUILayout.EndVertical();
            }

            GUILayout.Label(messageQueue.GetMessages());

            UnityEngine.GUI.color = savedColor;

            // Make the windows be draggable.
            UnityEngine.GUI.DragWindow();
        }
    }
}