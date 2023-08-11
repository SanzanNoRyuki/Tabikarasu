using DG.Tweening;

namespace Utility.Extensions
{
    public static class TweenExtensions
    {
        /// <summary>
        /// Overrides the <paramref name="overriden"/> tween with the <paramref name="tween"/>.
        /// </summary>
        /// <remarks>
        /// Used to prevent multiple tweens from running at the same time on the same value.
        /// </remarks>
        /// <param name="tween"></param>
        /// <param name="overriden"></param>
        /// <returns></returns>
        public static Tween Override(this Tween tween, ref Tween overriden)
        {
            overriden.Kill();
            overriden = tween;
            return tween;
        }
    }
}