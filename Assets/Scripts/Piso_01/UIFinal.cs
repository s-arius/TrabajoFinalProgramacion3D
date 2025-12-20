using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIFinal : MonoBehaviour
{
    [Header("UI a activar")]
    public GameObject ui;

    [Header("Imagen blanca para el fundido")]
    public Image imagenFade;

    [Header("Tiempos")]
    public float delayAntesFade = 2f;
    public float duracionFade = 1f;

    [Header("Opciones")]
    public bool ocultarAlSalir = false;

    private bool yaActivado = false;

    private void Start()
    {
        if (ui != null)
            ui.SetActive(false);

        if (imagenFade != null)
        {
            Color c = imagenFade.color;
            c.a = 0f;
            imagenFade.color = c;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (yaActivado)
            return;

        yaActivado = true;
        StartCoroutine(ActivarUIConFade());
    }

    IEnumerator ActivarUIConFade()
    {
        // Espera antes del fundido
        yield return new WaitForSeconds(delayAntesFade);

        // Activar la UI
        if (ui != null)
            ui.SetActive(true);

        // Fundido a blanco
        float tiempo = 0f;
        Color c = imagenFade.color;

        while (tiempo < duracionFade)
        {
            tiempo += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, tiempo / duracionFade);
            imagenFade.color = c;
            yield return null;
        }

        c.a = 1f;
        imagenFade.color = c;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && ocultarAlSalir)
        {
            if (ui != null)
                ui.SetActive(false);
        }
    }
}
