using TMPro;
using UnityEngine;
using System.Collections;

public class scr_MensajeManager : MonoBehaviour
{
    public static scr_MensajeManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private GameObject panel_Mensaje; // El panel contenedor
    [SerializeField] private TextMeshProUGUI texto_Mensaje; // El texto dentro del panel
    [SerializeField] private float DuracionMensaje = 2f;

    private Coroutine Corrutina_MensajeActual;

    void Awake()
    {
        if (Instance == null)        
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject); //Nota: No lo pongo porque no es un objeto que necesite persistir
        }
        else
        {
            Destroy(gameObject);
        }

        // Ocultar el panel al inicio
        if (panel_Mensaje != null)
        {
            panel_Mensaje.SetActive(false);
        }
    }

    // Muestra un mensaje temporal en pantalla
    public void MostrarMensaje(string message, float duracion = -1f) // Este de activa y modifica dependiendo de lo que le llegue del script scr_ObjetoBloqueador 
    {
        if (panel_Mensaje == null || texto_Mensaje == null) return;

        // Usar duración por defecto si no se especifica
        if (duracion < 0)
        {
            duracion = DuracionMensaje;
        }

        // Cancelar mensaje anterior si existe
        if (Corrutina_MensajeActual != null)
        {
            StopCoroutine(Corrutina_MensajeActual);
        }

        // Actualizar texto y mostrar panel
        texto_Mensaje.text = message;
        panel_Mensaje.SetActive(true);

        Corrutina_MensajeActual = StartCoroutine(OcultarMensaje_TrasDelay(duracion));
    }




    private IEnumerator OcultarMensaje_TrasDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (panel_Mensaje != null)
        {
            panel_Mensaje.SetActive(false);
        }

        Corrutina_MensajeActual = null;
    }

    //METODOS ADICIONALES/EN DESUSO

    // Oculta el mensaje inmediatamente, alternativa a OcultarMensaje_TrasDelay(float delay), Podria servir segun el caso
    public void OcultarMensaje()
    {
        if (Corrutina_MensajeActual != null)
        {
            StopCoroutine(Corrutina_MensajeActual);
            Corrutina_MensajeActual = null;
        }

        if (panel_Mensaje != null)
        {
            panel_Mensaje.SetActive(false);
        }
    }

}