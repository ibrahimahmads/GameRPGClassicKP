using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicSorting : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
        }
    }
}
