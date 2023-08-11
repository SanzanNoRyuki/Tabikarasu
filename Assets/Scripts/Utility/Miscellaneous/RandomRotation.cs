using UnityEngine;

namespace Utility.Miscellaneous
{
    /// <summary>
    /// Randomly rotates object on enable.
    /// </summary>
    [DisallowMultipleComponent]
    public class RandomRotation : MonoBehaviour
    {
        private void OnEnable()
        {
            transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
        }
    }
}
