public enum InteractionType
{
    Grip,
    Pinch,
    Proximity
}

public interface IInteractable
{
    void OnInteract(InteractionType type);

    bool IsInteractable();
}