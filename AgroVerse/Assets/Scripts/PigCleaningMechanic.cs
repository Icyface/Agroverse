using UnityEngine;

// Ponlo en el prefab del CERDO (no del raspall)
// El raspall solo necesita PickupObject.cs de Adrián — no toques nada en él
public class PigCleaningMechanic : MonoBehaviour, IInteractable
{
    [Header("Configuración limpieza")]
    [Tooltip("Segundos de contacto continuo necesarios para limpiar")]
    public float requiredContactTime = 2f;

    [Header("Tag de la herramienta")]
    [Tooltip("El raspall debe tener este tag en el Inspector")]
    public string brushTag = "Brush";

    // Referencias
    private AnimalReaction _animalReaction;

    // Estado interno
    private bool _isCleaning = false;
    private float _cleaningTimer = 0f;
    private bool _taskDone = false;

    void Awake()
    {
        _animalReaction = GetComponent<AnimalReaction>();

        if (_animalReaction == null)
            Debug.LogError("[PigCleaningMechanic] Falta AnimalReaction en " + gameObject.name);
    }

    void Update()
    {
        if (_taskDone) return;

        if (_isCleaning)
        {
            _cleaningTimer += Time.deltaTime;
            Debug.Log($"[PigCleaningMechanic] Limpiando... {_cleaningTimer:F1} / {requiredContactTime}s");

            if (_cleaningTimer >= requiredContactTime)
            {
                CompleteCleaning();
            }
        }
        else
        {
            // Si pierde contacto, reseteamos el timer
            _cleaningTimer = 0f;
        }
    }

    // ── Colisión con el raspall ──────────────────────────────

    void OnTriggerEnter(Collider other)
    {
        if (_taskDone) return;
        if (!other.CompareTag(brushTag)) return;

        // Solo limpia si el raspall está siendo sostenido
        PickupObject pickup = other.GetComponent<PickupObject>();
        if (pickup != null && !pickup.IsHeld) return;

        _isCleaning = true;
        Debug.Log("[PigCleaningMechanic] Contacto con raspall detectado");
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(brushTag)) return;

        _isCleaning = false;
        Debug.Log("[PigCleaningMechanic] Raspall alejado, timer reseteado");
    }

    // ── IInteractable (para PlayerInteract de Adrián) ────────

    public void OnInteract(InteractionType type)
    {
        if (type == InteractionType.Proximity)
            Debug.Log("[PigCleaningMechanic] Proximidad detectada por PlayerInteract");
    }

    public bool IsInteractable()
    {
        return !_taskDone;
    }

    // ── Completar ────────────────────────────────────────────

    void CompleteCleaning()
    {
        _taskDone = true;
        _isCleaning = false;
        _animalReaction.Clean(); // Dispara UnityEvent → consola por ahora
        Debug.Log("[PigCleaningMechanic] ¡Cerdo limpio! Tarea completada.");
    }
}
