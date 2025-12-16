using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class KeypadUI : MonoBehaviour
{
    [System.Serializable]
    public class BotonInfo
    {
        public Button boton;
        public string valor;
    }

    [Header("Botones del keypad")]
    public List<BotonInfo> botones;
    public BotonInfo botonExtra; // Aparece solo si la tecla se ha colocado

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
        foreach (var bi in botones)
        {
            if (bi.boton != null)
            {
                Button temp = bi.boton;
                string valorTemp = bi.valor;
                temp.onClick.AddListener(() => PulsarBoton(valorTemp));
            }
        }

        if (botonExtra != null && botonExtra.boton != null)
        {
            botonExtra.boton.gameObject.SetActive(false);
            string valorTemp = botonExtra.valor;
            botonExtra.boton.onClick.AddListener(() => PulsarBoton(valorTemp));
        }

        if (mensajeCorrecto != null)
            mensajeCorrecto.SetActive(false);
        if (mensajeIncorrecto != null)
            mensajeIncorrecto.SetActive(false);
    }

    void Update()
    {
        // Mostrar botón extra solo si la tecla ya se colocó
        if (botonExtra != null && botonExtra.boton != null)
            botonExtra.boton.gameObject.SetActive(teclaColocada);
    }

    void PulsarBoton(string valor)
    {
        Debug.Log($"Botón pulsado: {valor}");

        if (controladorLuces != null && GameManagerGlobal.Instance.lucesApagadas)
        {
            Debug.Log($"La luz está apagada. El botón {valor} no funciona.");
            return;
        }

        Debug.Log($"Botón {valor} aceptado, añadiendo al código.");
        codigoActual += valor;

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
