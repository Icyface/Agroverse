using UnityEngine;

public class FeedZone : MonoBehaviour
{
    [Header("Configuración")]
    public string acceptedFoodType = "generic";
    public string animalName = "Animal";
    private bool _hasBeenFed = false;

    void OnTriggerEnter(Collider other)
    {
        if (_hasBeenFed) return;

        FoodContainer food = other.GetComponent<FoodContainer>();

        if (food != null && food.IsHeld && food.foodType == acceptedFoodType)
        {
            Feed(food);
        }
    }

    void Feed(FoodContainer food)
    {
        _hasBeenFed = true;
        food.isEmpty = true;

        Debug.Log($"[FeedZone] {animalName} alimentado con {food.foodType}.");

        // Avisar al TaskManager
        TaskManager.Instance?.CompleteTask("alimentar_" + animalName.ToLower());

        // Aquí Joel conectará con AnimalReaction:
        // GetComponent<AnimalReaction>()?.SetState(AnimalState.Alimentado);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}