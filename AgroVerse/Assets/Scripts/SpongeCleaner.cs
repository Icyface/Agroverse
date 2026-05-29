using UnityEngine;

public class SpongeCleaner : MonoBehaviour
{
    public ParticleSystem foamParticles;

    private void OnTriggerEnter(Collider other)
        {
            Debug.Log("TOCANDO ALGO");

            if (other.CompareTag("Pig"))
            {
                Debug.Log("TOCANDO CERDO");

                PigCleaner pig = other.GetComponentInParent<PigCleaner>();

                if (pig != null)
                {
                    Debug.Log("ENCONTRO SCRIPT");

                    pig.UseSponge();

                    if (foamParticles != null)
                    {
                        Debug.Log("PLAY FOAM");
                        foamParticles.Play();
                    }
                }
            }
        }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pig"))
        {
            if (foamParticles != null)
            {
                foamParticles.Stop();
            }
        }
    }
    
}