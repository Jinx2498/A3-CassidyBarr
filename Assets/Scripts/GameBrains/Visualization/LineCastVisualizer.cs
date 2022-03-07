using UnityEditor;
using UnityEngine;

namespace GameBrains.Visualization
{
    #if UNITY_EDITOR
    [FilePath(
        "ScriptableObjects/Visualizers/LineCastVisualizer",
        FilePathAttribute.Location.ProjectFolder)]
    [CreateAssetMenu(
        fileName = "LineCastVisualizer",
        menuName = "GameBrains/Visualization/LineCastVisualizer")]
    #endif
    public class LineCastVisualizer : CastVisualizer
    {
        public LineRenderer LineRenderer => lineRenderer;
        LineRenderer lineRenderer;

        public override void OnDestroy()
        {
            base.OnDestroy();
            lineRenderer = null;
            visualizerObject = null;
        }

        public override void Hide(bool shouldHide)
        {
            if (lineRenderer) { lineRenderer.enabled = !shouldHide; }
        }

        public override void SetColor(Color color)
        {
            if (lineRenderer) { lineRenderer.material.color = color; }
        }

        public override void Draw(Vector3 startPosition, Vector3 direction, float length)
        {
            Vector3 endPosition = startPosition + direction * length;

            if (!lineRenderer) { return; }
            Hide(Vector3.Distance(startPosition, endPosition) < hideThreshold);
            lineRenderer.SetPosition(0, startPosition);
            lineRenderer.SetPosition(1, endPosition);
        }

        protected override void Create()
        {
            visualizerPrefab
                = Resources.Load<GameObject>("Prefabs/Visualization/LineCastVisualizerPrefab");
            visualizerObject = Instantiate(visualizerPrefab);
            visualizerObject.name = "LineCastVisualizer";
            visualizerObject.hideFlags = HideFlags.HideInHierarchy;
            lineRenderer = visualizerObject.GetComponent<LineRenderer>();
        }
    }
}