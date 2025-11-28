using UnityEngine;

public class sonidoCoin : MonoBehaviour
{
    [Header("Sonido al tomar moneda")]
    public AudioClip sonidoMoneda;
    public float volumen = 1f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("personaje"))
        {
            // Reproduce el sonido en la posición de la cámara para que siempre se escuche
            if (Camera.main != null)
            {
                AudioSource.PlayClipAtPoint(sonidoMoneda, Camera.main.transform.position, volumen);
            }

            gameObject.SetActive(false);
        }
    }
}
