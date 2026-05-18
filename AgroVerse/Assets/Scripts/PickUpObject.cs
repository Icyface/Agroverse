using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable))]
public class PickupObject : MonoBehaviour, IInteractable
{
    [Header("Estado")]
    public bool canPickup = true;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable _grabInteractable;
    private Rigidbody _rb;
    private bool _isHeld = false;

    void Awake()
    {
        _grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        _rb = GetComponent<Rigidbody>();

        // Suscribirse a los eventos del XRI
        _grabInteractable.selectEntered.AddListener(OnGrabbed);
        _grabInteractable.selectExited.AddListener(OnReleased);
    }

    void OnDestroy()
    {
        _grabInteractable.selectEntered.RemoveListener(OnGrabbed);
        _grabInteractable.selectExited.RemoveListener(OnReleased);
    }

    // Llamado por XRI cuando se agarra el objeto
    void OnGrabbed(SelectEnterEventArgs args)
    {
        _isHeld = true;

        // Detectar si viene de hand tracking o controller
        bool isHandTracking = args.interactorObject is UnityEngine.XR.Interaction.Toolkit.Interactors.XRDirectInteractor &&
                              args.interactorObject.transform.name.ToLower().Contains("hand");

        InteractionType type = isHandTracking ? InteractionType.Pinch : InteractionType.Grab;
        OnInteract(type);

        Debug.Log($"[PickupObject] Agarrado ({type}): {gameObject.name}");
    }

    // Llamado por XRI cuando se suelta el objeto
    void OnReleased(SelectExitEventArgs args)
    {
        _isHeld = false;
        Debug.Log($"[PickupObject] Soltado: {gameObject.name}");
    }

    // ?? IInteractable ??????????????????????????????
    public void OnInteract(InteractionType type)
    {
        // Las subclases o scripts externos pueden sobreescribir comportamiento
        // Por ejemplo, el huevo llama a EggPickup.OnInteract
    }

    public bool IsInteractable()
    {
        return canPickup && !_isHeld;
    }

    public bool IsHeld => _isHeld;

}