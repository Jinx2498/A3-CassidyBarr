using GameBrains.Visualization;
using UnityEngine;

namespace Testing
{
    [AddComponentMenu("Scripts/Testing/W06 Test CapsuleCast Checker")]
    public class W06TestCapsuleCastChecker : W06TestCastChecker
    {
        public override string CheckerName => "CapsuleCast";

        protected override CastChecker CreateInstance()
        {
            return ScriptableObject.CreateInstance<CapsuleCastChecker>();
        }
    }
}