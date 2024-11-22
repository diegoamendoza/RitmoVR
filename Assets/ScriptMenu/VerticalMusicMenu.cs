using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class VerticalMusicMenu : MonoBehaviour
{
    public Button[] botones; // Array de botones, deben estar en el orden l�gico.
    public float desplazamiento = 100f; // Distancia entre los botones en el eje Y.
    private int indiceSeleccionado = 0; // El �ndice del bot�n "fijo" al centro.
    public Color colorOscuro = new Color(0.3f, 0.3f, 0.3f); // Color para botones no seleccionados.
    public Color colorBrillante = Color.white; // Color para el bot�n seleccionado.
    public float tiempoAnimacion = 0.5f; // Tiempo de animaci�n para el desplazamiento.
    private Vector3[] posicionesOriginales; // Para almacenar las posiciones originales de los botones.
    private bool enAnimacion = false; // Flag para saber si est� en animaci�n.

    void Start()
    {
        posicionesOriginales = new Vector3[botones.Length]; // Inicializamos el array de posiciones.
        OrganizarBotones();
        ActualizarVisual();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) && !enAnimacion)
        {
            MoverBotones(1); // Desplazar botones hacia abajo.
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) && !enAnimacion)
        {
            MoverBotones(-1); // Desplazar botones hacia arriba.
        }
    }

    void MoverBotones(int direccion)
    {
        // Movimiento c�clico: reordenar botones.
        if (direccion > 0)
        {
            // Mover el primer bot�n al final.
            Button botonTemp = botones[0];
            for (int i = 0; i < botones.Length - 1; i++)
            {
                botones[i] = botones[i + 1];
            }
            botones[botones.Length - 1] = botonTemp;
        }
        else
        {
            // Mover el �ltimo bot�n al principio.
            Button botonTemp = botones[botones.Length - 1];
            for (int i = botones.Length - 1; i > 0; i--)
            {
                botones[i] = botones[i - 1];
            }
            botones[0] = botonTemp;
        }

        // Reorganizar las posiciones visuales de todos los botones simult�neamente.
        StartCoroutine(AnimarDesplazamiento());
    }

    // M�todo para animar el desplazamiento de los botones
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

    // Organiza los botones en su posici�n original
    void OrganizarBotones()
    {
        // Guardamos las posiciones originales para animarlas.
        for (int i = 0; i < botones.Length; i++)
        {
            posicionesOriginales[i] = new Vector3(0, desplazamiento * (i - botones.Length / 2), 0);
        }

        // Colocar los botones en sus posiciones correspondientes (arriba, centro, abajo).
        for (int i = 0; i < botones.Length; i++)
        {
            botones[i].transform.localPosition = posicionesOriginales[i];
        }
    }

    // Actualiza la visual de los botones
    void ActualizarVisual()
    {
        for (int i = 0; i < botones.Length; i++)
        {
            var textoTMP = botones[i].GetComponentInChildren<TextMeshProUGUI>();
            var imagenFondo = botones[i].GetComponent<Image>();

            if (i == botones.Length / 2)
            {
                // Bot�n seleccionado (centro).
                textoTMP.color = Color.yellow;
                textoTMP.fontSize = 28;
                botones[i].transform.localScale = Vector3.one * 1.2f;
                if (imagenFondo != null) imagenFondo.color = colorBrillante;

                // Mantener la opacidad en 1 mientras est� seleccionado.
                SetBotonOpacity(botones[i], 1f);
            }
            else
            {
                // Botones no seleccionados.
                textoTMP.color = Color.white;
                textoTMP.fontSize = 24;
                botones[i].transform.localScale = Vector3.one;
                if (imagenFondo != null) imagenFondo.color = colorOscuro;

                // Reducir la opacidad de los botones no seleccionados.
                SetBotonOpacity(botones[i], 0.5f);
            }
        }
    }

    // M�todo para ajustar la opacidad de los botones
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

    // M�todo para cambiar la opacidad de todos los botones durante la animaci�n
    void ActualizarOpacidadDuranteAnimacion(float opacity)
    {
        // Cambiar la opacidad de todos los botones durante la animaci�n (excepto el bot�n central que tendr� opacidad 0.5f temporalmente).
        for (int i = 0; i < botones.Length; i++)
        {
            if (i == botones.Length / 2) // El bot�n central
            {
                SetBotonOpacity(botones[i], 0.5f); // Durante la animaci�n, el bot�n seleccionado se opacifica
            }
            else
            {
                SetBotonOpacity(botones[i], opacity); // Los otros botones tienen la opacidad reducida
            }
        }
    }
}