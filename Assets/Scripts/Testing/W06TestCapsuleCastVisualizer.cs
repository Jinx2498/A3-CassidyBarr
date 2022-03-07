using GameBrains.Visualization;
using UnityEngine;

namespace Testing
{
    [AddComponentMenu("Scripts/Testing/W06 Test CapsuleCast Visualizer")]
    public class W06TestCapsuleCastVisualizer : W06TestCastVisualizer
    {
        protected override CastVisualizer CreateInstance()
        {
            return ScriptableObject.CreateInstance<CapsuleCastVisualizer>();
        }
    }
}