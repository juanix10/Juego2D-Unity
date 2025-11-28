using UnityEngine;

public class MovCamara : MonoBehaviour
{
    public Transform objetivo;
    public float velocidadCamara = 0.1f;
    public Vector3 desplazamiento = new Vector3(0, 8, -18);

    private bool primeraActualizacion = true;

    private void Start()
    {
        if (objetivo != null)
        {
            // Forzar posición inicial de la cámara justo al iniciar
            transform.position = objetivo.position + desplazamiento;
        }
    }

    private void LateUpdate()
    {
        if (objetivo == null) return;

        Vector3 posicionDeseada = objetivo.position + desplazamiento;

        if (primeraActualizacion)
        {
            transform.position = posicionDeseada;
            primeraActualizacion = false;
        }
        else
        {
            Vector3 posicionSuavizada = Vector3.Lerp(transform.position, posicionDeseada, velocidadCamara);
            transform.position = posicionSuavizada;
        }
    }
}
