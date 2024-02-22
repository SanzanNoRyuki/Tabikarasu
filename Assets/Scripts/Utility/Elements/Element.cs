using System;

namespace Elements
{
    /// <summary>
    /// Classical element.
    /// </summary>
    /// <remarks>
    /// Cast to <see cref="Elements"/> is safe.
    /// </remarks>
    [Serializable]
    public enum Element
    {
        Air   = 1 << 0,
        Water = 1 << 1,
        Earth = 1 << 2,
        Fire  = 1 << 3,
    }
}