using UnityEngine;

public class CowMilkingMechanic : MonoBehaviour, IInteractable
{
    [Header("Configuración ordeño")]
    [Tooltip("Número de movimientos arriba-abajo para completar")]
    public int requiredStrokes = 8;

    [Tooltip("Distancia mínima de movimiento Y para contar como pasada")]
    public float strokeThreshold = 0.05f;

    [Header("Tags")]
    public string milkerTag = "Milker";

    // Referencias
    private AnimalReaction _animalReaction;

    // Estado del multi-step
    private enum MilkingStep { WaitingApproach, WaitingGrab, Milking, Done }
    private MilkingStep _currentStep = MilkingStep.WaitingApproach;

    // Detección de movimiento
    private GameObject _milkerInContact;
    private float _lastMilkerY;
    private bool _movingUp;
    private int _strokeCount = 0;

    void Awake()
    {
        _animalReaction = GetComponent<AnimalReaction>();

        if (_animalReaction == null)
            Debug.LogError("[CowMilkingMechanic] Falta AnimalReaction en " + gameObject.name);
    }

    void Update()
    {
        if (_currentStep == MilkingStep.Done) return;

        if (_currentStep == MilkingStep.Milking && _milkerInContact != null)
        {
            DetectStroke();
        }
    }

    // ── Detección movimiento arriba-abajo ────────────────────

    void DetectStroke()
    {
        float currentY = _milkerInContact.transform.position.y;

        if (!_movingUp)
        {
            if (currentY > _lastMilkerY + strokeThreshold)
            {
                _movingUp = true;
                _lastMilkerY = currentY;
                Debug.Log($"[CowMilkingMechanic] Subida detectada en Y: {currentY:F3}");
            }
        }
        else
        {
            if (currentY < _lastMilkerY - strokeThreshold)
            {
                _movingUp = false;
                _lastMilkerY = currentY;
                _strokeCount++;
                Debug.Log($"[CowMilkingMechanic] Pasada {_strokeCount} / {requiredStrokes} — Y: {currentY:F3}");

                if (_strokeCount >= requiredStrokes)
                    CompleteMilking();
            }
        }
    }

    // ── Colisión con la munyidora ────────────────────────────

    public void OnUdderTriggerEnter(Collider other)
    {
        if (_currentStep == MilkingStep.Done) return;
        if (!other.CompareTag(milkerTag)) return;

        PickupObject pickup = other.GetComponent<PickupObject>();

        if (_currentStep == MilkingStep.WaitingApproach)
        {
            _currentStep = MilkingStep.WaitingGrab;
            Debug.Log("[CowMilkingMechanic] Paso 1 OK — acercado a la vaca");
            return;
        }

        if (_currentStep == MilkingStep.WaitingGrab)
        {
            if (pickup != null && pickup.IsHeld)
            {
                _currentStep = MilkingStep.Milking;
                _milkerInContact = other.gameObject;
                _lastMilkerY = other.transform.position.y;
                Debug.Log("[CowMilkingMechanic] Paso 2 OK — munyidora en contacto, empieza a ordeñar");
            }
            else
            {
                Debug.Log("[CowMilkingMechanic] Acerca la munyidora pero no está agarrada");
            }
        }
    }

    public void OnUdderTriggerExit(Collider other)
    {
        if (!other.CompareTag(milkerTag)) return;

        // Opción 2: no anulamos _milkerInContact, seguimos trackeando
        Debug.Log("[CowMilkingMechanic] Munyidora fuera del trigger — sigue contando");
    }

    // ── IInteractable ────────────────────────────────────────

    public void OnInteract(InteractionType type)
    {
        if (type == InteractionType.Proximity)
            Debug.Log("[CowMilkingMechanic] Proximidad detectada por PlayerInteract");
    }

    public bool IsInteractable()
    {
        return _currentStep != MilkingStep.Done;
    }

    // ── Completar ────────────────────────────────────────────

    void CompleteMilking()
    {
        _currentStep = MilkingStep.Done;
        _milkerInContact = null;
        _animalReaction.Milk();
        Debug.Log("[CowMilkingMechanic] ¡Vaca ordeñada! Tarea completada.");
    }
}