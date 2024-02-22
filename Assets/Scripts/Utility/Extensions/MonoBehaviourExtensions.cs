using Audio;
using Unity.VisualScripting;
using UnityEngine;

namespace Utility.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="MonoBehaviour"/>.
    /// </summary>
    public static class MonoBehaviourExtensions
    {

        public static bool TryGetRelatedComponent<T>(this GameObject mb, out T component) where T : Component
        {
            component = mb.GetComponentInChildren<T>();

            // Component is part of the same game object or one of its children.
            if (component != null) return true;

            component = mb.GetComponentInParent<T>();

            // Component is part of one of its parents.
            if (component != null) return true;

            // Component is part of one of its siblings.
            component = mb.transform.root.GetComponentInChildren<T>();

            return component != null;
        }

        public static bool TryGetRelatedComponent<T>(this Component mb, out T component) where T : Component
        {
            component = mb.GetComponentInChildren<T>();

            // Component is part of the same game object or one of its children.
            if (component != null) return true;

            component = mb.GetComponentInParent<T>();

            // Component is part of one of its parents.
            if (component != null) return true;

            // Component is part of one of its siblings.
            component = mb.transform.root.GetComponentInChildren<T>();

            return component != null;
        }

        public static bool TryGetRequiredRelatedComponent<T>(this Component mb, out T component) where T : Component
        {
            if (!mb.TryGetRelatedComponent(out component))
            {
                Debug.LogWarning($"No related {typeof(T)} found. Possible functionality loss/break.", mb);
                return false;
            }

            return true;
        }


        /// <summary>
        /// Require component of type <typeparamref name="T"/> to be attached to the game object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mb"></param>
        /// <param name="component"></param>
        public static T GetRelatedComponent<T>(this GameObject mb) where T : Component
        {
            T component = mb.GetComponentInChildren<T>();

            // Component is part of the same game object or one of its children.
            if (component != null) return component;

            component = mb.GetComponentInParent<T>();

            // Component is part of one of its parents.
            if (component != null) return component;

            // Component is part of one of its siblings.
            component = mb.transform.root.GetComponentInChildren<T>();

            if (component != null) return component;

            // New component is created.
            component = mb.AddComponent<T>();

            Debug.LogWarning($"{mb.name} requires {typeof(T)} component. New one was attached", mb);

            return component;
        }



        /// <summary>
        /// Require component of type <typeparamref name="T"/> to be attached to the game object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mb"></param>
        /// <param name="component"></param>
        public static void GetRelatedComponent<T>(this GameObject mb, ref T component) where T : Component
        {
            // Component is already assigned.
            if (component != null) return;

            component = mb.GetComponentInChildren<T>();

            // Component is part of the same game object or one of its children.
            if (component != null) return;

            component = mb.GetComponentInParent<T>();

            // Component is part of one of its parents.
            if (component != null) return;

            // Component is part of one of its siblings.
            component = mb.transform.root.GetComponentInChildren<T>();

            if (component != null) return;

            // New component is created.
            component = mb.AddComponent<T>();

            Debug.LogWarning($"{mb.name} requires {typeof(T)} component. New one was attached", mb);
        }

        /// <summary>
        /// Require component of type <typeparamref name="T"/> to be attached to the game object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mb"></param>
        /// <param name="component"></param>
        public static void GetRelatedComponent<T>(this Component mb, ref T component) where T : Component
        {
            // Component is already assigned.
            if (component != null) return;

            component = mb.GetComponentInChildren<T>();

            // Component is part of the same game object or one of its children.
            if (component != null) return;

            component = mb.GetComponentInParent<T>();

            // Component is part of one of its parents.
            if (component != null) return;

            // Component is part of one of its siblings.
            component = mb.transform.root.GetComponentInChildren<T>();

            if (component != null) return;

            // New component is created.
            component = mb.AddComponent<T>();

            Debug.LogWarning($"{mb.name} requires {typeof(T)} component. New one was attached", mb);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mb"></param>
        /// <param name="component"></param>
        public static void RelatedInterface<T>(this Component mb, ref T component) where T : class
        {
            // Ensure it is interface.
            if (!typeof(T).IsInterface)
            {
                Debug.LogError($"{typeof(T)} is not an interface", mb);
                return;
            }
            
            // Component is already assigned.
            if (component != null) return;
            
        #pragma warning disable UNT0014 // Invalid type for call to GetComponent - we know it's interface
            component = mb.GetComponentInChildren<T>();

            // Component is part of the same game object or one of its children.
            if (component != null) return;

            component = mb.GetComponentInParent<T>();

            // Component is part of one of its parents.
            if (component != null) return;

            // Component is part of one of its siblings.
            component = mb.transform.root.GetComponentInChildren<T>();
        #pragma warning restore UNT0014 // Invalid type for call to GetComponent

            if (component != null) return;

            Debug.LogWarning($"{mb.name} requires {typeof(T)} component. New one was attached", mb);
        }

    }
}