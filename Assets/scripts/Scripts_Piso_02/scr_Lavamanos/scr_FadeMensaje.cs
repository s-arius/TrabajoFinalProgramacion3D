using UnityEngine;
using System.Collections;

public class scr_FadeMensaje : MonoBehaviour
{
    [Header("Configuración Fade")]
    [Tooltip("Tiempo en segundos del fade")]
    public float duracionFade = 2.0f;

    private Renderer miRenderer;
    private Color colorOriginal;
    private bool mensajeDeberiaEstarVisible = false;
    private bool estadoPrevioLuces = false;

    void Start()
    {
        miRenderer = GetComponent<Renderer>();
        colorOriginal = miRenderer.material.color;

        // Iniciar invisible
        Color c = colorOriginal;
        c.a = 0f;
        miRenderer.material.color = c;

        // Guardar estado inicial de las luces
        if (GameManagerGlobal.Instance != null)
        {
            estadoPrevioLuces = GameManagerGlobal.Instance.lucesApagadas;
        }

        Debug.Log("Objeto iniciado invisible con alpha = 0");
    }

    void Update()
    {
        if (GameManagerGlobal.Instance == null) return;

        bool lucesActualmenteApagadas = GameManagerGlobal.Instance.lucesApagadas;

        // Detectar cambio en el estado de las luces
        if (lucesActualmenteApagadas != estadoPrevioLuces)
        {
            OnCambioEstadoLuces(lucesActualmenteApagadas);
            estadoPrevioLuces = lucesActualmenteApagadas;
        }
    }

    private void OnCambioEstadoLuces(bool lucesApagadas)
    {
        if (lucesApagadas)
        {
            if (mensajeDeberiaEstarVisible)
            {
                Debug.Log("Luces apagadas - Ocultando mensaje automáticamente");
                StartCoroutine(IniciarFade(false));
            }
        }
        else
        {
            if (mensajeDeberiaEstarVisible)
            {
                Debug.Log("Luces encendidas - Mostrando mensaje automáticamente");
                StartCoroutine(IniciarFade(true));
            }
        }
    }

    public void fade_Activar(bool esFadeIn)
    {
        // Validar: solo permitir fade in si las luces están encendidas
        if (esFadeIn && GameManagerGlobal.Instance != null && GameManagerGlobal.Instance.lucesApagadas)
        {
            Debug.Log("No se puede mostrar el mensaje con las luces apagadas");
            return;
        }

        // Evitar fade out si ya está invisible (alpha = 0)
        if (!esFadeIn && miRenderer.material.color.a == 0f)
        {
            mensajeDeberiaEstarVisible = false;
            Debug.Log("El mensaje ya está invisible, no se ejecuta fade out");
            return;
        }

        mensajeDeberiaEstarVisible = esFadeIn;
        StartCoroutine(IniciarFade(esFadeIn));
    }

    IEnumerator IniciarFade(bool esFadeIn)
    {
        float tiempoTranscurrido = 0f;

        Color colorInicio = esFadeIn
            ? new Color(colorOriginal.r, colorOriginal.g, colorOriginal.b, 0f)
            : colorOriginal;

        Color colorFinal = esFadeIn
            ? colorOriginal
            : new Color(colorOriginal.r, colorOriginal.g, colorOriginal.b, 0f);

        miRenderer.material.color = colorInicio;

        while (tiempoTranscurrido < duracionFade)
        {
            tiempoTranscurrido += Time.deltaTime;
            float porcentaje = tiempoTranscurrido / duracionFade;
            miRenderer.material.color = Color.Lerp(colorInicio, colorFinal, porcentaje);
            yield return null;
        }

        miRenderer.material.color = colorFinal;
        Debug.Log($"Fade {(esFadeIn ? "In" : "Out")} completado");
    }
}