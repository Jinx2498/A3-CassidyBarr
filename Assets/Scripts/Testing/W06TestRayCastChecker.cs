using GameBrains.Visualization;
using UnityEngine;

namespace Testing
{
    [AddComponentMenu("Scripts/Testing/W06 Test RayCast Checker")]
    public class W06TestRayCastChecker : W06TestCastChecker
    {
        public override string CheckerName => "RayCast";

        protected override CastChecker CreateInstance()
        {
            return ScriptableObject.CreateInstance<RayCastChecker>();
        }
    }
}