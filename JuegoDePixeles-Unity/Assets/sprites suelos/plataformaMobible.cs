using UnityEngine;

public class plataforma3 : MonoBehaviour
{
    public float velocidad = 2f;         
    public float limiteSuperior = 4f;    

    private Vector3 posicionInicial;      
    private int direccion = 1;            

    void Start()
    {
        // Guardar la posición inicial de la plataforma
        posicionInicial = transform.position;
    }

    void Update()
    {
        // Movimiento vertical (sube o baja)
        transform.Translate(Vector2.up * direccion * velocidad * Time.deltaTime);

        // Cambiar dirección al llegar a los límites
        if (transform.position.y >= posicionInicial.y + limiteSuperior)
        {
            direccion = -1; // baja
        }
        else if (transform.position.y <= posicionInicial.y)
        {
            direccion = 1; // sube
            // Asegura que no baje más de la posición inicial
            transform.position = new Vector3(transform.position.x, posicionInicial.y, transform.position.z);
        }
    }
}
