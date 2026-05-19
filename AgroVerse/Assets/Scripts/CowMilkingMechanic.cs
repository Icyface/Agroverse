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
        float delta = currentY - _lastMilkerY;

        // Detecta cambio de dirección con threshold
        if (delta > strokeThreshold && !_movingUp)
        {
            _movingUp = true;
        }
        else if (delta < -strokeThreshold && _movingUp)
        {
            _movingUp = false;
            _strokeCount++;
            Debug.Log($"[CowMilkingMechanic] Pasada {_strokeCount} / {requiredStrokes}");

            if (_strokeCount >= requiredStrokes)
                CompleteMilking();
        }

        _lastMilkerY = currentY;
    }

    // ── Colisión con la munyidora ────────────────────────────

    void OnTriggerEnter(Collider other)
    {
        if (_currentStep == MilkingStep.Done) return;
        if (!other.CompareTag(milkerTag)) return;

        PickupObject pickup = other.GetComponent<PickupObject>();

        // Paso 1 → 2: jugador se acerca
        if (_currentStep == MilkingStep.WaitingApproach)
        {
            _currentStep = MilkingStep.WaitingGrab;
            Debug.Log("[CowMilkingMechanic] Paso 1 OK — acercado a la vaca");
            return;
        }

        // Paso 2 → 3: munyidora agarrada entra en contacto
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

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(milkerTag)) return;

        if (_currentStep == MilkingStep.Milking)
        {
            _milkerInContact = null;
            Debug.Log("[CowMilkingMechanic] Munyidora alejada — sigue desde donde estabas");
            // No reseteamos _strokeCount, el jugador puede volver a acercarla
        }
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