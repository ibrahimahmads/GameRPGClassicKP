using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{ private void Start()
{
    if (GameManager.Instance != null && !string.IsNullOrEmpty(GameManager.Instance.nextSpawnPoint))
    {
        GameObject spawnPoint = GameObject.Find(GameManager.Instance.nextSpawnPoint);
        if (spawnPoint != null)
        {
            // Ambil offset dari komponen SpawnPoint
            SpawnPoint spawnPointComponent = spawnPoint.GetComponent<SpawnPoint>();
            Vector3 adjustedPosition = spawnPoint.transform.position;

            if (spawnPointComponent != null)
            {
                adjustedPosition += (Vector3)spawnPointComponent.spawnOffset;
            }

            transform.position = adjustedPosition;
        }
    }
}

}