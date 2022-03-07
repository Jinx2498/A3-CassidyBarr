using System.Collections;
using GameBrains.Extensions.MonoBehaviours;
using GameBrains.Visualization;
using UnityEngine;

namespace Testing
{
    public abstract class W06TestCastVisualizer : ExtendedMonoBehaviour
    {
        [SerializeField] int numberOfVisualizers = 1;
        CastVisualizer[] visualizers;

        public override void OnEnable()
        {
            base.OnEnable();

            visualizers = new CastVisualizer[numberOfVisualizers];

            for (var index = 0; index < visualizers.Length; index++)
            {
                DoCreateVisualizer(index);
            }

            StartCoroutine(Visualize());
        }

        protected virtual void DoCreateVisualizer(int index)
        {
            visualizers[index] = CreateInstance();

            // Test setting color.
            visualizers[index].SetColor(index % 2 == 0 ? Color.cyan : Color.blue);

            // Test altering default hideFlags.
            var visualizerObject = visualizers[index].VisualizerObject;
            visualizerObject.hideFlags
                = index % 2 == 0
                    ? visualizerObject.hideFlags | HideFlags.HideInHierarchy
                    : visualizerObject.hideFlags & ~HideFlags.HideInHierarchy;
        }

        protected abstract CastVisualizer CreateInstance();

        public override void OnDisable()
        {
            base.OnDisable();

            StopCoroutine(Visualize());

            for (var index = 0; visualizers != null && index < visualizers.Length; index++)
            {
                visualizers[index] = null;
            }
            visualizers = null;
        }

        protected virtual IEnumerator Visualize()
        {
            while (!Input.GetKey(KeyCode.Escape))
            {
                DoDrawVisualizer();
                yield return new WaitForSeconds(1);
            }
        }

        protected virtual void DoDrawVisualizer()
        {
            foreach (var visualizer in visualizers)
            {
                var start = new Vector3(0, 0.08f, 0); // raise a little to avoid ground
                var randomAngle = Random.Range(0f, Mathf.PI * 2f);
                var direction // keep it parallel to the ground
                    = new Vector3(Mathf.Sin(randomAngle), 0, Mathf.Cos(randomAngle)).normalized;
                var length = Random.Range(10f, 20f);
                visualizer.Draw(start, direction, length);
            }
        }
    }
}