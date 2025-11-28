using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PausaManager : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject menuPausa;
    public AudioSource musicaFondo;
    public GameObject jugador;

    private bool juegoPausado = false;
    private Keyboard teclado;
    private MonoBehaviour movimientoJugador;

    private void Awake()
    {
        teclado = Keyboard.current;

        if (jugador != null)
            movimientoJugador = jugador.GetComponent<MonoBehaviour>();
    }

    private void Update()
    {
        if (teclado.escapeKey.wasPressedThisFrame)
        {
            if (juegoPausado)
                Reanudar();
            else
                Pausar();
        }
    }

    public void Reanudar()
    {
        menuPausa.SetActive(false);
        Time.timeScale = 1f;
        juegoPausado = false;

        if (musicaFondo != null)
            musicaFondo.Play();

        if (movimientoJugador != null)
            movimientoJugador.enabled = true;
    }

    public void Pausar()
    {
        menuPausa.SetActive(true);
        Time.timeScale = 0f;
        juegoPausado = true;

        if (musicaFondo != null)
            musicaFondo.Pause();

        if (movimientoJugador != null)
            movimientoJugador.enabled = false;
    }

    // NUEVA FUNCIÓN PARA VOLVER AL MENÚ PRINCIPAL
    public void VolverAlMenu()
    {
        // Restaurar tiempo del juego
        Time.timeScale = 1f;

        Debug.Log("Regresando al menú principal...");

        // Cambiar a la escena del menú
        SceneManager.LoadScene("Menu");  // Cambia el nombre por el de tu escena de menú
    }
}
