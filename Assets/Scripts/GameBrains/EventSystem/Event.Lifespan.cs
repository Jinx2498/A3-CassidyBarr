namespace GameBrains.EventSystem
{
    public abstract partial class Event
    {
		public struct Lifespan
        {
            readonly int lifespan;

            public Lifespan(int lifespan)
                : this()
            {
                this.lifespan = lifespan;
            }

            public static implicit operator int(Lifespan lifespan)
            {
                return lifespan.lifespan;
            }

            public static explicit operator Lifespan(int lifespan)
            {
                return new Lifespan(lifespan);
            }

            public override string ToString()
            {
                return Lifespans.GetDescription(this);
            }
        }
    }
}
