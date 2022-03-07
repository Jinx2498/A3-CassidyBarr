using UnityEditor;
using UnityEngine;

namespace GameBrains.Editor.Tools
{
    public class EditorTools : UnityEditor.Editor
    {
        [MenuItem("Tools/Reset Player Prefs")]
        public static void ResetPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("<b> **** Player Prefs Deleted **** </b>");
        }
    }
}