using GameBrains.Extensions.MonoBehaviours;
using UnityEngine;

namespace GameBrains.PerformanceMeasures
{
    public class PerformanceCriteria : ExtendedMonoBehaviour
    {
        // How much do we care about this criteria?
        public float weightPercentage;

        // Keeping track of how many times a good thing happened,
        // and the total number of things that happened.
        // We can track the percentage of how often that happened
        [SerializeField] int totalNumberOfOccurrences = 0;
        [SerializeField] int totalNumberOfGoodOccurrences = 0;

        public void GoodOccurrenceHappened()
        {
            totalNumberOfOccurrences++;
            totalNumberOfGoodOccurrences++;
        }

        public void BadOccurrenceHappened()
        {
            totalNumberOfOccurrences++;
        }

        public void ResetCounter() {
            totalNumberOfOccurrences = 0;
            totalNumberOfGoodOccurrences = 0;
        }

        public float GetPercentageOfGoodOccurrences()
        {
            if (totalNumberOfOccurrences == 0)
            {
                return 0f;
            }

            return (float)totalNumberOfGoodOccurrences / totalNumberOfOccurrences;
        }
    }
}