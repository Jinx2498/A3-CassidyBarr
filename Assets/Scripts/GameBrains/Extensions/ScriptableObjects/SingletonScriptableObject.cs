using System.Collections.Generic;
using System.Linq;
using GameBrains.Extensions.Attributes;
using UnityEngine;

namespace GameBrains.Extensions.ScriptableObjects
{
    public class SingletonScriptableObject<TExtended>
        : ExtendedScriptableObject where TExtended : ExtendedScriptableObject
    {
        public static readonly HashSet<SingletonScriptableObject<TExtended>> InstanceHashSet
            = new HashSet<SingletonScriptableObject<TExtended>>();

        public static TSingleton Instance<TSingleton>()
            where TSingleton : SingletonScriptableObject<TExtended>
        {
            var hashedInstance = InstanceHashSet.OfType<TSingleton>().FirstOrDefault();

            if (!hashedInstance)
            {
                string filePath = GetFilePath();

                if (!string.IsNullOrEmpty(filePath))
                {
                    var sso = Resources.Load<TSingleton>(filePath);
                    UpdateHash(sso);
                }

                hashedInstance = InstanceHashSet.OfType<TSingleton>().FirstOrDefault();
                if (!hashedInstance)
                {
                    CreateInstance<TSingleton>().hideFlags = HideFlags.HideAndDontSave;
                }
            }

            return hashedInstance;
        }

        bool dontDestroyOnSceneChange;

        public bool DontDestroyOnSceneChange
        {
            get => dontDestroyOnSceneChange;
            set
            {
                dontDestroyOnSceneChange = value;
                if (value) { DontDestroyOnLoad(this); }
            }
        }

        public override void OnEnable() { UpdateHash(this); }

        static void UpdateHash(SingletonScriptableObject<TExtended> sso)
        {
            bool Match(SingletonScriptableObject<TExtended> singletonScriptableObject)
                => singletonScriptableObject.GetType().IsInstanceOfType(sso);

            if (sso.DontDestroyOnSceneChange && InstanceHashSet.Any(Match))
            {
                // This is a multiple object caused by DontDestroyOnLoad(...) function
                // and loading the starter scene.
                //DestroyImmediate(this.gameObject);
            }
            else
            {
                InstanceHashSet.RemoveWhere(Match);
                InstanceHashSet.Add(sso);
            }
        }

        protected static string GetFilePath()
        {
            foreach (object customAttribute in typeof(TExtended).GetCustomAttributes(true))
            {
                if (customAttribute is FilePathAttribute attribute) { return attribute.Filepath; }
            }

            return string.Empty;
        }
    }
}