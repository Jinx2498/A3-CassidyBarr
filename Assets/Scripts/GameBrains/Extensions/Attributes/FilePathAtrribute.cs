using System;
using UnityEditorInternal;
using UnityEngine;

// An attribute that specifies a file location relative to the Project folder or Unity's
// preferences folder.
namespace GameBrains.Extensions.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class FilePathAttribute : Attribute
    {
        string filePath;
        string relativePath;
        Location location;

        public string Filepath
        {
            get
            {
                if (filePath != null || relativePath == null) { return filePath; }

                filePath = CombineFilePath(relativePath, location);
                relativePath = null;

                return filePath;
            }
        }

        public FilePathAttribute(string relativePath, Location location)
        {
            this.relativePath = !string.IsNullOrEmpty(relativePath)
                ? relativePath
                : throw new ArgumentException("Invalid relative path (it is empty)");
            this.location = location;
        }

        static string CombineFilePath(string relativePath, Location location)
        {
            if (relativePath[0] == '/') { relativePath = relativePath.Substring(1); }

            switch (location)
            {
                case Location.PreferencesFolder:
                    return InternalEditorUtility.unityPreferencesFolder + "/" + relativePath;
                case Location.ProjectFolder:
                    return relativePath;
                default:
                    Debug.LogError("Unhandled enum: " + location);
                    return relativePath;
            }
        }

        // Specifies the folder location that Unity uses together with the relative path provided
        // in the FilePathAttribute constructor.
        public enum Location
        {
            // Use this location to save a file relative to the preferences folder. Useful for
            // per-user files that are across all projects.
            PreferencesFolder,

            // Use this location to save a file relative to the Project Folder. Useful for
            // per-project files (not shared between projects).
            ProjectFolder,
        }
    }
}