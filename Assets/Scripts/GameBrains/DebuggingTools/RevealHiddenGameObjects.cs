using UnityEngine;

namespace GameBrains.DebuggingTools
{
    // Use this to reveal game objects hidden in the hierarchy.
    [ExecuteAlways]
    [AddComponentMenu("Scripts/GameBrains/DebuggingTools/Reveal Hidden GameObjects")]
    public class RevealHiddenGameObjects : MonoBehaviour
    {
        void Update ()
        {
            var gameObjects = FindObjectsOfType<GameObject>();
            foreach (GameObject go in gameObjects) { go.hideFlags &= ~HideFlags.HideInHierarchy; }
        }
    }
}