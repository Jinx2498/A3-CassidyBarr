using GameBrains.Extensions;
using GameBrains.Extensions.MonoBehaviours;
using GameBrains.Extensions.ScriptableObjects;
using UnityEngine;

namespace Testing
{
    public class W06TestExtendedMonoBehaviourLogging : ExtendedMonoBehaviour
    {
        TestExtendedScriptableObjectLogging so;

        public override void Awake()
        {
            base.Awake();
            // Note that Awake executes even if script not enabled.
            if (enabled) { Log.Debug("MonoBehaviour Awake"); }
        }

        public override void OnEnable()
        {
            base.OnEnable();
            Log.Debug("MonoBehaviour OnEnable");
            so = ScriptableObject.CreateInstance<TestExtendedScriptableObjectLogging>();
        }

        public override void OnDisable()
        {
            base.OnDisable();
            Log.Debug("MonoBehaviour OnDisable");
            so.CheckAndDestroy();
            so = null;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            // Note that OnDestroy executes even if script not enabled.
            if (enabled) { Log.Debug("MonoBehaviour OnDestroy"); }
        }
    }

    public class TestExtendedScriptableObjectLogging : ExtendedScriptableObject
    {
        public override void Awake()
        {
            base.Awake();
            Log.Debug("ScriptableObject Awake");
        }

        public override void OnEnable()
        {
            base.OnEnable();
            Log.Debug("ScriptableObject OnEnable");
        }

        public override void OnDisable()
        {
            base.OnDisable();
            Log.Debug("ScriptableObject OnDisable");
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            Log.Debug("ScriptableObject OnDestroy");
        }
    }
}