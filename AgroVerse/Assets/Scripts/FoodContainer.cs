using UnityEngine;

public class FoodContainer : PickupObject
{
    [Header("Comida")]
    public string foodType = "generic";

    public override void OnInteract(InteractionType type)
    {
        base.OnInteract(type);
        Debug.Log($"[FoodContainer] Recipiente agarrado ({type}), tipo: {foodType}");
    }
}