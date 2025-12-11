using UnityEngine;
using System.Collections; //IEnumerator

public class scr_FadeMensaje : MonoBehaviour
{

    [Header("Configuración Fade")]
    [Tooltip("Tiempo en segundos del fade")]
    public float duracionFade = 2.0f;
    private Renderer miRenderer;
    private Color colorOriginal;
    private bool colorGuardado = false;

    void Start()
    {
        miRenderer = GetComponent<Renderer>();

        // Guardamos el color original SOLO una vez
        colorOriginal = miRenderer.material.color;
        colorGuardado = true;

        // 🔹 Hacer el objeto invisible al inicio
        Color c = colorOriginal;
        c.a = 0f;
        miRenderer.material.color = c;

        Debug.Log("Objeto iniciado invisible con alpha = 0");
    }

    public void fade_Activar(bool esFadeIn)
    {

        StartCoroutine(IniciarFade(esFadeIn));
    }

    IEnumerator IniciarFade(bool esFadeIn)
    {
        float tiempoTranscurrido = 0f;

        Color colorInicio = esFadeIn
            ? new Color(colorOriginal.r, colorOriginal.g, colorOriginal.b, 0f) //? = operario ternario; Color inicio es fadeIn(true) -> dame el valor A
            : colorOriginal;                                                   //: si Color inicio es fadeIn(false) dame el valor B

        Color colorFinal = esFadeIn
            ? colorOriginal
            : new Color(colorOriginal.r, colorOriginal.g, colorOriginal.b, 0f);

        // Establecer color inicial
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