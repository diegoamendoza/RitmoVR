using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class VerticalMusicMenu : MonoBehaviour
{
    public Button[] botones; // Array de botones, deben estar en el orden l�gico.
    public Button botonArriba; // Bot�n para mover hacia arriba.
    public Button botonAbajo; // Bot�n para mover hacia abajo.
    public Image[] imagenesAsociadas; // Array de im�genes asociadas a cada bot�n.
    public float desplazamiento = 100f; // Distancia entre los botones en el eje Y.
    private int indiceSeleccionado = 0; // El �ndice del bot�n "fijo" al centro.
    public Color colorOscuro = new Color(0.3f, 0.3f, 0.3f); // Color para botones no seleccionados.
    public Color colorBrillante = Color.white; // Color para el bot�n seleccionado.
    public float tiempoAnimacion = 0.5f; // Tiempo de animaci�n para el desplazamiento.
    private Vector3[] posicionesOriginales; // Para almacenar las posiciones originales de los botones.
    private bool enAnimacion = false; // Flag para saber si est� en animaci�n.

    void Start()
    {
        // Validar que cada bot�n tenga una imagen asociada.
        if (botones.Length != imagenesAsociadas.Length)
        {
            Debug.LogError("El n�mero de botones no coincide con el n�mero de im�genes asociadas.");
            return;
        }

        // Almacena las posiciones originales y organiza los botones.
        posicionesOriginales = new Vector3[botones.Length];
        OrganizarBotones();
        ActualizarVisual();

        // Conecta los botones interactivos con sus acciones.
        botonArriba.onClick.AddListener(() => {
            if (!enAnimacion) MoverBotones(-1); // Mover hacia arriba.
        });

        botonAbajo.onClick.AddListener(() => {
            if (!enAnimacion) MoverBotones(1); // Mover hacia abajo.
        });
    }

    void MoverBotones(int direccion)
    {
        // Movimiento c�clico: reordenar botones.
        if (direccion > 0)
        {
            // Mover el primer bot�n al final.
            Button botonTemp = botones[0];
            Image imagenTemp = imagenesAsociadas[0];
            for (int i = 0; i < botones.Length - 1; i++)
            {
                botones[i] = botones[i + 1];
                imagenesAsociadas[i] = imagenesAsociadas[i + 1];
            }
            botones[botones.Length - 1] = botonTemp;
            imagenesAsociadas[imagenesAsociadas.Length - 1] = imagenTemp;
        }
        else
        {
            // Mover el �ltimo bot�n al principio.
            Button botonTemp = botones[botones.Length - 1];
            Image imagenTemp = imagenesAsociadas[imagenesAsociadas.Length - 1];
            for (int i = botones.Length - 1; i > 0; i--)
            {
                botones[i] = botones[i - 1];
                imagenesAsociadas[i] = imagenesAsociadas[i - 1];
            }
            botones[0] = botonTemp;
            imagenesAsociadas[0] = imagenTemp;
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
        for (int i = 0; i < botones.Length; i++)
        {
            var textoTMP = botones[i].GetComponentInChildren<TextMeshProUGUI>();
            var imagenFondo = botones[i].GetComponent<Image>();

            if (i == botones.Length / 2)
            {
                // Bot�n seleccionado (centro).
                textoTMP.color = Color.white;
                textoTMP.fontSize = 28;
                SetBotonOpacity(botones[i], 1f);
                botones[i].transform.localScale = Vector3.one * 1.2f;
                if (imagenFondo != null) imagenFondo.color = colorBrillante;

                // Mostrar la imagen asociada.
                imagenesAsociadas[i].gameObject.SetActive(true);
            }
            else
            {
                // Botones no seleccionados.
                textoTMP.color = Color.black;
                textoTMP.fontSize = 24;
                SetBotonOpacity(botones[i], 0.5f);
                botones[i].transform.localScale = Vector3.one;
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
