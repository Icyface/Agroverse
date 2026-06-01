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

    // Contador compartido entre todos los FeedZone
    private static int _chickensFed = 0;
    private static int _chickensRequired = 2;

    void OnTriggerEnter(Collider other)
    {
        if (_hasBeenFed) return;

        FoodContainer food = other.GetComponent<FoodContainer>();

        if (food != null && food.IsHeld && food.foodType == acceptedFoodType)
        {
            _isFeeding = true;
            _feedTimer = 0f;
            Debug.Log($"[FeedZone] Alimentando a {animalName}... mantén la comida cerca.");
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (_hasBeenFed || !_isFeeding) return;

        FoodContainer food = other.GetComponent<FoodContainer>();

        if (food != null && food.IsHeld && food.foodType == acceptedFoodType)
        {
            _feedTimer += Time.deltaTime;
            Debug.Log($"[FeedZone] {animalName}: {_feedTimer:F1} / {feedDuration}s");

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
        if (food != null)
        {
            _isFeeding = false;
            _feedTimer = 0f;
            Debug.Log($"[FeedZone] {animalName}: alimentación cancelada.");
        }
    }

    void Feed()
    {
        _hasBeenFed = true;
        _isFeeding = false;
        _chickensFed++;

        Debug.Log($"[FeedZone] {animalName} alimentado. Pollos: {_chickensFed} / {_chickensRequired}");

        if (_chickensFed >= _chickensRequired)
        {
            Debug.Log("[FeedZone] ¡Todos los pollos alimentados!");
            TaskManager.Instance?.CompleteTask("alimentar_chicken");
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}