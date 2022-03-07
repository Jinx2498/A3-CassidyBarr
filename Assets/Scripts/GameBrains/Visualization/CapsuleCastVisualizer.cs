using UnityEditor;
using UnityEngine;

namespace GameBrains.Visualization
{
    #if UNITY_EDITOR
    [FilePath(
        "ScriptableObjects/Visualizers/CapsuleCastVisualizer",
        FilePathAttribute.Location.ProjectFolder)]
    [CreateAssetMenu(
        fileName = "CapsuleCastVisualizer",
        menuName = "GameBrains/Visualization/CapsuleCastVisualizer")]
    #endif
    public class CapsuleCastVisualizer : CastVisualizer
    {
        public float castRadiusMultiplier = 1f;

        Transform prefabTop;
        Transform prefabMiddle;
        Transform prefabBottom;
        Transform prefabEnd;

        Transform top;
        Transform middle;
        Transform bottom;
        Transform end;

        Renderer topRenderer;
        Renderer middleRenderer;
        Renderer bottomRenderer;
        Renderer endRenderer;

        public override void OnDestroy()
        {
            base.OnDestroy();

            topRenderer = null;
            middleRenderer = null;
            bottomRenderer = null;
            endRenderer = null;

            top = null;
            middle = null;
            bottom = null;
            end = null;

            visualizerObject = null;
        }

        public override void Hide(bool shouldHide)
        {
            if (topRenderer) { topRenderer.enabled = !shouldHide; }
            if (middleRenderer) { middleRenderer.enabled = !shouldHide; }
            if (bottomRenderer) { bottomRenderer.enabled = !shouldHide; }
            if (endRenderer) { endRenderer.enabled = !shouldHide; }
        }

        public override void SetColor(Color color)
        {
            if (topRenderer) { topRenderer.material.color = color; }
            if (middleRenderer) { middleRenderer.material.color = color; }
            if (bottomRenderer) { bottomRenderer.material.color = color; }
            if (endRenderer) { endRenderer.material.color = color; }
        }

        public override void Draw(Vector3 startPosition, Vector3 direction, float length)
        {
            if (!visualizerObject) { return; }

            bool tooSmallToShow = length < hideThreshold;
            Hide(tooSmallToShow);
            if (tooSmallToShow) return;

            visualizerObject.transform.position = startPosition;
            Vector3 lengthDirection = length * direction;
            Vector3 halfLengthDirection = lengthDirection / 2f;

            top.localPosition = halfLengthDirection + Vector3.up * prefabTop.localPosition.y;
            middle.localPosition = halfLengthDirection + Vector3.up * prefabMiddle.localPosition.y;
            bottom.localPosition = halfLengthDirection + Vector3.up * prefabBottom.localPosition.y;
            end.localPosition = lengthDirection + Vector3.up * prefabEnd.localPosition.y;

            Vector3 scale = prefabEnd.localScale;
            scale.x *= castRadiusMultiplier;
            scale.z *= castRadiusMultiplier;
            end.localScale = scale;

            scale = prefabTop.localScale;
            scale.x *= castRadiusMultiplier;
            scale.y *= length;
            top.localScale = scale;

            scale = prefabMiddle.localScale;
            scale.z *= castRadiusMultiplier;
            scale.x *= length;
            middle.localScale = scale;

            scale = prefabBottom.localScale;
            scale.x *= castRadiusMultiplier;
            scale.y *= length;
            bottom.localScale = scale;

            top.LookAt(top.position + direction);
            top.transform.Rotate(Vector3.right, 90f);

            middle.LookAt(middle.position + direction);
            middle.transform.Rotate(Vector3.up, 90f);

            bottom.LookAt(bottom.position + direction);
            bottom.transform.Rotate(Vector3.right, 90f);

            end.LookAt(end.position + direction);
        }

        protected override void Create()
        {
            visualizerPrefab
                = Resources.Load<GameObject>("Prefabs/Visualization/CapsuleCastVisualizerPrefab");
            visualizerObject = Instantiate(visualizerPrefab);
            visualizerObject.name = "CapsuleCastVisualizer";
            visualizerObject.hideFlags = HideFlags.HideInHierarchy;

            prefabTop = visualizerPrefab.transform.Find("Top");
            prefabMiddle = visualizerPrefab.transform.Find("Middle");
            prefabBottom = visualizerPrefab.transform.Find("Bottom");
            prefabEnd = visualizerPrefab.transform.Find("End");

            top = visualizerObject.transform.Find("Top");
            middle = visualizerObject.transform.Find("Middle");
            bottom = visualizerObject.transform.Find("Bottom");
            end = visualizerObject.transform.Find("End");

            topRenderer = top.GetComponent<Renderer>();
            middleRenderer = middle.GetComponent<Renderer>();
            bottomRenderer = bottom.GetComponent<Renderer>();
            endRenderer = end.GetComponent<Renderer>();
        }
    }
}