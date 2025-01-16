using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalOnTrigger : MonoBehaviour
{
    public GameObject result;
    private DungeonResultHandler hasil;

    void Start()
    {
        hasil = result.GetComponent<DungeonResultHandler>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            result.SetActive(true);
            hasil.showResult();
        }
    }
}
