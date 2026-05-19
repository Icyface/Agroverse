using UnityEngine;

public class CollectionZone : MonoBehaviour
{
    [Header("Qué acepta esta zona")]
    public string acceptedTag = "Egg";

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(acceptedTag))
        {
            EggPickup egg = other.GetComponent<EggPickup>();
            if (egg != null && egg.IsHeld == false)
            {
                // El huevo ha sido soltado dentro de la zona
                egg.Collect();
            }
        }
    }
}