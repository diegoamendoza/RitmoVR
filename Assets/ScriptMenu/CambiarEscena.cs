using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cargar escenas.

public class CambiarEscena : MonoBehaviour
{
    // Método que se llama cuando se presiona el botón
    public void CambiarAEscena(string nombreEscena)
    {
        // Carga la escena especificada por nombre
        SceneManager.LoadScene("LukaLuka");
    }
    public void CambiarAEscenaDos(string nombreEscena)
    {
        // Carga la escena especificada por nombre
        SceneManager.LoadScene("Tutorial");
    }
    public void CambiarAEscenaTres(string nombreEscena)
    {
        // Carga la escena especificada por nombre
        SceneManager.LoadScene("Menu");
    }
}
