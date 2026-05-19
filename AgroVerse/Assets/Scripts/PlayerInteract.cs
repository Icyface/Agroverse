using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerInteract : MonoBehaviour
{
    [Header("Configuración")]
    public float interactDistance = 1.5f;
    public LayerMask interactableLayer;

    private XRBaseController _controller;

    void Awake()
    {
        _controller = GetComponent<XRBaseController>();
    }

    void Update()
    {
        DetectNearby();
    }

    void DetectNearby()
    {
        // Detecta objetos interactuables en un radio alrededor de la mano
        Collider[] hits = Physics.OverlapSphere(transform.position, interactDistance, interactableLayer);

        foreach (Collider hit in hits)
        {
            IInteractable interactable = hit.GetComponent<IInteractable>();
            if (interactable != null && interactable.IsInteractable())
            {
                // Destacar visualmente (opcional por ahora)
                // El grab real lo gestiona XRGrabInteractable en la tarea 3
            }
        }
    }

    // Llamado externamente cuando se hace grab con controller
    public void TriggerInteract(IInteractable target)
    {
        if (target != null && target.IsInteractable())
        {
            target.OnInteract(InteractionType.Grab);
        }
    }

    // Llamado externamente cuando se hace pinch con hand tracking
    public void TriggerPinch(IInteractable target)
    {
        if (target != null && target.IsInteractable())
        {
            target.OnInteract(InteractionType.Pinch);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactDistance);
    }
}