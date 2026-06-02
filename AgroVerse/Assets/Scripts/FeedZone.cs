using UnityEngine;

public class FeedZone : MonoBehaviour
{
    [Header("Configuración")]
    public string acceptedFoodType = "generic";
    public string animalName = "Animal";
    public float feedDuration = 3f;

    private bool _hasBeenFed = false;
    private float _feedTimer = 0f;
    private bool _isFeeding = false;

    // Referencia al Animator de la gallina
    private Animator _animator;

    // Contador compartido entre todos los FeedZone
    private static int _chickensFed = 0;
    private static int _chickensRequired = 2;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            _animator = GetComponentInParent<Animator>();
        }
    }

    void Start()
    {
        _chickensFed = 0; 

        // 🌟 TRUCO: Al empezar el juego, pausamos la animación de la gallina
        // para que se quede quieta como si fuera su pose de espera (Idle)
        if (_animator != null)
        {
            _animator.speed = 0f;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (_hasBeenFed) return;

        FoodContainer food = other.GetComponent<FoodContainer>();
        if (food == null) food = other.GetComponentInParent<FoodContainer>();

        if (food != null && food.foodType == acceptedFoodType)
        {
            _isFeeding = true;
            _feedTimer = 0f;
            Debug.Log($"[FeedZone] ¡Alimentando a {animalName}! Activando movimiento.");

            // 🌟 TRUCO: Despausamos la animación. La gallina empezará a moverse/picar el suelo
            if (_animator != null)
            {
                _animator.speed = 1f; 
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (_hasBeenFed || !_isFeeding) return;

        FoodContainer food = other.GetComponent<FoodContainer>();
        if (food == null) food = other.GetComponentInParent<FoodContainer>();

        if (food != null && food.foodType == acceptedFoodType)
        {
            _feedTimer += Time.deltaTime;
            Debug.Log($"[FeedZone] {animalName} comiendo: {_feedTimer:F1} / {feedDuration}s");

            if (_feedTimer >= feedDuration)
            {
                Feed();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (_hasBeenFed) return;

        FoodContainer food = other.GetComponent<FoodContainer>();
        if (food == null) food = other.GetComponentInParent<FoodContainer>();

        if (food != null && food.foodType == acceptedFoodType)
        {
            _isFeeding = false;
            _feedTimer = 0f;
            Debug.Log($"[FeedZone] {animalName}: alimentación cancelada. Pausando movimiento.");

            // 🌟 TRUCO: Si se lleva la comida antes de tiempo, la gallina se vuelve a congelar
            if (_animator != null)
            {
                _animator.speed = 0f;
            }
        }
    }

    void Feed()
    {
        _hasBeenFed = true;
        _isFeeding = false;
        _chickensFed++;

        Debug.Log($"[FeedZone] ✓ {animalName} alimentado con éxito. Pollos: {_chickensFed} / {_chickensRequired}");

        // 🌟 TRUCO: Cuando ya está saciada, congelamos la animación para que deje de comer
        if (_animator != null)
        {
            _animator.speed = 0f; 
        }

        if (_chickensFed >= _chickensRequired)
        {
            Debug.Log("[FeedZone] ¡Todos los pollos requeridos han comido!");
            if (TaskManager.Instance != null)
            {
                TaskManager.Instance.CompleteTask("alimentar_chicken");
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}