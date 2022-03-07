using GameBrains.Visualization;
using UnityEngine;

namespace Testing
{
    [AddComponentMenu("Scripts/Testing/W06 Test LineCast Checker")]
    public class W06TestLineCastChecker : W06TestCastChecker
    {
        public override string CheckerName => "LineCast";

        protected override CastChecker CreateInstance()
        {
            return ScriptableObject.CreateInstance<LineCastChecker>();
        }
    }
}