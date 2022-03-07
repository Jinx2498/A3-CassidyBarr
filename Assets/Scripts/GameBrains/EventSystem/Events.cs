using System.ComponentModel;

namespace GameBrains.EventSystem
{
    public static partial class Events
    {
        public static int Count { get; private set; }

        public static string GetDescription(EventType eventType)
        {
            foreach (var fieldInfo in typeof(Events).GetFields())
            {
                if ((EventType)fieldInfo.GetValue(null) != eventType)
                {
                    continue;
                }

                DescriptionAttribute[] attributes =
                    (DescriptionAttribute[])fieldInfo.GetCustomAttributes(
                        typeof(DescriptionAttribute),
                        false);

                if (attributes.Length > 0)
                {
                    return attributes[0].Description;
                }
            }

            return eventType.ToString();
        }
    }
}