using UnityEngine;

public class SpongeCleaner : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pig"))
        {
            PigCleaner pig = other.GetComponent<PigCleaner>();

            if (pig != null)
            {
                pig.Clean();
            }
        }
    }
}