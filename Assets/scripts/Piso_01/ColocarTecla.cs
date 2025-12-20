using UnityEngine;

public class ColocarTecla : MonoBehaviour
{
    [Header("Cartel")]
    public GameObject cartelColocarTecla;

    [Header("Control luces")]
    public ControladorLuces controladorLuces;

    [Header("Cámara")]
    public CamaraSeguir camara;

    [Header("Keypad UI")]
    public GameObject canvasKeypadUI; // SOLO la UI del keypad
    public KeypadUI keypadUI;         // Script del keypad

    [Header("Objeto que aparece tras colocar la tecla")]
    public ObjetoVisibleTrasTecla objetoVisibleTrasTecla;

    [HideInInspector]
    public bool teclaColocada = false; // Para otros scripts

    private Player jugador;
    private bool jugadorCerca = false;

    void Start()
    {
        if (cartelColocarTecla != null)
            cartelColocarTecla.SetActive(false);

        if (canvasKeypadUI != null)
            canvasKeypadUI.SetActive(false);

        // Recuperamos estado global
        teclaColocada = GameManagerGlobal.Instance.teclaColocada;

        // Si la tecla ya fue colocada, activamos el objeto automáticamente
        if (teclaColocada && objetoVisibleTrasTecla != null)
            objetoVisibleTrasTecla.Activar();
    }

    void Update()
    {
        if (!teclaColocada &&
            jugadorCerca &&
            jugador != null &&
            jugador.tengoTecla &&
            Input.GetKeyDown(KeyCode.E))
        {
            ColocarTeclaEnSlot();
        }
    }

    void ColocarTeclaEnSlot()
    {
        teclaColocada = true;
        GameManagerGlobal.Instance.teclaColocada = true; // ⚠ independiente de luces

        // Apagar luces
        GameManagerGlobal.Instance.lucesApagadas = true;
        controladorLuces.ApagarLuces();

        // Ocultar cartel y UI
        if (cartelColocarTecla != null)
            cartelColocarTecla.SetActive(false);

        if (canvasKeypadUI != null)
            canvasKeypadUI.SetActive(false);

        // Actualizamos el estado en el keypad UI
        if (keypadUI != null)
            keypadUI.teclaColocada = true;

        // Activamos el objeto que debe aparecer tras colocar la tecla
        if (objetoVisibleTrasTecla != null)
            objetoVisibleTrasTecla.Activar();

        Debug.Log("Tecla colocada. Luces apagadas y objeto visible activado.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        jugador = other.GetComponent<Player>();
        jugadorCerca = true;

        if (!teclaColocada &&
            jugador != null &&
            jugador.tengoTecla &&
            cartelColocarTecla != null)
        {
            cartelColocarTecla.SetActive(true);
        }

        if (camara != null)
            camara.ActivarColocacion(transform);

        if (canvasKeypadUI != null && !GameManagerGlobal.Instance.lucesApagadas)
            canvasKeypadUI.SetActive(true);

        if (keypadUI != null)
        {
            keypadUI.jugador = jugador;
            keypadUI.controladorLuces = controladorLuces;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        jugadorCerca = false;
        jugador = null;

        if (cartelColocarTecla != null)
            cartelColocarTecla.SetActive(false);

        if (camara != null)
            camara.DesactivarColocacion();

        if (canvasKeypadUI != null)
            canvasKeypadUI.SetActive(false);
    }
}
