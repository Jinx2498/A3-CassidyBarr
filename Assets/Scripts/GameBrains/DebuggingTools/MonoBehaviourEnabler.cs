using UnityEngine;

namespace GameBrains.DebuggingTools
{
    // This is for re-enabling scripts that were disabled because of an exception thrown in Awake.
    [ExecuteAlways]
    [AddComponentMenu("Scripts/GameBrains/DebuggingTools/MonoBehaviour Enabler")]
    public class MonoBehaviourEnabler : MonoBehaviour
    {
        public bool enableAll;

        public void Update()
        {
            if (!enableAll || Application.isPlaying) { return; }

            enableAll = false;

            var monoBehaviours = FindObjectsOfType<MonoBehaviour>();

            foreach (MonoBehaviour monoBehaviour in monoBehaviours)
            {
                if (!monoBehaviour.enabled)
                {
                    monoBehaviour.enabled = true;
                }
            }
        }
    }
}