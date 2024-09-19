using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGhizmos : MonoBehaviour
{
    public float detectionRange = 5f;
    public float attackRange = 1.5f;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
