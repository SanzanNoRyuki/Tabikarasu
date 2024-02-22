using UnityEngine;

namespace Video.VFX
{
    /// <summary>
    /// Animation visual special effect.
    /// </summary>
    /// <remarks>
    /// To work properly, <see cref="Animator">animator</see> must have single non-looping animation.
    /// </remarks>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Animator))]
    public class AnimationVFX : MonoBehaviour
    {
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();

            // Prevent animator playing animation by default.
            _animator.Play(_animator.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, 1f);
        }

        /// <summary>
        /// Plays VFX animation once.
        /// </summary>
        [ContextMenu("Play")]
        public void Play()
        {
            _animator.Play(_animator.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, 0f);
        }
    }
}