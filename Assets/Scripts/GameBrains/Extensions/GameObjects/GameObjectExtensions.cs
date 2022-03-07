using UnityEngine;

namespace GameBrains.Extensions.GameObjects
{
    public static class GameObjectExtensions
    {
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            return gameObject.TryGetComponent(out T requestedComponent)
                ? requestedComponent
                : gameObject.AddComponent<T>();
        }
    }
}