using UnityEngine;

public class CowMilkingMechanic : MonoBehaviour, IInteractable
{
    [Header("Configuración Ordeño")]
    [Tooltip("Movimientos arriba-abajo requeridos con la mano")]
    public int requiredStrokes = 8;
    [Tooltip("Distancia vertical mínima para contar un movimiento")]
    public float strokeThreshold = 0.05f;

    [Header("Referencias de VR (Manos)")]
    [Tooltip("Tag que tienen los colisionadores de las manos del jugador en VR")]
    public string handTag = "Hand"; // Asegúrate de que tus manos en VR tengan este Tag o cámbialo aquí

    [Header("Efectos Visuales y Sonido")]
    [Tooltip("Arrastra aquí las partículas de leche que saldrán de las ubres")]
    public ParticleSystem milkParticles;
    
    [Tooltip("Script de sonido y animación que nos pasaste")]
    private SonidoAnimacion _sonidoAnimacion;
    private Animator _animator;

    // Referencias internas
    private AnimalReaction _animalReaction;

    // Estados del ordeño
    private enum MilkingState { Waiting, Milking, Done }
    private MilkingState _currentState = MilkingState.Waiting;

    // Control del movimiento de la mano
    private GameObject _handInContact;
    private float _lastHandY;
    private bool _movingUp;
    private int _strokeCount = 0;

    void Awake()
    {
        _animalReaction = GetComponent<AnimalReaction>();
        _sonidoAnimacion = GetComponent<SonidoAnimacion>();
        _animator = GetComponent<Animator>(); // Para activar las animaciones de la cola/cabeza

        if (_animalReaction == null)
            Debug.LogError("[CowMilkingMechanic] Falta AnimalReaction en " + gameObject.name);
        
        if (milkParticles != null)
            milkParticles.Stop(); // Empezar sin soltar leche
    }

    void Update()
    {
        if (_currentState == MilkingState.Done) return;

        // Si la mano está en la zona y se está moviendo
        if (_currentState == MilkingState.Milking && _handInContact != null)
            DetectHandStroke();
    }

    // ── DETECCIÓN DEL MOVIMIENTO ARRIBA/ABAJO DE LA MANO ────────────────
    void DetectHandStroke()
    {
        float currentHandY = _handInContact.transform.position.y;
        float deltaY = currentHandY - _lastHandY;

        if (Mathf.Abs(deltaY) > strokeThreshold)
        {
            bool isNowMovingUp = deltaY > 0;

            // Si cambia de dirección (sube tras bajar, o baja tras subir) cuenta como un "stroke"
            if (isNowMovingUp != _movingUp)
            {
                _strokeCount++;
                _movingUp = isNowMovingUp;
                _lastHandY = currentHandY;

                Debug.Log($"[Ordeño] Progreso: {_strokeCount}/{requiredStrokes}");

                // --- FEEDBACK EN TIEMPO REAL ---
                // 1. Activar partículas de leche momentáneamente
                if (milkParticles != null && !milkParticles.isPlaying)
                    milkParticles.Play();

                // 2. Hacer ruido (reproduce el sonido del animal)
                if (_sonidoAnimacion != null)
                    _sonidoAnimacion.ReproducirSonido();

                // 3. Activar animación en el Animator de la vaca (Mover cola/cabeza)
                if (_animator != null)
                    _animator.SetTrigger("Moverse"); // Asegúrate de que el parámetro en el Animator sea un Trigger llamado "Moverse"

                // Comprobar si hemos terminado
                if (_strokeCount >= requiredStrokes)
                    CompleteMilking();
            }
        }
    }

    // ── TRIGGERS (Llamados desde UdderZoneTrigger) ──────────────────────
    public void OnUdderTriggerEnter(Collider other)
    {
        if (_currentState == MilkingState.Done) return;

        // Comprobamos si lo que toca las ubres es la mano del jugador
        if (other.CompareTag(handTag))
        {
            _currentState = MilkingState.Milking;
            _handInContact = other.gameObject;
            _lastHandY = other.transform.position.y;
            Debug.Log("[CowMilkingMechanic] Mano en la ubre. ¡Empieza a mover de arriba a abajo!");
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
                milkParticles.Stop(); // Deja de salir leche si quitas la mano
            
            Debug.Log("[CowMilkingMechanic] Mano fuera de la ubre.");
        }
    }

    // ── FINALIZAR TAREA ─────────────────────────────────────────────────
void CompleteMilking()
{
    _currentState = MilkingState.Done;
    _handInContact = null;

    if (milkParticles != null)
        milkParticles.Stop();

    // 1. Cambia el estado de la vaca al Estado Final usando el nuevo script limpio
    _animalReaction.ChangeState(AnimalState.Final); 

    // 2. Notifica al TaskManager
    if (TaskManager.Instance != null)
        TaskManager.Instance.CompleteTask("munyir_vaca"); 
    
    Debug.Log("[CowMilkingMechanic] ¡Vaca completamente ordeñada!");
}

    // Interfaz requerida por vuestro sistema
    public void OnInteract(InteractionType type) { }
    public bool IsInteractable() => _currentState != MilkingState.Done;
}