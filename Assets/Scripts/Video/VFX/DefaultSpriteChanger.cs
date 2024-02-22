using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(SpriteRenderer))]
public class DefaultSpriteChanger : MonoBehaviour
{
    [SerializeField]
    private Sprite enabledSprite;

    [SerializeField]
    private Sprite disabledSprite;

    [SerializeField]
    private MonoBehaviour target;

    private void Awake()
    {
        UpdateSprite();
    }

    private void OnEnable()
    {
        if (Application.IsPlaying(gameObject)) enabled = false;
    }

    private void Update()
    {
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        if (target != null) GetComponent<SpriteRenderer>().sprite = target.enabled ? enabledSprite : disabledSprite;
    }
}