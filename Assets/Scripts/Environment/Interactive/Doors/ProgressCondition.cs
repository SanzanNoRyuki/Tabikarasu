using UnityEngine;

namespace Environment.Interactive.Doors
{
    public abstract class ProgressCondition : MonoBehaviour
    {
        /// <summary>
        /// Condition to be satisfied.
        /// </summary>
        /// <returns>Returns true if condition is met.</returns>
        public abstract bool ConditionMet();
    }
}