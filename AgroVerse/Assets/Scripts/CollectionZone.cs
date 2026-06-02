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

        // Buscamos el componente del huevo en el objeto o sus hijos
        EggPickup egg = other.transform.root.GetComponentInChildren<EggPickup>();

        if (egg != null && !egg.hasBeenCollected)
        {
            egg.hasBeenCollected = true;
            _eggsCollected++;

            Debug.Log($"[CollectionZone] Huevo {_eggsCollected} / {eggsRequired} detectado físicamente.");

            // Buscamos tu interfaz en la escena y le decimos que sume un huevo
            ObjectivesUI ui = Object.FindFirstObjectByType<ObjectivesUI>();
            if (ui != null)
            {
                ui.AddEgg(); // Esto actualizará la UI a 1/3, 2/3... y avisará al TaskManager al llegar a 3
                Debug.Log("[CollectionZone] UI notificada con éxito.");
            }
            else
            {
                Debug.LogError("[CollectionZone] ¡ERROR! No se encuentra el script 'ObjectivesUI' en la escena para actualizar el texto.");
            }

            // Si ya hemos alcanzado el total en la cesta, bloqueamos el trigger para no contar de más
            if (_eggsCollected >= eggsRequired)
            {
                _taskCompleted = true;
                Debug.Log("[CollectionZone] Cesta llena. Objetivo completado.");
            }
        }
    }
}