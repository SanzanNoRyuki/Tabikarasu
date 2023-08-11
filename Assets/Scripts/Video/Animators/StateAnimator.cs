using UnityEngine;

namespace Characters.Behaviour
{
    /// <summary>
    /// Base class for state-based animator.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Animator))]
    public abstract class StateAnimator : MonoBehaviour
    {
        protected int  CurrentState { get; private set; }
        
        private bool     _isLocked;
        private Animator _animator;

        protected void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        protected virtual void Update()
        {
            int state = UpdateState();
            if (state == CurrentState) return;

            _animator.CrossFade(state, 0f, 0);
            CurrentState = state;
        }

        /// <summary>
        /// Replay current state.
        /// </summary>
        [ContextMenu("Replay")]
        public void Replay()
        {
            _animator.Play(CurrentState, 0, 0f);
        }

        /// <summary>
        /// Reset state to default.
        /// </summary>
        public void ResetState()
        {
            CurrentState = 0;
            _isLocked = false;
        }

        /// <summary>
        /// Lock animation state for its duration.
        /// </summary>
        /// <param name="state">State to lock.</param>
        /// <returns>Locked state.</returns>
        protected int Lock(int state)
        {
            _isLocked     = true;
            return state;
        }
        
        /// <summary>
        /// Get the current state.
        /// </summary>
        /// <returns>Updated state.</returns>
        protected abstract int GetState();

        private int UpdateState()
        {
            if (_isLocked)
            {
                if (IsFinished())
                {
                    _isLocked = false;
                }
                else
                {
                    return CurrentState;
                }
            }
            

            return GetState();
        }

        private bool IsFinished()
        {
            return _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f;
        }
    }
}