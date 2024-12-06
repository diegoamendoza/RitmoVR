using UnityEngine;
using UnityEngine.UI;

public class ActivarContenedorYDesactivarBoton : MonoBehaviour
{
    public Button boton; // Referencia al bot�n en la UI.
    public GameObject contenedor; // Referencia al objeto que se activar� o desactivar�.

    void Start()
    {
        // Verificar que las referencias est�n asignadas correctamente.
        if (boton == null || contenedor == null)
        {
            Debug.LogError("Faltan referencias en el script. Aseg�rate de asignar el bot�n y el objeto contenedor.");
            return;
        }

        // A�adir el listener al bot�n para alternar la activaci�n del contenedor.
        boton.onClick.AddListener(ToggleContenedorYDesactivarBoton);
    }

    void ToggleContenedorYDesactivarBoton()
    {
        // Alternar el estado activo del objeto contenedor.
        contenedor.SetActive(!contenedor.activeSelf);

        // Desactivar el bot�n despu�s de presionarlo.
        boton.gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        // Eliminar el listener para evitar errores si el objeto se destruye.
        boton.onClick.RemoveListener(ToggleContenedorYDesactivarBoton);
    }
}
