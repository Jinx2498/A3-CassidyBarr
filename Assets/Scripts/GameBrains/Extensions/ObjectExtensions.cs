using UnityEngine;

namespace GameBrains.Extensions
{
    public static class ObjectExtensions
    {
        // Use this to destroy objects when using [ExecuteAlways].
        public static void CheckAndDestroy(this Object objectToDestroy, float delay = 0f)
        {
            if (Application.isPlaying) { Object.Destroy(objectToDestroy, delay); }
            else { Object.DestroyImmediate(objectToDestroy); }
        }
    }
}