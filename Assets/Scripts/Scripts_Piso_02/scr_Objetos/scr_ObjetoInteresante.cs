using TMPro;
using UnityEngine;
using System.Collections;

// Objeto que muestra un pensamiento automaticamente cuando el jugador permanece cerca durante cierto tiempo
public class scr_ObjetoInteresante : MonoBehaviour
{
    //CONFIGURACION
    [Header("UI Referencias")]
    [SerializeField] private GameObject panelPensamiento;
    [SerializeField] private TextMeshProUGUI textoPensamientoUI;

    [Header("Configuracion")]
    [TextArea(2, 3)]
    [SerializeField] private string textoPensamiento = "Este objeto parece interesante...";
    [SerializeField] private float duracionMensaje = 5f;
    [SerializeField] private float tiempoRequeridoDentro = 3f; //Tiempo dentro dle trigger requerido para que aparezca el mensaje
    [SerializeField] private float cooldownEntreActivaciones = 30f; //Evitar spawneo del mensaje

    [Header("Debug (Opcional)")]
    [Tooltip("Mostrar mensajes de debug en consola")]
    [SerializeField] private bool mostrarDebug = false; //Interesante planteamiento para evitar escribir y borrar constantemente los debugs. 


    // VARIABLES PRIVADAS
    private bool estaEnRango = false;
    private float tiempoDentroDelTrigger = 0f;
    private float ultimaActivacion = -999f; // Inicializado muy en el pasado, sirve para evitar que se bloquee el primer uso de ese trigger
    private bool pensamientoMostrado = false;
    private Coroutine ocultarCoroutine;


    void Start()
    {
        // Asegurar que el collider es trigger
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = true;
        }
        else
        {
            Debug.LogError($"No tiene Collider. Añade un Collider para que funcione.");
        }

        // Validar referencias UI
        if (panelPensamiento == null)
        {
            Debug.LogError($"Panel de pensamiento no asignado en el Inspector.");
        }

        if (textoPensamientoUI == null)
        {
            Debug.LogError($"Texto de pensamiento no asignado en el Inspector.");
        }

        // Asegurar que el panel está oculto al inicio
        if (panelPensamiento != null)
        {
            panelPensamiento.SetActive(false);
        }


    }

    void Update()
    {

        if (!estaEnRango) return;

        // Incrementar el tiempo dentro del trigger
        tiempoDentroDelTrigger += Time.deltaTime;

        // Debug visual
        if (mostrarDebug)
        {
            Debug.Log($"Tiempo dentro: {tiempoDentroDelTrigger}s / {tiempoRequeridoDentro}s");
        }

        // Verificar si se alcanzó el tiempo requerido
        if (tiempoDentroDelTrigger >= tiempoRequeridoDentro && !pensamientoMostrado)
        {
            IntentarMostrarPensamiento();
        }


    }


    // Intenta mostrar el pensamiento si el cooldown lo permite
    void IntentarMostrarPensamiento()
    {
        // Verificar cooldown
        float tiempoDesdeUltimaActivacion = Time.time - ultimaActivacion;

        if (tiempoDesdeUltimaActivacion < cooldownEntreActivaciones)
        {
            // Cooldown activo
            float tiempoRestante = cooldownEntreActivaciones - tiempoDesdeUltimaActivacion;

            if (mostrarDebug)
            {
                Debug.Log($"Cooldown activo. Faltan {tiempoRestante:F1}s"); // F1 significa la cantidad de decimales que mostrará el float, en este caso solo 1. Ej: 3.237 -> 3.2
            }

            pensamientoMostrado = true; // Evitar chequear múltiples veces
            return;
        }

        // Cooldown terminado, mostrar pensamiento
        MostrarPensamiento();
    }

    // Muestra el pensamiento en el panel
    void MostrarPensamiento()
    {
        // Verificar referencias
        if (panelPensamiento == null || textoPensamientoUI == null) //Debo de acostumbrarme ha hacer el codigo así... impecable; Al basho
        {
            Debug.LogError(" Referencias UI no configuradas.");
            return;
        }

        // Actualizar texto
        textoPensamientoUI.text = textoPensamiento;

        // Mostrar panel
        panelPensamiento.SetActive(true);


        // Iniciar corrutina para ocultar después del tiempo configurado
        ocultarCoroutine = StartCoroutine(OcultarDespuesDeTiempo());

        // Actualizar estado
        pensamientoMostrado = true;
        ultimaActivacion = Time.time;

    }

    // Corrutina que oculta el panel después del tiempo configurado
    IEnumerator OcultarDespuesDeTiempo()
    {
        yield return new WaitForSeconds(duracionMensaje);

        if (panelPensamiento != null)
        {
            panelPensamiento.SetActive(false);
        }

        if (mostrarDebug)
        {
            Debug.Log("Pensamiento ocultado automáticamente");
        }

        ocultarCoroutine = null; // es una limpieza, como la corrutina y hizo su trabajo podemos hacer una limpieza como buena praxis.
    }

    void OnTriggerEnter(Collider other)
    {
        // Verificar si es el jugador
        if (!other.CompareTag("Player")) return;

        estaEnRango = true;
        tiempoDentroDelTrigger = 0f; // Resetear contador

        if (mostrarDebug)
        {
            Debug.Log($" Jugador ENTRO en {gameObject.name}");
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Verificar si es el jugador
        if (!other.CompareTag("Player")) return;

        estaEnRango = false;
        tiempoDentroDelTrigger = 0f; // Resetear contador
        pensamientoMostrado = false; // Permitir mostrar de nuevo si vuelve a entrar

        if (mostrarDebug)
        {
            Debug.Log($"Jugador SALIO de {gameObject.name}");
        }
    }

}
