using UnityEngine;


public class MangueraCerdo : MonoBehaviour
{
    public ParticleSystem waterParticles;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;

    void Start()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

        if (waterParticles != null)
        {
            waterParticles.Stop(); // empieza apagado
        }
    }

    void Update()
    {
        if (grabInteractable.isSelected)
        {
            //EST�S COGIENDO LA MANGUERA
            if (!waterParticles.isPlaying)
            {
                waterParticles.Play();
            }
        }
        else
        {
            //NO LA TIENES EN LA MANO
            if (waterParticles.isPlaying)
            {
                waterParticles.Stop();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // solo moja si la est�s cogiendo Y tocas el cerdo
        if (grabInteractable.isSelected && other.CompareTag("Pig"))
        {
            PigCleaner pig = other.GetComponentInParent<PigCleaner>();

            if (pig != null)
            {
                pig.UseWater();
            }
        }
    }
}

