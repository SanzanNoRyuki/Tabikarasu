using UnityEngine;

namespace Utility.Attributes
{
    /// <summary>
    /// Attribute for interface type inspector property.
    /// </summary>
    public class RequireInterfaceAttribute : PropertyAttribute
    {
        /// <summary>
        /// Required interface type.
        /// </summary>
        public System.Type RequiredType { get; private set; }

        /// <summary>
        /// Requiring implementation of the <see cref="T:Utility.Attributes.RequireInterfaceAttribute"/> interface.
        /// </summary>
        /// <param name="type">Interface type.</param>
        public RequireInterfaceAttribute(System.Type type)
        {
            RequiredType = type;
        }
    }
}