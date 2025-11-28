using UnityEngine;

public class sonidoMuerteEnemigo : MonoBehaviour
{
    [Header("Sonido al morir el enemigo")]
    public AudioClip sonidoMuerte;  //  Asigna el sonido en el Inspector
    public float volumen = 1f;      // Volumen (0–1)
    private AudioSource audioSource;

    private void Start()
    {
        // Asegura que haya un AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
    }

    // 👇 Esta función será llamada desde la animación
    public void ReproducirSonidoMuerte()
    {
        if (sonidoMuerte != null)
            audioSource.PlayOneShot(sonidoMuerte, volumen);
    }
}
