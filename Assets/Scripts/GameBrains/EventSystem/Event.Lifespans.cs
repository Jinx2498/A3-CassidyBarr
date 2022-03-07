using System.ComponentModel;

namespace GameBrains.EventSystem
{
    public abstract partial class Event
    {
		public static partial class Lifespans
        {
            [Description("Cycle")]
            public static readonly Lifespan Cycle = (Lifespan)Count++;

            [Description("Level")]
            public static readonly Lifespan Level = (Lifespan)Count++;

            [Description("Game")]
            public static readonly Lifespan Game = (Lifespan)Count++;

            public static int Count { get; private set; }

            public static string GetDescription(Lifespan lifespan)
            {
                foreach (var fieldInfo in typeof(Lifespans).GetFields())
                {
                    if ((Lifespan)fieldInfo.GetValue(null) != lifespan)
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

                return lifespan.ToString();
            }
        }
	}
}
