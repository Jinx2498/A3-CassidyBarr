using System;

namespace GameBrains.Extensions.Attributes
{
    public class HidePropertiesInInspectorAttribute : Attribute
    {
        public HidePropertiesInInspectorAttribute(params string[] hiddenProperties)
        {
            HiddenProperties = hiddenProperties;
        }

        public string[] HiddenProperties { get; }
    }
}