using UnityEngine;

public class SonidoAnimacion : MonoBehaviour
{
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void ReproducirSonido()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}