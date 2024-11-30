using UnityEngine;
using UnityEngine.UI;

public class ActivarContenedorYDesactivarBoton : MonoBehaviour
{
    public Button boton; // Referencia al botón en la UI.
    public GameObject contenedor; // Referencia al objeto que se activará o desactivará.

    void Start()
    {
        // Verificar que las referencias estén asignadas correctamente.
        if (boton == null || contenedor == null)
        {
            Debug.LogError("Faltan referencias en el script. Asegúrate de asignar el botón y el objeto contenedor.");
            return;
        }

        // Añadir el listener al botón para alternar la activación del contenedor.
        boton.onClick.AddListener(ToggleContenedorYDesactivarBoton);
    }

    void ToggleContenedorYDesactivarBoton()
    {
        // Alternar el estado activo del objeto contenedor.
        contenedor.SetActive(!contenedor.activeSelf);

        // Desactivar el botón después de presionarlo.
        boton.gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        // Eliminar el listener para evitar errores si el objeto se destruye.
        boton.onClick.RemoveListener(ToggleContenedorYDesactivarBoton);
    }
}
