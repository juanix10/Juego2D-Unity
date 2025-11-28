using UnityEngine;

public class sonidoAtaque : MonoBehaviour
{
    [Header("Sonidos")]
    public AudioClip sonidoEspada;      // Sonido del ataque de la espada
    public AudioClip sonidoRecibirDaño; // Sonido cuando el jugador recibe daño
    public float volumen = 1f;

    private AudioSource audioSource;

    void Start()
    {
        // Busca o crea un AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
    }

    // Llamada por Animation Event para el ataque
    public void ReproducirSonidoAtaque()
    {
        if (sonidoEspada != null)
        {
            audioSource.PlayOneShot(sonidoEspada, volumen);
        }
    }

    // Llamada para cuando el jugador recibe daño
    public void ReproducirSonidoRecibirDaño()
    {
        if (sonidoRecibirDaño != null)
        {
            audioSource.PlayOneShot(sonidoRecibirDaño, volumen);
        }
    }
}
