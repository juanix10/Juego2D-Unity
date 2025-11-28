using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSistem : MonoBehaviour
{
    public void Jugar()
    {
        // Cargar la siguiente escena del índice actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Salir()
    {
        Debug.Log("Saliendo del juego...");

        // Cierra el juego (solo funciona en build)
        Application.Quit();

        // Para pruebas en el editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
