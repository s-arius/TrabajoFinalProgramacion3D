using UnityEngine;

public class Keypad : MonoBehaviour
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

    [Header("Estado de la llave")]
    public bool teclaColocada = false; // true si ya se colocó la tecla

    private string codigoActual = "";

    void Start()
    {
        if (mensajeCorrecto != null)
            mensajeCorrecto.SetActive(false);

        if (mensajeIncorrecto != null)
            mensajeIncorrecto.SetActive(false);
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

        // Teclas del 1 al 9 (fila superior y keypad)
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
        Debug.Log($"Código actual: {codigoActual}");

        if (codigoActual.Length >= codigoCorrecto.Length)
        {
            if (codigoActual == codigoCorrecto)
            {
                Debug.Log("Código correcto!");

                if (mensajeCorrecto != null)
                    mensajeCorrecto.SetActive(true);
            }
            else
            {
                Debug.Log("Código incorrecto.");

                if (mensajeIncorrecto != null)
                    mensajeIncorrecto.SetActive(true);
            }

            codigoActual = "";
        }
    }
}
