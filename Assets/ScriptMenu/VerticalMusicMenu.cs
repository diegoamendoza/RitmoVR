using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class VerticalMusicMenu : MonoBehaviour
{
    public Button[] botones; // Array de botones, deben estar en el orden l�gico.
    public Button botonArriba; // Bot�n para mover hacia arriba.
    public Button botonAbajo; // Bot�n para mover hacia abajo.
    public Button botonActivar; // Bot�n espec�fico que activar� las im�genes.
    public Image[] imagenesAsociadas; // Array de im�genes asociadas a cada bot�n.
    public GameObject[] bordes; // Array de bordes asociados a cada bot�n.
    public float desplazamiento = 100f; // Distancia entre los botones en el eje Y.
    private int indiceSeleccionado = 0; // El �ndice del bot�n "fijo" al centro.
    public Color colorOscuro = new Color(0.3f, 0.3f, 0.3f); // Color para botones no seleccionados.
    public Color colorBrillante = Color.white; // Color para el bot�n seleccionado.
    public float tiempoAnimacion = 0.5f; // Tiempo de animaci�n para el desplazamiento.
    private Vector3[] posicionesOriginales; // Para almacenar las posiciones originales de los botones.
    private bool enAnimacion = false; // Flag para saber si est� en animaci�n.
    private bool imagenesHabilitadas = false; // Flag para saber si las im�genes est�n habilitadas.

    void Start()
    {
        // Validar que cada bot�n tenga una imagen y un borde asociado.
        if (botones.Length != imagenesAsociadas.Length || botones.Length != bordes.Length)
        {
            Debug.LogError("El n�mero de botones no coincide con el n�mero de im�genes o bordes asociados.");
            return;
        }

        // Almacena las posiciones originales y organiza los botones.
        posicionesOriginales = new Vector3[botones.Length];
        OrganizarBotones();
        ActualizarVisual();

        // Conecta los botones interactivos con sus acciones.
        botonArriba.onClick.AddListener(() => {
            if (!enAnimacion && imagenesHabilitadas) MoverBotones(-1); // Mover hacia arriba.
        });

        botonAbajo.onClick.AddListener(() => {
            if (!enAnimacion && imagenesHabilitadas) MoverBotones(1); // Mover hacia abajo.
        });

        // Conectar el bot�n que habilitar� las im�genes.
        if (botonActivar != null)
        {
            botonActivar.onClick.AddListener(ActivarImagenes);
        }

        // Asegurarse de que las im�genes y bordes est�n desactivados al inicio.
        DesactivarTodasLasImagenes();
        DesactivarTodosLosBordes();
    }

    void ActivarImagenes()
    {
        imagenesHabilitadas = true;
        ActualizarVisual(); // Actualizar visualmente ahora que las im�genes est�n habilitadas.
        botonActivar.gameObject.SetActive(false); // Desactivar el bot�n de activaci�n.
    }

    void DesactivarTodasLasImagenes()
    {
        foreach (var imagen in imagenesAsociadas)
        {
            imagen.gameObject.SetActive(false);
        }
    }

    void DesactivarTodosLosBordes()
    {
        foreach (var borde in bordes)
        {
            borde.SetActive(false);
        }
    }

    void MoverBotones(int direccion)
    {
        // Movimiento c�clico: reordenar botones.
        if (direccion > 0)
        {
            // Mover el primer bot�n al final.
            Button botonTemp = botones[0];
            Image imagenTemp = imagenesAsociadas[0];
            GameObject bordeTemp = bordes[0];
            for (int i = 0; i < botones.Length - 1; i++)
            {
                botones[i] = botones[i + 1];
                imagenesAsociadas[i] = imagenesAsociadas[i + 1];
                bordes[i] = bordes[i + 1];
            }
            botones[botones.Length - 1] = botonTemp;
            imagenesAsociadas[imagenesAsociadas.Length - 1] = imagenTemp;
            bordes[bordes.Length - 1] = bordeTemp;
        }
        else
        {
            // Mover el �ltimo bot�n al principio.
            Button botonTemp = botones[botones.Length - 1];
            Image imagenTemp = imagenesAsociadas[imagenesAsociadas.Length - 1];
            GameObject bordeTemp = bordes[bordes.Length - 1];
            for (int i = botones.Length - 1; i > 0; i--)
            {
                botones[i] = botones[i - 1];
                imagenesAsociadas[i] = imagenesAsociadas[i - 1];
                bordes[i] = bordes[i - 1];
            }
            botones[0] = botonTemp;
            imagenesAsociadas[0] = imagenTemp;
            bordes[0] = bordeTemp;
        }

        // Reorganizar las posiciones visuales de todos los botones simult�neamente.
        StartCoroutine(AnimarDesplazamiento());
    }

    IEnumerator AnimarDesplazamiento()
    {
        enAnimacion = true;

        // Reducir la opacidad de todos los botones al inicio de la animaci�n.
        ActualizarOpacidadDuranteAnimacion(0.5f);

        // Calculamos las nuevas posiciones para los botones.
        Vector3[] posicionesObjetivo = new Vector3[botones.Length];
        for (int i = 0; i < botones.Length; i++)
        {
            posicionesObjetivo[i] = new Vector3(0, desplazamiento * (i - botones.Length / 2), 0);
        }

        // Mover todos los botones de forma sincronizada.
        float tiempo = 0f;
        while (tiempo < tiempoAnimacion)
        {
            for (int i = 0; i < botones.Length; i++)
            {
                botones[i].transform.localPosition = Vector3.Lerp(botones[i].transform.localPosition, posicionesObjetivo[i], tiempo / tiempoAnimacion);
            }
            tiempo += Time.deltaTime;
            yield return null;
        }

        // Asegurarse de que todos los botones lleguen a sus posiciones finales.
        for (int i = 0; i < botones.Length; i++)
        {
            botones[i].transform.localPosition = posicionesObjetivo[i];
        }

        // Restaurar la opacidad despu�s de que la animaci�n termine.
        ActualizarOpacidadDuranteAnimacion(1f);

        // Actualizar la visual de los botones (cambiar colores, tama�os, etc).
        ActualizarVisual();

        enAnimacion = false; // Finaliza la animaci�n.
    }

    void OrganizarBotones()
    {
        for (int i = 0; i < botones.Length; i++)
        {
            posicionesOriginales[i] = new Vector3(0, desplazamiento * (i - botones.Length / 2), 0);
            botones[i].transform.localPosition = posicionesOriginales[i];
        }
    }

    void ActualizarVisual()
    {
        DesactivarTodosLosBordes();

        for (int i = 0; i < botones.Length; i++)
        {
            var textoTMP = botones[i].GetComponentInChildren<TextMeshProUGUI>();
            var imagenFondo = botones[i].GetComponent<Image>();

            // Desactivar/activar el componente Button seg�n si est� en el centro o no.
            Button buttonComponent = botones[i].GetComponent<Button>();
            if (i == botones.Length / 2)
            {
                // Bot�n seleccionado (centro).
                buttonComponent.interactable = true; // Activar el bot�n en el centro.
                textoTMP.color = Color.black;
                textoTMP.fontSize = 10;
                SetBotonOpacity(botones[i], 1f);
                botones[i].transform.localScale = Vector3.one * 1.7f;
                if (imagenFondo != null) imagenFondo.color = colorBrillante;

                // Mostrar la imagen y el borde asociados si est�n habilitados.
                if (imagenesHabilitadas)
                {
                    imagenesAsociadas[i].gameObject.SetActive(true);
                    bordes[i].SetActive(true);
                }
            }
            else
            {
                // Botones no seleccionados.
                buttonComponent.interactable = false; // Desactivar el bot�n cuando no est� en el centro.
                textoTMP.color = Color.white;
                textoTMP.fontSize = 7;
                SetBotonOpacity(botones[i], 0.5f);
                botones[i].transform.localScale = Vector3.one * 1.5f;
                if (imagenFondo != null) imagenFondo.color = colorOscuro;

                // Ocultar la imagen asociada.
                imagenesAsociadas[i].gameObject.SetActive(false);
            }
        }
    }

    void ActualizarOpacidadDuranteAnimacion(float opacity)
    {
        for (int i = 0; i < botones.Length; i++)
        {
            if (i == botones.Length / 2) SetBotonOpacity(botones[i], 0.5f);
            else SetBotonOpacity(botones[i], opacity);
        }
    }

    void SetBotonOpacity(Button boton, float opacity)
    {
        var imagenFondo = boton.GetComponent<Image>();
        if (imagenFondo != null)
        {
            var color = imagenFondo.color;
            color.a = opacity;
            imagenFondo.color = color;
        }

        var textoTMP = boton.GetComponentInChildren<TextMeshProUGUI>();
        if (textoTMP != null)
        {
            var colorTexto = textoTMP.color;
            colorTexto.a = opacity;
            textoTMP.color = colorTexto;
        }
    }
}
