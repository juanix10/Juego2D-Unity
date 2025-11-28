using UnityEngine;

public class DanoEnemigo : MonoBehaviour
{
    [Header("Configuracion de dano")]
    public float danoPorGolpe = 25f;
    public float tiempoEntreGolpes = 1f;

    private float tiempoUltimoGolpe;

    private void OnTriggerEnter2D(Collider2D other)
    {
        personaje p = other.GetComponent<personaje>();
        if (p != null)
        {
            AplicarDano(p, transform.position);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        personaje p = collision.gameObject.GetComponent<personaje>();
        if (p != null)
        {
            AplicarDano(p, transform.position);
        }
    }

    private void AplicarDano(personaje p, Vector2 posicionGolpe)
    {
        if (Time.time - tiempoUltimoGolpe < tiempoEntreGolpes)
            return;

        tiempoUltimoGolpe = Time.time;

        // Ahora se llama a la función del personaje
        p.RecibeDanio(posicionGolpe, danoPorGolpe);
    }
}
