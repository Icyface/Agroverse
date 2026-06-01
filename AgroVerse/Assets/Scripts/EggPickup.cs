using UnityEngine;

public class EggPickup : MonoBehaviour
{
    public bool hasBeenCollected = false;

    public void Collect()
    {
        if (hasBeenCollected) return;

        hasBeenCollected = true;
        Debug.Log("[EggPickup] Huevo recogido correctamente.");

        transform.root.gameObject.SetActive(false);
    }
}