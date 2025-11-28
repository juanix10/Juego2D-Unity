using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameOverManager : MonoBehaviour
{
    public CanvasGroup panelGameOver; // 🔹 Asigna el panel del UI en el inspector
    public AudioSource musica;        //  Música de fondo

    private bool esperandoReinicio = false;

    void Start()
    {
        if (panelGameOver != null)
        {
            panelGameOver.alpha = 0;
            panelGameOver.interactable = false;
            panelGameOver.blocksRaycasts = false;
        }
    }

    public void MostrarGameOver()
    {
        if (musica != null)
            musica.Stop(); //  Detiene la música actual

        if (panelGameOver != null)
        {
            panelGameOver.alpha = 1;
            panelGameOver.interactable = true;
            panelGameOver.blocksRaycasts = true;
        }

        esperandoReinicio = true;
        Time.timeScale = 0f; //  Pausa el juego
    }

    void Update()
    {
        if (esperandoReinicio && Mouse.current.leftButton.wasPressedThisFrame)
        {
            ReiniciarJuego();
        }
    }

    void ReiniciarJuego()
    {
        Time.timeScale = 1f;
        esperandoReinicio = false;

        if (musica != null)
        {
            musica.time = 0f;  // Reinicia desde el inicio
            musica.Play();
        }

        if (panelGameOver != null)
        {
            panelGameOver.alpha = 0;
            panelGameOver.interactable = false;
            panelGameOver.blocksRaycasts = false;
        }

        //  Reinicia el jugador
        personaje jugador = FindObjectOfType<personaje>();
        if (jugador != null)
        {
            jugador.ReiniciarPosicion();
        }
    }
}
