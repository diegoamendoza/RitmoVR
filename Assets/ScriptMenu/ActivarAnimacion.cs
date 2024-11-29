using UnityEngine;
using UnityEngine.UI;

public class ActivarAnimacion : MonoBehaviour
{
    public Button boton; // Referencia al bot�n en la UI.
    public Animator animator; // Referencia al Animator que controla la animaci�n.
    public string nombreTriggerAnimacion; // Nombre del Trigger en el Animator.

    void Start()
    {
        // Verificar que las referencias est�n asignadas correctamente.
        if (boton == null || animator == null || string.IsNullOrEmpty(nombreTriggerAnimacion))
        {
            Debug.LogError("Faltan referencias en el script. Aseg�rate de asignar el bot�n, el Animator y el nombre del Trigger.");
            return;
        }

        // A�adir el listener al bot�n para activar la animaci�n al ser presionado.
        boton.onClick.AddListener(ActivarAnimacionYDesactivar);
    }

    void ActivarAnimacionYDesactivar()
    {
        // Activar el Trigger en el Animator.
        animator.SetTrigger(nombreTriggerAnimacion);

        // Desactivar completamente el objeto del bot�n despu�s de activar la animaci�n.
        boton.gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        // Eliminar el listener para evitar errores si el objeto se destruye.
        boton.onClick.RemoveListener(ActivarAnimacionYDesactivar);
    }
}
