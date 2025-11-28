using UnityEngine;
using TMPro;

public class TextoFlotante : MonoBehaviour
{
    public TextMeshProUGUI textoTMP;
    public float duracion = 1f;
    public Vector3 offset = new Vector3(0, 50, 0); // movimiento hacia arriba (en píxeles)
    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void ConfigurarTexto(string texto, Color color)
    {
        if (textoTMP == null)
            textoTMP = GetComponentInChildren<TextMeshProUGUI>();

        textoTMP.text = texto;
        textoTMP.color = color;

        Destroy(gameObject, duracion);
    }

    void Update()
    {
        if (rect != null)
        {
            // Convertimos el offset a Vector2 para evitar el error
            rect.anchoredPosition += (Vector2)(offset * Time.deltaTime);
        }
    }
}
