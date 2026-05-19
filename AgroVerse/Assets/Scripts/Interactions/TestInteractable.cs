using UnityEngine;

public class TestInteractable : MonoBehaviour, IInteractable
{
    public bool interactable = true;

    public void OnInteract(InteractionType type)
    {
        Debug.Log(gameObject.name + " interacted with: " + type);

        transform.localScale *= 1.1f;
    }

    public bool IsInteractable()
    {
        return interactable;
    }
}