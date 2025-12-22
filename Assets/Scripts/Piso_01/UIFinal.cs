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

    [Header("Keypad")]
    public KeypadUI keypad;

    private bool jugadorDentro = false;
    private bool yaActivado = false;

    void Start()
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

    void Update()
    {
        if (yaActivado)
            return;

        if (jugadorDentro && keypad != null && keypad.codigoCorrectoIntroducido)
        {
            yaActivado = true;
            StartCoroutine(ActivarUIConFade());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorDentro = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorDentro = false;
        }
    }

    IEnumerator ActivarUIConFade()
    {
        yield return new WaitForSeconds(delayAntesFade);

        if (ui != null)
            ui.SetActive(true);

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
}
