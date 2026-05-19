using UnityEngine;

// Ponlo en el prefab de la GALLINA
// El recipiente de comida solo necesita PickupObject.cs de Adrián
public class FeedingMechanic : MonoBehaviour, IInteractable
{
    [Header("Configuración")]
    public string foodTag = "Food";

    // Referencias
    private AnimalReaction _animalReaction;
    private bool _taskDone = false;

    void Awake()
    {
        _animalReaction = GetComponent<AnimalReaction>();

        if (_animalReaction == null)
            Debug.LogError("[FeedingMechanic] Falta AnimalReaction en " + gameObject.name);
    }

    // ── Colisión con la comida ───────────────────────────────

    void OnTriggerEnter(Collider other)
    {
        if (_taskDone) return;
        if (!other.CompareTag(foodTag)) return;

        PickupObject pickup = other.GetComponent<PickupObject>();
        if (pickup != null && !pickup.IsHeld) return;

        CompleteFeeding();
    }

    // ── IInteractable ────────────────────────────────────────

    public void OnInteract(InteractionType type)
    {
        if (type == InteractionType.Proximity)
            Debug.Log("[FeedingMechanic] Proximidad detectada por PlayerInteract");
    }

    public bool IsInteractable() => !_taskDone;

    // ── Completar ────────────────────────────────────────────

    void CompleteFeeding()
    {
        _taskDone = true;
        _animalReaction.Feed();
        Debug.Log("[FeedingMechanic] ¡Gallina alimentada! Tarea completada.");
    }
}