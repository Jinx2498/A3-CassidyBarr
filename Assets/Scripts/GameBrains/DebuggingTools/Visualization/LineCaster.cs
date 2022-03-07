using GameBrains.Extensions;
using GameBrains.Extensions.MonoBehaviours;
using GameBrains.Visualization;
using UnityEngine;

namespace GameBrains.DebuggingTools.Visualization
{
    public class LineCaster : ExtendedMonoBehaviour
    {
        public LineCastVisualizer visualizer;
        public Transform to;
        public bool drawIt;

        public override void OnEnable()
        {
            base.OnEnable();
            if (visualizer) { return; }
            visualizer = ScriptableObject.CreateInstance<LineCastVisualizer>();
        }

        public override void OnDisable()
        {
            base.OnDisable();
            if (!visualizer) { return; }
            visualizer.CheckAndDestroy();
            visualizer = null;
        }

        public override void Update()
        {
            base.Update();
            if (!drawIt) { return; }
            drawIt = false;
            visualizer.Draw(transform.position, to.position);
        }
    }
}