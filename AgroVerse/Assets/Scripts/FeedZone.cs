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

    private Animator _animator;

    // Contador estático para la misión global
    private static int _chickensFed = 0;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        if (_animator == null) _animator = GetComponentInParent<Animator>();
    }

    void Start()
    {
        _chickensFed = 0; 
        
        // Al empezar, nos aseguramos de que el parámetro esté apagado
        if (_animator != null)
        {
            _animator.SetBool("isEating", false);
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
            Debug.Log($"[FeedZone] ¡Alimentando a {animalName}!");

            // 🌟 NUEVO: Solo ESTA gallina activa su animación
            if (_animator != null)
            {
                _animator.SetBool("isEating", true); 
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
            Debug.Log($"[FeedZone] {animalName}: alimentación cancelada.");

            // 🌟 NUEVO: Si se retira la comida, solo ESTA gallina se detiene
            if (_animator != null)
            {
                _animator.SetBool("isEating", false);
            }
        }
    }

    void Feed()
    {
        _hasBeenFed = true;
        _isFeeding = false;
        _chickensFed++;

        Debug.Log($"[FeedZone] ✓ {animalName} alimentado con éxito. Total: {_chickensFed}");

        // 🌟 NUEVO: Apagamos la animación de esta gallina porque ya terminó
        if (_animator != null)
        {
            _animator.SetBool("isEating", false); 
        }

        // Actualizamos la interfaz custom
        ObjectivesUI ui = Object.FindFirstObjectByType<ObjectivesUI>();
        if (ui != null)
        {
            ui.FeedAnimal(); 
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}