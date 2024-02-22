using System;
using Elements;
using static Elements.Elements;

namespace Utility.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="Elements"/>.
    /// </summary>
    public static class ElementsExtensions
    {
        /// <summary>
        /// Determines whether one or more bit fields are set in the current instance.
        /// </summary>
        /// <param name="elements">Elements reference.</param>
        /// <param name="element">An enumeration value.</param>
        /// <returns>True if the bit field or bit fields that are set in flag are also set in the current instance. False otherwise.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Element is not handled by the elements.</exception>
        public static bool HasFlag(this Elements.Elements elements, Element element)
        {
            return element switch
                   {
                       Element.Air   => elements.HasFlag(Air),
                       Element.Water => elements.HasFlag(Water),
                       Element.Earth => elements.HasFlag(Earth),
                       Element.Fire  => elements.HasFlag(Fire),
                       _             => throw new ArgumentOutOfRangeException(nameof(element), element, "Element is not handled by the elements."),
                   };
        }
    }
}