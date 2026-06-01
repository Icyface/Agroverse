using UnityEngine;
using UnityEngine.XR;

public class CowMilkingMechanic : MonoBehaviour, IInteractable
{
    [Header("Configuración Ordeño")]
    [Tooltip("Movimientos arriba-abajo requeridos con el mando")]
    public int requiredStrokes = 8;
    [Tooltip("Distancia vertical mínima para contar un movimiento")]
    public float strokeThreshold = 0.05f;
    [Tooltip("Tiempo mínimo en segundos entre tirón y tirón para que no vaya súper rápido")]
    public float strokeCooldown = 0.6f;

    [Header("Referencias de VR (Mandos)")]
    public string handTag = "Hand";

    [Header("Efectos Visuales y Sonido")]
    public ParticleSystem milkParticles;
    
    private SonidoAnimacion _sonidoAnimacion;
    private Animator _animator;
    private AnimalReaction _animalReaction;

    private enum MilkingState { Waiting, Milking, Done }
    private MilkingState _currentState = MilkingState.Waiting;

    private GameObject _handInContact;
    private float _lastHandY;
    private bool _movingUp;
    private int _strokeCount = 0;

    // Control de tiempo para el ritmo
    private float _nextStrokeTime = 0f;

    private InputDevice _targetDevice;

    void Awake()
    {
        _animalReaction = GetComponent<AnimalReaction>();
        _sonidoAnimacion = GetComponent<SonidoAnimacion>();
        _animator = GetComponent<Animator>();

        if (_animalReaction == null)
            Debug.LogError("[CowMilkingMechanic] Falta AnimalReaction en " + gameObject.name);
        
        if (milkParticles != null)
            milkParticles.Stop();
    }

    void Update()
    {
        if (_currentState == MilkingState.Done) return;

        if (_currentState == MilkingState.Milking && _handInContact != null)
        {
            // Ahora comprueba que AMBOS botones estén presionados a la vez
            if (IsPlayerSqueezingBothButtons())
            {
                DetectHandStroke();
            }
            else
            {
                _lastHandY = _handInContact.transform.position.y;
            }
        }
    }

    // Devuelve 'true' SOLO si se están apretando el Grip Y el Trigger al mismo tiempo
    bool IsPlayerSqueezingBothButtons()
    {
        if (!_targetDevice.isValid) return false;

        _targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool gripPressed);
        _targetDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerPressed);

        // Retorna verdadero solo si cierras la mano (Grip) Y aprietas el dedo índice (Trigger)
        return gripPressed && triggerPressed;
    }

    void DetectHandStroke()
    {
        // Si no ha pasado el tiempo de cooldown, ignoramos el movimiento
        if (Time.time < _nextStrokeTime) return;

        float currentHandY = _handInContact.transform.position.y;
        float deltaY = currentHandY - _lastHandY;

        if (Mathf.Abs(deltaY) > strokeThreshold)
        {
            bool isNowMovingUp = deltaY > 0;

            if (isNowMovingUp != _movingUp)
            {
                _strokeCount++;
                _movingUp = isNowMovingUp;
                _lastHandY = currentHandY;
                
                // Aplicamos el cooldown para el siguiente tirón
                _nextStrokeTime = Time.time + strokeCooldown;

                Debug.Log($"[Ordeño] Tirón válido registrado. Progreso: {_strokeCount}/{requiredStrokes}");

                // --- FEEDBACK VISUAL MEJORADO ---
                if (milkParticles != null)
                {
                   milkParticles.Clear(); // Borra al instante los chorros viejos que queden flotando
                    milkParticles.Stop();  // Detiene el sistema por si acaso antes de reiniciarlo// Resetea el chorro anterior si quedaba algo
                    milkParticles.Play(); // Lanza un lechazo nuevo e independiente
                }

                if (_sonidoAnimacion != null)
                    _sonidoAnimacion.ReproducirSonido();

                if (_animator != null)
                    _animator.SetTrigger("Moverse");

                if (_strokeCount >= requiredStrokes)
                    CompleteMilking();
            }
        }
    }

    public void OnUdderTriggerEnter(Collider other)
    {
        if (_currentState == MilkingState.Done) return;

        if (other.CompareTag(handTag))
        {
            _currentState = MilkingState.Milking;
            _handInContact = other.gameObject;
            _lastHandY = other.transform.position.y;

            if (other.gameObject.name.ToLower().Contains("left"))
            {
                _targetDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
            }
            else
            {
                _targetDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
            }

            Debug.Log("[CowMilkingMechanic] Mando en la ubre. ¡Mantén pulsados GRIP + GATILLO a la vez y tira!");
        }
    }

    public void OnUdderTriggerExit(Collider other)
    {
        if (other.CompareTag(handTag))
        {
            if (_currentState == MilkingState.Milking)
                _currentState = MilkingState.Waiting;

            _handInContact = null;
            
            if (milkParticles != null)
                milkParticles.Stop();
            
            Debug.Log("[CowMilkingMechanic] Mando fuera de la ubre.");
        }
    }

    void CompleteMilking()
    {
        _currentState = MilkingState.Done;
        _handInContact = null;

        if (milkParticles != null)
            milkParticles.Stop();

        _animalReaction.ChangeState(AnimalState.Final); 

        if (TaskManager.Instance != null)
            TaskManager.Instance.CompleteTask("munyir_vaca"); 
        
        Debug.Log("[CowMilkingMechanic] ¡Vaca completamente ordeñada!");
    }

    public void OnInteract(InteractionType type) { }
    public bool IsInteractable() => _currentState != MilkingState.Done;
}