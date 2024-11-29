using UnityEngine;
using UnityEngine.UI;

public class ActivarAnimacion : MonoBehaviour
{
    public Button boton; // Referencia al botón en la UI.
    public Animator animator; // Referencia al Animator que controla la animación.
    public string nombreTriggerAnimacion; // Nombre del Trigger en el Animator.

    void Start()
    {
        // Verificar que las referencias estén asignadas correctamente.
        if (boton == null || animator == null || string.IsNullOrEmpty(nombreTriggerAnimacion))
        {
            Debug.LogError("Faltan referencias en el script. Asegúrate de asignar el botón, el Animator y el nombre del Trigger.");
            return;
        }

        // Añadir el listener al botón para activar la animación al ser presionado.
        boton.onClick.AddListener(ActivarAnimacionYDesactivar);
    }

    void ActivarAnimacionYDesactivar()
    {
        // Activar el Trigger en el Animator.
        animator.SetTrigger(nombreTriggerAnimacion);

        // Desactivar completamente el objeto del botón después de activar la animación.
        boton.gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        // Eliminar el listener para evitar errores si el objeto se destruye.
        boton.onClick.RemoveListener(ActivarAnimacionYDesactivar);
    }
}
