using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cargar escenas.

public class CambiarEscena : MonoBehaviour
{
    // M�todo que se llama cuando se presiona el bot�n
    public void CambiarAEscena(string nombreEscena)
    {
        // Carga la escena especificada por nombre
        SceneManager.LoadScene("Demo");
    }
    public void CambiarAEscenaDos(string nombreEscena)
    {
        // Carga la escena especificada por nombre
        SceneManager.LoadScene("Demo");
    }
    public void CambiarAEscenaTres(string nombreEscena)
    {
        // Carga la escena especificada por nombre
        SceneManager.LoadScene("Demo");
    }
}
