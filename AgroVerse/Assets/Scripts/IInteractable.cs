public enum InteractionType
{
    Grab,
    Pinch,
    Proximity
}

public interface IInteractable
{
    void OnInteract(InteractionType type);
    bool IsInteractable();
}