using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GameBrains.Extensions.Enumerations
{
    public abstract class Enumeration : IComparable
    {
        readonly int value;
        readonly string displayName;
        protected Enumeration() { }

        protected Enumeration(int value, string displayName)
        {
            this.value = value;
            this.displayName = displayName;
        }

        public int Value => value;

        public string DisplayName => displayName;

        public override string ToString() { return DisplayName; }

        public static IEnumerable<T> GetAll<T>() where T : Enumeration, new()
        {
            var type = typeof(T);
            // Look for the public static members declared in the derived class.
            // These will be the declared enumerations.
            var fields = type.GetFields(BindingFlags.Public |
                                        BindingFlags.Static |
                                        BindingFlags.DeclaredOnly);

            foreach (var info in fields)
            {
                var instance = new T();

                if (info.GetValue(instance) is T locatedValue)
                {
                    yield return locatedValue;
                }
            }
        }

        public override bool Equals(object obj)
        {
            var otherValue = obj as Enumeration;

            if (otherValue == null)
            {
                return false;
            }

            var typeMatches = GetType() == obj.GetType();
            var valueMatches = Value.Equals(otherValue.Value);

            return typeMatches && valueMatches;
        }

        public override int GetHashCode() { return Value.GetHashCode(); }

        public static int AbsoluteDifference(Enumeration firstValue, Enumeration secondValue)
        {
            var absoluteDifference = System.Math.Abs(firstValue.Value - secondValue.Value);
            return absoluteDifference;
        }

        public static T FromValue<T>(int value) where T : Enumeration, new()
        {
            var matchingItem = Parse<T, int>(value, "value", item => item.Value == value);
            return matchingItem;
        }

        public static T FromDisplayName<T>(string displayName) where T : Enumeration, new()
        {
            var matchingItem = Parse<T, string>(displayName, "display name",
                item => item.DisplayName == displayName);
            return matchingItem;
        }

        static T Parse<T, TK>(TK value, string description, Func<T, bool> predicate)
            where T : Enumeration, new()
        {
            var matchingItem = GetAll<T>().FirstOrDefault(predicate);

            if (matchingItem == null)
            {
                var message = $"'{value}' is not a valid {description} in {typeof(T)}";
                throw new ApplicationException(message);
            }

            return matchingItem;
        }

        public int CompareTo(object other) { return Value.CompareTo(((Enumeration) other).Value); }
    }
    
    // Example
    // CardType automatically assigns the next enumeration value, but you could specify desired values manually.
    // public partial class CardType : Enumeration
    // {
    //     static int nextID;
    //     static int NextID => nextID++;
    //     public CardType() : base(0, "") { }
    //
    //     public CardType(int id, string name) : base(id, name) { }
    // }
    // Using a partial class to allow specifying additional enumeration values in the future.
    // public partial class CardType
    // {
    //     public static readonly CardType Amex = new CardType(NextID, nameof(Amex));
    //     public static readonly CardType Visa = new CardType(NextID, nameof(Visa));
    //     public static readonly  CardType MasterCard = new CardType(NextID, nameof(MasterCard));
    // }
    // Here's an enumeration value added later.
    // public partial class CardType
    // {
    //     public static readonly CardType Discover = new CardType(NextID, nameof(Discover));
    // }
}