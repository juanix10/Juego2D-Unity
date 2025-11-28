using UnityEngine;
using System.Collections;

public class Muerte : MonoBehaviour
{
    public float tiempoReaparicion = 1.5f; // tiempo en segundos antes de reaparecer

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("muerte")) return; // evita auto-colisión

        // Si el objeto que tocó tiene el tag "muerte", busca al personaje en la colisión
        personaje p = collision.collider.GetComponent<personaje>();
        if (p != null)
        {
            StartCoroutine(MorirYReaparecer(p));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("muerte")) return;

        personaje p = other.GetComponent<personaje>();
        if (p != null)
        {
            StartCoroutine(MorirYReaparecer(p));
        }
    }

    private IEnumerator MorirYReaparecer(personaje p)
    {
        if (p.barraVida != null)
        {
            // Vacía la barra de vida instantáneamente
            p.barraVida.RestarVida(p.barraVida.vidaMaxima);
        }

        // 🔴 Desactiva momentáneamente al personaje (para simular "muerte")
        p.gameObject.SetActive(false);

        // Espera el tiempo de reaparición
        yield return new WaitForSeconds(tiempoReaparicion);

        // 🔵 Reactiva al personaje y restablece su posición
        p.gameObject.SetActive(true);
        p.ReiniciarPosicion();
    }
}
