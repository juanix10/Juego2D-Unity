using UnityEngine;

public class moneda : MonoBehaviour
{
    public int puntosMoneda = 1000;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("personaje"))
        {
            // Llama al método que muestra el texto y suma puntos
            GameManager.instancia.MonedaTomada(collision.transform.position + new Vector3(0, 1.5f, 0));

            // Desactiva la moneda (como si desapareciera)
            gameObject.SetActive(false);
        }
    }
}
