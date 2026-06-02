using UnityEngine;

public class SonidoAnimacion : MonoBehaviour
{
    public AudioClip[] sonidos;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void ReproducirSonido()
    {
        if (audioSource != null && sonidos.Length > 0)
        {
            audioSource.PlayOneShot(
                sonidos[Random.Range(0, sonidos.Length)]
            );
        }
    }
}