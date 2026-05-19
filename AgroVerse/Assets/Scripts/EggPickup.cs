using UnityEngine;

public class EggPickup : MonoBehaviour
{
    public bool hasBeenCollected = false;

    public void Collect()
    {
        if (hasBeenCollected) return;

        hasBeenCollected = true;
        Debug.Log("[EggPickup] Huevo recogido correctamente.");

        TaskManager.Instance?.CompleteTask("recoger_huevo");

        transform.root.gameObject.SetActive(false);
    }
}