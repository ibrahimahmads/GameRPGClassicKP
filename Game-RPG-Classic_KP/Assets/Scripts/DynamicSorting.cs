using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicSorting : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public float updateRadius = 10f;       // Radius jarak untuk update sorting order
    private Transform playerTransform;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Player object not found. Make sure the player has a tag 'Player'.");
        }
    }

    void FixedUpdate()
    {
        if (playerTransform != null && IsWithinRadius())
        {
            spriteRenderer.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
        }
    }

    private bool IsWithinRadius()
    {
        float distance = Vector2.Distance(transform.position, playerTransform.position);
        return distance <= updateRadius;
    }
}
