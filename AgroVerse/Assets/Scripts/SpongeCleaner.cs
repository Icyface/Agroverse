using UnityEngine;

public class SpongeCleaner : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pig"))
        {
            PigCleaner pig = other.GetComponentInParent<PigCleaner>();

            if (pig != null)
            {
                pig.CleanStep(); // suma 1 pasada
            }
        }
    }
}