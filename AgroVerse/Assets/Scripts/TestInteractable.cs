using UnityEngine;

public class TestInteractable : MonoBehaviour, IInteractable
{
    private bool _canInteract = true;

    public void OnInteract(InteractionType type)
    {
        Debug.Log($"[TestInteractable] Interacción recibida: {type} en {gameObject.name}");
    }

    public bool IsInteractable()
    {
        return _canInteract;
    }
}