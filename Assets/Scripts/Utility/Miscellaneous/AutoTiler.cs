using System;
using UnityEngine;
using UnityEngine.Serialization;

[ExecuteAlways]
[RequireComponent(typeof(BoxCollider2D))]
public class AutoTiler : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private void OnEnable()
    {
        if (Application.IsPlaying(gameObject)) enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (spriteRenderer == null) return;

        BoxCollider2D coll = GetComponent<BoxCollider2D>();
        coll.size = new Vector2(spriteRenderer.size.x * 0.95f, coll.size.y);
    }
}
