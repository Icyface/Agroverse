using UnityEngine;

public class MangueraCerdo : MonoBehaviour
{
    public ParticleSystem waterParticles;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;

    void Start()
    {
        grabInteractable = GetComponentInParent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

        if (waterParticles != null)
        {
            waterParticles.Stop();
        }
    }

    void Update()
    {
        if (grabInteractable != null && grabInteractable.isSelected)
        {
            if (!waterParticles.isPlaying)
                waterParticles.Play();
        }
        else
        {
            if (waterParticles.isPlaying)
                waterParticles.Stop();
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("PARTICULA TOCA: " + other.name);

        if (other.CompareTag("Pig"))
        {
            PigCleaner pig = other.GetComponentInParent<PigCleaner>();

            if (pig != null)
            {
                Debug.Log("CERDO MOJADO");
                pig.UseWater();
            }
        }
    }
}