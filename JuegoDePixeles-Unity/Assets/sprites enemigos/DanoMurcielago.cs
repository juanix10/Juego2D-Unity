using UnityEngine;

public class DanoMurcielago : MonoBehaviour
{
    [Header("Configuración de daño")]
    public float danoPorGolpe = 20f;
    public float tiempoEntreGolpes = 1f;
    private float tiempoUltimoGolpe;

    private void OnTriggerEnter2D(Collider2D other)
    {
        personaje p = other.GetComponent<personaje>();
        if (p != null)
            AplicarDano(p, transform.position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        personaje p = collision.gameObject.GetComponent<personaje>();
        if (p != null)
            AplicarDano(p, transform.position);
    }

    private void AplicarDano(personaje p, Vector2 posicionGolpe)
    {
        if (Time.time - tiempoUltimoGolpe < tiempoEntreGolpes)
            return;

        tiempoUltimoGolpe = Time.time;
        p.RecibeDanio(posicionGolpe, danoPorGolpe);
    }
}
