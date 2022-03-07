using UnityEngine;

namespace GameBrains.GUI
{
    [AddComponentMenu("Scripts/GameBrains/GUI/FPS Viewer")]

    // This calculates frames/second over each updateInterval,
    // so the display does not keep changing wildly.
    //
    // It is also fairly accurate at very low FPS counts (<10).
    // We do this not by simply counting frames per interval, but
    // by accumulating FPS for each frame. This way we end up with
    // correct overall FPS even if the interval renders something
    // like 5.5 frames.
    public class FpsViewer : WindowManager
    {
        public bool showFps = true;
        public float updateInterval = 0.5f;

        protected float fps;
        protected float accumulatedFps; // FPS accumulated over the interval.
        protected int frames; // Frames drawn over the interval.
        protected float timeLeft; // Time left for current interval.

        public override void Start()
        {
            base.Start(); // Initializes the window id.
            timeLeft = updateInterval;
            windowTitle = "FPS";
        }

        public override void Update()
        {
            base.Update();

            timeLeft -= Time.deltaTime;
            accumulatedFps += Time.timeScale / Time.deltaTime;
            ++frames;

            // Interval ended - update fps and start new interval.
            if (!(timeLeft <= 0.0f)) { return; }

            fps = accumulatedFps / frames;
            timeLeft = updateInterval;
            accumulatedFps = 0.0f;
            frames = 0;
        }

        // This creates the GUI inside the window.
        // It requires the id of the window it's currently making GUI for.
        protected override void WindowFunction(int windowID)
        {
            // Draw any Controls inside the window here.

            Color savedColor = UnityEngine.GUI.color;
            UnityEngine.GUI.color = defaultContentColor;

            if (showFps) { GUILayout.Label($"{fps:f1}"); }

            UnityEngine.GUI.color = savedColor;

            // Make the windows be draggable.
            UnityEngine.GUI.DragWindow();
        }
    }
}