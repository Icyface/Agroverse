// UdderZoneTrigger.cs — ponlo en el GameObject hijo UdderZone
using UnityEngine;

public class UdderZoneTrigger : MonoBehaviour
{
    private CowMilkingMechanic _milking;

    void Awake()
    {
        _milking = GetComponentInParent<CowMilkingMechanic>();

        if (_milking == null)
            Debug.LogError("[UdderZoneTrigger] No encuentra CowMilkingMechanic en el padre");
    }

    void OnTriggerEnter(Collider other)
    {
        _milking.OnUdderTriggerEnter(other);
    }

    void OnTriggerExit(Collider other)
    {
        _milking.OnUdderTriggerExit(other);
    }
}