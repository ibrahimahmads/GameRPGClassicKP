using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Items/Item")]
public class ItemData : ScriptableObject
{
    public GameObject itemPrefab; // Prefab item
    public float dropChance; // Peluang drop item
    public AnimationClip itemAnimation; // Animasi item (jika diperlukan)
}
