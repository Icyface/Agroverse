using UnityEngine;

public class EggPickup : PickupObject
{
    [Header("Huevo")]
    public bool hasBeenCollected = false;

    // Se llama cuando el huevo entra en la zona de colección
    public void Collect()
    {
        if (hasBeenCollected) return;

        hasBeenCollected = true;
        canPickup = false;

        Debug.Log("[EggPickup] Huevo recogido correctamente.");

        // Avisar al TaskManager
        TaskManager.Instance?.CompleteTask("recoger_huevo");

        // Destruir o desactivar el huevo
        gameObject.SetActive(false);
    }

    public override void  OnInteract(InteractionType type)
    {
        base.OnInteract(type);
        Debug.Log($"[EggPickup] Huevo interactuado con: {type}");
    }
}