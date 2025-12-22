using UnityEngine;
using System.Collections;

public class KeypadUI : MonoBehaviour
{
    [Header("Código correcto")]
    public string codigoCorrecto = "4209";

    [Header("Jugador")]
    public Player jugador;

    [Header("Control luces")]
    public ControladorLuces controladorLuces;

    [Header("UI de resultado")]
    public GameObject mensajeCorrecto;
    public GameObject mensajeIncorrecto;

    [Header("Animaciones")]
    public Animator animacionObjeto;
    public string animacionInicial = "PuertaFinal";
    public string animacionFinal = "PuertaAbierta";

    [Header("Sonidos")]
    public AudioSource audioSource;
    public AudioClip sonidoCorrecto;
    public AudioClip sonidoIncorrecto;

    [HideInInspector] public bool codigoCorrectoIntroducido = false;

    private string codigoActual = "";
    private bool animacionReproducida = false;

    void Start()
    {
        if (mensajeCorrecto != null) mensajeCorrecto.SetActive(false);
        if (mensajeIncorrecto != null) mensajeIncorrecto.SetActive(false);
    }

    void Update()
    {
        LeerTecladoNumerico();
    }

    void LeerTecladoNumerico()
    {
        if (!GameManagerGlobal.Instance.teclaColocada)
            return;

        if (GameManagerGlobal.Instance.lucesApagadas)
            return;

        if (codigoCorrectoIntroducido)
            return;

        for (int i = 1; i <= 9; i++)
        {
            if (Input.GetKeyDown(i.ToString()) ||
                Input.GetKeyDown((KeyCode)((int)KeyCode.Keypad1 + i - 1)))
            {
                PulsarNumero(i.ToString());
            }
        }

        if (Input.GetKeyDown("0") || Input.GetKeyDown(KeyCode.Keypad0))
        {
            PulsarNumero("0");
        }
    }

    void PulsarNumero(string valor)
    {
        Debug.Log($"Número pulsado: {valor}");
        codigoActual += valor;

        if (codigoActual.Length >= codigoCorrecto.Length)
        {
            if (codigoActual == codigoCorrecto)
            {
                Debug.Log("Código correcto!");

                codigoCorrectoIntroducido = true;

                if (mensajeCorrecto != null)
                    mensajeCorrecto.SetActive(true);

                if (audioSource != null && sonidoCorrecto != null)
                    audioSource.PlayOneShot(sonidoCorrecto);

                if (!animacionReproducida && animacionObjeto != null)
                {
                    animacionReproducida = true;
                    StartCoroutine(ReproducirAnimaciones());
                }
            }
            else
            {
                Debug.Log("Código incorrecto.");

                if (mensajeIncorrecto != null)
                    mensajeIncorrecto.SetActive(true);

                if (audioSource != null && sonidoIncorrecto != null)
                    audioSource.PlayOneShot(sonidoIncorrecto);
            }

            codigoActual = "";
        }
    }

    private IEnumerator ReproducirAnimaciones()
    {
        animacionObjeto.Play(animacionInicial);

        AnimatorStateInfo stateInfo;
        do
        {
            yield return null;
            stateInfo = animacionObjeto.GetCurrentAnimatorStateInfo(0);
        }
        while (stateInfo.IsName(animacionInicial) && stateInfo.normalizedTime < 1f);

        animacionObjeto.Play(animacionFinal);
    }
}
