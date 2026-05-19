using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;

public class PlayerInteract : MonoBehaviour
{
    [Header("Raycast Settings")]
    public float interactDistance = 3f;

    [Header("References")]
    public Transform rightHand;
    public Transform leftHand;

    private InputDevice rightController;
    private InputDevice leftController;

    void Start()
    {
        InitializeControllers();
    }

    void InitializeControllers()
    {
        List<InputDevice> devices = new List<InputDevice>();

        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, devices);

        if (devices.Count > 0)
        {
            rightController = devices[0];
        }

        devices.Clear();

        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, devices);

        if (devices.Count > 0)
        {
            leftController = devices[0];
        }
    }

    void Update()
    {
        HandleGripInput();
        HandlePinchInput();
    }

    void HandleGripInput()
    {
        if (rightController.TryGetFeatureValue(CommonUsages.gripButton, out bool gripPressed))
        {
            if (gripPressed)
            {
                TryInteract(rightHand, InteractionType.Grip);
            }
        }
    }

    void HandlePinchInput()
    {
        if (rightController.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerPressed))
        {
            if (triggerPressed)
            {
                TryInteract(rightHand, InteractionType.Pinch);
            }
        }
    }

    void TryInteract(Transform origin, InteractionType type)
    {
        Ray ray = new Ray(origin.position, origin.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            IInteractable interactable =
                hit.collider.GetComponent<IInteractable>();

            if (interactable != null && interactable.IsInteractable())
            {
                interactable.OnInteract(type);

                Debug.Log("Interacted with: " + hit.collider.name);
            }
        }
    }
}