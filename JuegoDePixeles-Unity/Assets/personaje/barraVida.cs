using UnityEngine;
using UnityEngine.UI;
using System.Collections; // Importante para usar IEnumerator

public class BarraVida : MonoBehaviour
{
    [Header("Referencia a la imagen de la barra")]
    public Image barra;  // Asigna aquí tu Image de la UI (la que tiene el fill)

    [Header("Configuración de vida")]
    public float vidaMaxima = 100f;
    public float vidaActual;

    void Start()
    {
        vidaActual = vidaMaxima;
        ActualizarBarra();
    }

    public void RestarVida(float cantidad)
    {
        vidaActual -= cantidad;
        vidaActual = Mathf.Clamp(vidaActual, 0, vidaMaxima);
        ActualizarBarra();
    }

    public void RestaurarVida()
    {
        vidaActual = vidaMaxima;
        ActualizarBarra();
    }

    public void ActualizarBarra()
    {
        if (barra != null)
            barra.fillAmount = vidaActual / vidaMaxima;
    }

    public bool EstaMuerto()
    {
        return vidaActual <= 0;
    }

    // 👇 Corutina para restaurar la vida con retardo
    public IEnumerator RestaurarVidaConRetraso(float segundos)
    {
        yield return new WaitForSeconds(segundos);
        RestaurarVida();
    }
}

