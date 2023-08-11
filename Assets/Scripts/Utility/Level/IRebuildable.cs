namespace Utility.Level
{
    /// <summary>
    /// Component that should be rebuilt when level is changed.
    /// </summary>
    public interface IRebuildable
    {
        /// <summary>
        /// Rebuild component.
        /// </summary>
        public void Rebuild();
    }
}