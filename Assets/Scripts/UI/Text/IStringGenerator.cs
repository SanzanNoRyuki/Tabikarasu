using UnityEngine;

namespace Miscellaneous.Quotes
{
    /// <summary>
    /// Generates string values.
    /// </summary>
    public abstract class Generator : MonoBehaviour
    {
        /// <summary>
        /// <inheritdoc cref="IStringGenerator"/>
        /// </summary>
        /// <returns>Generated string.</returns>
        public abstract string GenerateString();
    }
}