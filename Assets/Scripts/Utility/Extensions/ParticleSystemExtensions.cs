using UnityEngine;

namespace Utility.Extensions
{
    public static class ParticleSystemExtensions
    {
        /// <summary>
        /// Restarts particle system.
        /// </summary>
        /// <param name="particleSystem"></param>
        public static void Restart(this ParticleSystem particleSystem)
        {
            if (particleSystem.isPlaying) particleSystem.Stop();
            particleSystem.Play();
        }
    }
}