using System;

namespace Elements
{
    /// <summary>
    /// Classical elements.
    /// </summary>
    /// <remarks>
    /// Single flag cast to <see cref="Element"/> is safe. Otherwise it's undefined.
    /// </remarks>
    [Flags]
    public enum Elements
    {
        Nothing    = 0,
        Everything = ~0,
        Air        = 1 << 0,
        Water      = 1 << 1,
        Earth      = 1 << 2,
        Fire       = 1 << 3,
    }
}