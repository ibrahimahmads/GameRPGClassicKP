using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour
{
    public Vector2 startPosition;  // Posisi awal slime
    
    void Start()
    {
        // Simpan posisi awal ketika game dimulai
        startPosition = transform.position;
    }
}
