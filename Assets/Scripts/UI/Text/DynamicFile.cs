using UnityEngine;

namespace UI.Text
{
    /// <summary>
    /// Dynamically get text from <see cref="Resources">resources</see> file.
    /// </summary>
    public class DynamicFile : DynamicText
    {
        [SerializeField]
        private TextAsset file;

        protected override void Awake()
        {
            base.Awake();
            Reload();
        }

        /// <summary>
        /// Updates text field according to file text.
        /// </summary>
        [ContextMenu(nameof(Reload))]
        public void Reload()
        {
            if (file) TextField.text = file.text;
            else Debug.LogWarning("Text file is missing.", this);
        }
    }
}