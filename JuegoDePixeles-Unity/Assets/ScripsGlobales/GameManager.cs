using UnityEngine;
using UnityEngine.InputSystem; // Necesario para detectar clic del mouse
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instancia;

    [Header("Configuración de puntaje")]
    public int valorMoneda = 1000;
    public GameObject prefabTextoFlotante;

    public int puntos = 0;

    private Vector3[] posicionesMonedas;
    private GameObject[] monedas;

    private Vector3[] posicionesEnemigos;
    private GameObject[] enemigos;

    // 🔹 NUEVO: Referencias para reinicio visual y música
    [Header("Configuración de reinicio")]
    public CanvasGroup panelGameOver;  //  Debe ser tipo CanvasGroup
    public AudioSource musica; 
    
    private bool esperandoReinicio = false;

    private void Awake()
    {
        if (instancia == null) instancia = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        RefreshMonedas();
        RefreshEnemigos();

        if (panelGameOver != null)
        {
            panelGameOver.alpha = 0;
            panelGameOver.interactable = false;
            panelGameOver.blocksRaycasts = false;
        }
    }

    private void Update()
    {
        // Si el jugador está en GameOver, espera clic para reiniciar
        if (esperandoReinicio && Mouse.current.leftButton.wasPressedThisFrame)
        {
            ReiniciarJuego();
            esperandoReinicio = false;
        }
    }

    // ----------------------------
    //  MONEDAS
    // ----------------------------
    public void RefreshMonedas()
    {
        monedas = GameObject.FindGameObjectsWithTag("Moneda");
        posicionesMonedas = new Vector3[monedas.Length];

        for (int i = 0; i < monedas.Length; i++)
            posicionesMonedas[i] = monedas[i].transform.position;
    }

    public void MonedaTomada(Vector3 posicion)
    {
        SumarPuntos(valorMoneda);

        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas != null)
        {
            GameObject texto = Instantiate(prefabTextoFlotante, canvas.transform);
            texto.transform.position = Camera.main.WorldToScreenPoint(posicion);
            TextoFlotante tf = texto.GetComponent<TextoFlotante>();
            if (tf != null)
                tf.ConfigurarTexto("+" + valorMoneda, Color.yellow);
        }
    }

    public void SumarPuntos(int cantidad)
    {
        puntos += cantidad;
        Debug.Log("Puntos " + puntos);

        if (UIManager.instancia != null)
            UIManager.instancia.ActualizarPuntos(puntos);
    }

    // ----------------------------
    //  ENEMIGOS
    // ----------------------------
    public void RefreshEnemigos()
    {
        enemigos = GameObject.FindGameObjectsWithTag("enemigo");
        posicionesEnemigos = new Vector3[enemigos.Length];

        for (int i = 0; i < enemigos.Length; i++)
            posicionesEnemigos[i] = enemigos[i].transform.position;
    }

    private void ReiniciarEnemigos()
{
    if (enemigos == null || enemigos.Length == 0)
        RefreshEnemigos();

    Transform jugador = GameObject.FindGameObjectWithTag("personaje")?.transform;

    for (int i = 0; i < enemigos.Length; i++)
    {
        if (enemigos[i] != null)
        {
            enemigos[i].SetActive(true);

            // ⚡ Si el enemigo tiene script Murcielago, usa su método Reiniciar()
            Murcielago murcielago = enemigos[i].GetComponent<Murcielago>();
            if (murcielago != null)
            {
                murcielago.Reiniciar(jugador);
                continue; // Ya se encargó el propio script
            }

            // Si no tiene script Murcielago (otro tipo de enemigo)
            enemigos[i].transform.position = posicionesEnemigos[i];

            Animator anim = enemigos[i].GetComponent<Animator>();
            if (anim != null)
            {
                anim.Rebind();
                anim.Update(0f);
            }

            Rigidbody2D rb = enemigos[i].GetComponent<Rigidbody2D>();
            Collider2D col = enemigos[i].GetComponent<Collider2D>();

            if (rb != null)
            {
                rb.isKinematic = false;
                rb.linearVelocity = Vector2.zero;
            }

            if (col != null)
                col.enabled = true;
        }
    }
}


    // ----------------------------
    //  REINICIAR JUEGO
    // ----------------------------
    public void ReiniciarJuego()
    {
        Time.timeScale = 1f;

        puntos = 0;
        if (UIManager.instancia != null)
            UIManager.instancia.ActualizarPuntos(puntos);

        // Reiniciar monedas
        if (monedas == null || monedas.Length == 0)
            RefreshMonedas();

        for (int i = 0; i < monedas.Length; i++)
        {
            if (monedas[i] != null)
            {
                monedas[i].SetActive(true);
                monedas[i].transform.position = posicionesMonedas[i];
            }
        }

        // Reiniciar enemigos
        ReiniciarEnemigos();

        // Reiniciar música
        if (musica != null)
        {
            musica.time = 0f;
            musica.Play();
        }

        // Ocultar panel Game Over
        if (panelGameOver != null)
        {
            panelGameOver.alpha = 0;
            panelGameOver.interactable = false;
            panelGameOver.blocksRaycasts = false;
        }
    }

    // ----------------------------
    //  GAME OVER
    // ----------------------------
    public void MostrarGameOver()
    {
        if (musica != null)
            musica.Stop();

        if (panelGameOver != null)
        {
            panelGameOver.alpha = 1;
            panelGameOver.interactable = true;
            panelGameOver.blocksRaycasts = true;
        }

        esperandoReinicio = true;
        Time.timeScale = 0f;
    }
}
