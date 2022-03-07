using GameBrains.Extensions.MonoBehaviours;
using UnityEngine;

namespace GameBrains.GUI
{
    public abstract class WindowManager : ExtendedMonoBehaviour
    {
        // The next available window id.
        protected static int nextWindowId;

        // The id for the behaviour's main (or only) window.
        [SerializeField] protected int windowId;

        [SerializeField] protected Vector2 positionOffset = new Vector2(0f, 50f);
        [SerializeField] protected Vector2 minimumWindowSize = new Vector2(150f, 0f);
        [SerializeField] protected int minimumColumnWidth = 120;
        [SerializeField] protected float indent = 15f;

        [SerializeField] protected HorizontalAlignment horizontalAlignment;
        [SerializeField] protected VerticalAlignment verticalAlignment;

        [SerializeField] protected Color defaultTitleColor;
        [SerializeField] protected Color defaultContentColor;

        protected GUIStyle titleStyle;
        protected Rect windowRectangle;
        protected string windowTitle;
        protected int screenWidth;
        protected int screenHeight;

        public enum HorizontalAlignment { Left, Middle, Right }

        public enum VerticalAlignment { Top, Middle, Bottom }

        // If the behaviour has additional windows, store the ids in the behaviour
        // and initialize it in Start with:
        // Example:
        // private int anotherWindowId;
        //
        // protected override void Start()
        // {
        //     base.Start();
        //     anotherWindowId = nextWindowId++;
        //     // etc.
        // }

        public override void Start() { windowId = nextWindowId++; }

        protected virtual void OnGUI()
        {
            //if (PauseManager.IsPaused) { return; }
            
            var zoomWidth = Screen.width/ 1920f;
            var zoomHeight = Screen.height/ 1080f;

            // Scale for resolution.
            UnityEngine.GUI.matrix = Matrix4x4.TRS(
                Vector3.zero,
                Quaternion.AngleAxis(0f, new Vector3(0f, 1f, 0f)),
                new Vector3(zoomHeight, zoomWidth, 1f));

            if (screenWidth != Screen.width || screenHeight != Screen.height)
            {
                SetTitleStyle();
                SetMinimumWindowSize();
                SetPositionOffset();

                float x = 0;
                switch (horizontalAlignment)
                {
                    case HorizontalAlignment.Left:
                        x = Screen.width / zoomWidth * 0.02f + positionOffset.x;
                        break;
                    case HorizontalAlignment.Middle:
                        x = Screen.width / zoomWidth * 0.50f -
                            positionOffset.x -
                            minimumWindowSize.x / 2f;
                        break;
                    case HorizontalAlignment.Right:
                        x = Screen.width / zoomWidth * 0.98f -
                            positionOffset.x -
                            minimumWindowSize.x;
                        break;
                }

                float y = 0;
                switch (verticalAlignment)
                {
                    case VerticalAlignment.Top:
                        y = Screen.height / zoomHeight * 0.02f + positionOffset.y;
                        break;
                    case VerticalAlignment.Middle:
                        y = Screen.height / zoomHeight * 0.50f -
                            positionOffset.y -
                            minimumWindowSize.y / 2f;
                        break;
                    case VerticalAlignment.Bottom:
                        y = Screen.height / zoomHeight * 0.98f -
                            positionOffset.y -
                            minimumWindowSize.y;
                        break;
                }

                screenWidth = Screen.width;
                screenHeight = Screen.height;
                // If height is zero, GUILayout will determine height.
                windowRectangle = new Rect(x, y, minimumWindowSize.x, minimumWindowSize.y);
            }

            Color savedColor = UnityEngine.GUI.color;
            UnityEngine.GUI.color = defaultTitleColor;

            windowRectangle =
                GUILayout.Window(
                    windowId,
                    windowRectangle,
                    WindowFunction,
                    windowTitle,
                    titleStyle,
                    GUILayout.MinWidth(minimumWindowSize.x),
                    GUILayout.MinHeight(minimumWindowSize.y));

            UnityEngine.GUI.color = savedColor;
        }

        // This creates the GUI inside the window.
        // It requires the id of the window it's currently making GUI for.
        protected abstract void WindowFunction(int windowID);

        protected virtual void SetTitleStyle()
        {
            titleStyle = new GUIStyle(UnityEngine.GUI.skin.GetStyle("Window"))
            {
                fontSize = 16
            };
        }

        // Override in derived class to dynamically change WindowSize
        protected virtual void SetMinimumWindowSize() { }

        // Override in derived class to dynamically change PositionOffset
        protected virtual void SetPositionOffset() { }
    }
}