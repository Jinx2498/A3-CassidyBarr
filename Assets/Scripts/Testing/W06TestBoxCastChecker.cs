using GameBrains.Visualization;
using UnityEngine;

namespace Testing
{
    [AddComponentMenu("Scripts/Testing/W06 Test BoxCast Checker")]
    public class W06TestBoxCastChecker : W06TestCastChecker
    {
        public override string CheckerName => "BoxCast";

        protected override CastChecker CreateInstance()
        {
            return ScriptableObject.CreateInstance<BoxCastChecker>();
        }
    }
}