using UnityEngine;
using System.Collections;

public class SonidoCerdo : MonoBehaviour
{
    public AudioClip[] sonidos;
    public float tiempoMin = 8f;
    public float tiempoMax = 20f;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(HacerSonidos());
    }

    IEnumerator HacerSonidos()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(tiempoMin, tiempoMax));

            if (sonidos.Length > 0)
            {
                audioSource.PlayOneShot(
                    sonidos[Random.Range(0, sonidos.Length)]
                );
            }
        }
    }
}