using UnityEngine;
using System.Collections;

public class KeypadUI : MonoBehaviour
{
    [Header("Código correcto")]
    public string codigoCorrecto = "1234";

    [Header("Jugador")]
    public Player jugador;

    [Header("Control luces")]
    public ControladorLuces controladorLuces;

    [Header("UI de resultado")]
    public GameObject mensajeCorrecto;
    public GameObject mensajeIncorrecto;

    [Header("Animaciones")]
    public Animator animacionObjeto; // Asignar en inspector
    public string animacionInicial = "PuertaFinal";
    public string animacionFinal = "PuertaAbierta";

    [Header("Estado de la llave")]
    public bool teclaColocada = false; // true si ya se colocó la tecla

    private string codigoActual = "";
    private bool animacionReproducida = false; // Para que solo ocurra 1 vez

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
        // No permitir introducir código si la luz está apagada
        if (controladorLuces != null && GameManagerGlobal.Instance.lucesApagadas)
            return;

        // Teclas del 1 al 9
        for (int i = 1; i <= 9; i++)
        {
            if (Input.GetKeyDown(i.ToString()) || Input.GetKeyDown((KeyCode)((int)KeyCode.Keypad1 + i - 1)))
            {
                PulsarNumero(i.ToString());
            }
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
                if (mensajeCorrecto != null) mensajeCorrecto.SetActive(true);

                // Reproducir animación una sola vez
                if (!animacionReproducida && animacionObjeto != null)
                {
                    animacionReproducida = true;
                    StartCoroutine(ReproducirAnimaciones());
                }
            }
            else
            {
                Debug.Log("Código incorrecto.");
                if (mensajeIncorrecto != null) mensajeIncorrecto.SetActive(true);
            }

            codigoActual = "";
        }
    }

    private IEnumerator ReproducirAnimaciones()
    {
        // Reproducir animación inicial
        animacionObjeto.Play(animacionInicial);

        // Esperar a que termine la animación inicial
        AnimatorStateInfo stateInfo;
        do
        {
            yield return null;
            stateInfo = animacionObjeto.GetCurrentAnimatorStateInfo(0);
        } while (stateInfo.IsName(animacionInicial) && stateInfo.normalizedTime < 1f);

        // Reproducir animación final
        animacionObjeto.Play(animacionFinal);
        Debug.Log("Animación final reproducida: puerta abierta permanentemente.");
    }
}
