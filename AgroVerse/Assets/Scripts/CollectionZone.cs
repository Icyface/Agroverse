using UnityEngine;

public class CollectionZone : MonoBehaviour
{
    [Header("Configuración")]
    public int eggsRequired = 3;

    private int _eggsCollected = 0;
    private bool _taskCompleted = false;

    void OnTriggerEnter(Collider other)
    {
        if (_taskCompleted) return;

        // Buscamos el script de forma mucho más segura.
        // Primero en el propio objeto que choca, luego en sus hijos, y por último en sus padres.
        EggPickup egg = other.GetComponent<EggPickup>();
        if (egg == null) egg = other.GetComponentInChildren<EggPickup>();
        if (egg == null) egg = other.GetComponentInParent<EggPickup>();

        // Si sigue siendo null, probamos con el método root por si acaso
        if (egg == null) egg = other.transform.root.GetComponentInChildren<EggPickup>();

        if (egg != null)
        {
            if (!egg.hasBeenCollected)
            {
                egg.hasBeenCollected = true;
                _eggsCollected++;

                Debug.Log($"[CollectionZone] ✓ Huevo contado con éxito: {_eggsCollected} / {eggsRequired}. Objeto: {other.gameObject.name}");

                // Notificamos a la UI
                ObjectivesUI ui = Object.FindFirstObjectByType<ObjectivesUI>();
                if (ui != null)
                {
                    ui.AddEgg();
                }

                if (_eggsCollected >= eggsRequired)
                {
                    _taskCompleted = true;
                    Debug.Log("[CollectionZone] ¡Objetivo de huevos alcanzado!");
                }
            }
            else
            {
                // CHIVATO: Si el huevo ya se había contado antes, te avisará en la consola
                Debug.LogWarning($"[CollectionZone] El huevo {other.gameObject.name} ha entrado, pero YA tenía el check de recolectado activo.");
            }
        }
    }
}