using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Video.Shader_Instances
{
    /// <summary>
    /// Default shader instance.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(SpriteRenderer))]
    public abstract class ShaderInstance : MonoBehaviour, INotifyPropertyChanged
    {
        /// <summary>
        /// Minimum shader progress value.
        /// </summary>
        public const float MinProgress = 0f;

        /// <summary>
        /// Maximum shader progress value.
        /// </summary>
        public const float MaxProgress = 1f;

        /// <summary>
        /// Minimum shader border value.
        /// </summary>
        public const float MinBorder = 0f;

        /// <summary>
        /// Maximum shader border value.
        /// </summary>
        public const float MaxBorder = 1f;

        /// <summary>
        /// Minimum shader noise value.
        /// </summary>
        public const float MinNoise = 0f;

        /// <summary>
        /// Maximum shader noise value.
        /// </summary>
        public const float MaxNoise = 1f;

        /// <summary>
        /// Minimum shader noise scale value.
        /// </summary>
        public const float MinNoiseScale = 0f;

        /// <summary>
        /// Maximum shader noise scale value.
        /// </summary>
        public const float MaxNoiseScale = 1000f;

        /// <summary>
        /// <see cref="SpriteRenderer"/> material.
        /// </summary>
        protected Material Material;

        /// <summary>
        /// Event that is invoked when any property is changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private void Awake()
        {
            Material = GetComponent<SpriteRenderer>().material;
        
            ApplyChanges();
            PropertyChanged += ApplyChanges;
        }

        private void OnDestroy()
        {
            PropertyChanged -= ApplyChanges;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Apply shader changes to the material.
        /// </summary>
        protected abstract void ApplyChanges();

        private void ApplyChanges(object sender, PropertyChangedEventArgs e)
        {
            ApplyChanges();
        }
    }
}