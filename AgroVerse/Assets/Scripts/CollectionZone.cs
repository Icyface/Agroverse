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

        EggPickup egg = other.transform.root.GetComponentInChildren<EggPickup>();

        if (egg != null && !egg.hasBeenCollected)
        {
            egg.hasBeenCollected = true;
            _eggsCollected++;

            Debug.Log($"[CollectionZone] Huevo {_eggsCollected} / {eggsRequired} en la cesta.");

            if (_eggsCollected >= eggsRequired)
            {
                _taskCompleted = true;
                Debug.Log("[CollectionZone] ¡Todos los huevos recogidos!");
                TaskManager.Instance?.CompleteTask("recoger_huevo");
            }
        }
    }
}