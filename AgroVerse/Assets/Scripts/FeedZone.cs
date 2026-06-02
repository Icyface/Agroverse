using UnityEngine;

public class FeedZone : MonoBehaviour
{
    [Header("Configuración")]
    public string acceptedFoodType = "grain"; // Ajustado a vuestro tipo "grain"
    public string animalName = "Animal";
    public float feedDuration = 3f;

    [Header("Visuales del Alimento")]
    [Tooltip("Arrastra aquí el modelo, malla o hijo del plato vacío que queráis activar al terminar.")]
    public GameObject emptyVisualPrefab; 

    private bool _hasBeenFed = false;
    private float _feedTimer = 0f;
    private bool _isFeeding = false;

    private Animator _animator;
    private static int _chickensFed = 0;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        if (_animator == null) _animator = GetComponentInParent<Animator>();
    }

    void Start()
    {
        _chickensFed = 0; 
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

            if (_feedTimer >= feedDuration)
            {
                // 🌟 Pasamos el objeto "other" (el plato) para poder vaciarlo
                Feed(other.gameObject);
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

            if (_animator != null)
            {
                _animator.SetBool("isEating", false);
            }
        }
    }

    // Modificado para recibir el objeto del plato lleno
    void Feed(GameObject foodObject)
    {
        _hasBeenFed = true;
        _isFeeding = false;
        _chickensFed++;

        Debug.Log($"[FeedZone] ✓ {animalName} alimentado con éxito.");

        if (_animator != null)
        {
            _animator.SetBool("isEating", false); 
        }

        // 🌟 NUEVO: TRUCO DE VACIAZO VISUAL DEL PLATO
        VaciarPlatoVisual(foodObject);

        // Actualizamos la interfaz
        ObjectivesUI ui = Object.FindFirstObjectByType<ObjectivesUI>();
        if (ui != null)
        {
            ui.FeedAnimal(); 
        }
    }

    // Intercambia los platos o mallas
    void VaciarPlatoVisual(GameObject fullPlate)
    {
        if (fullPlate == null) return;

        // Buscamos el objeto raíz del plato por si acaso chocó un hijo colisionador
        GameObject rootPlate = fullPlate.transform.root.gameObject;

        if (emptyVisualPrefab != null)
        {
            // 1. Instanciamos el plato vacío en la misma posición y rotación exacta que tiene el lleno en la mano del jugador
            GameObject emptyPlate = Instantiate(emptyVisualPrefab, rootPlate.transform.position, rootPlate.transform.rotation);
            
            // Si el plato estaba enganchado a la mano de VR (tiene un padre), lo enganchamos al mismo sitio
            if (rootPlate.transform.parent != null)
            {
                emptyPlate.transform.SetParent(rootPlate.transform.parent);
            }

            Debug.Log("[FeedZone] ¡Plato lleno cambiado por el plato vacío con éxito!");
        }

        // 2. Destruimos el plato lleno de la escena para que desaparezca
        Destroy(rootPlate);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}