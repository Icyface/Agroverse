using UnityEngine;

public class CollectionZone : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        EggPickup egg = other.transform.root.GetComponentInChildren<EggPickup>();

        if (egg != null && !egg.hasBeenCollected)
        {
            egg.Collect();
            Debug.Log("[CollectionZone] Huevo recogido.");
        }
    }
}