using UnityEngine;
using TMPro; // Asegúrate de tener TextMeshPro

public class UIManager : MonoBehaviour
{
    public static UIManager instancia;
    public TextMeshProUGUI textoPuntos;

    private void Awake()
    {
        if (instancia == null)
            instancia = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        ActualizarPuntos(0);
    }

    public void ActualizarPuntos(int puntos)
    {
        textoPuntos.text = "" + puntos;
    }
}
