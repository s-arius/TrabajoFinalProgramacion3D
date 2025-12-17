using TMPro; 
using UnityEngine;

//Este es un ejemplo de una puerta que se abrirá en caso de que el jugador tenga una llave, la idea esque sirva de ejemplo para crear los otros objetos que requieren de otro para usarse.
// Ejemplo: Llave = abrir Puerta
[RequireComponent(typeof(Collider))]
public class scr_doorController : MonoBehaviour
{
    [Header("Requerimientos")]
    [SerializeField] private scr_ItemData ItemNecesario; // La llave necesaria
    [SerializeField] private bool consumirItem = false;  // Cuando se activehara que el objeto sea eliminado de la lista.

    [Header("Configuración")]
    [SerializeField] private KeyCode Tecla_deInteraccion = KeyCode.E;
    [SerializeField] private float distancia_interactuar = 3f;

    [Header("UI")]
    [SerializeField] private GameObject panelMensaje;
    //[SerializeField] private TextMeshProUGUI text_Mensaje; // Mensaje en pantalla
    [SerializeField] private float messageDuration = 2f;
    [SerializeField] private GameObject panelInteractuar;
    [SerializeField] private TextMeshProUGUI text_Interactuar;

    [Header("Animación (Opcional)")]
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private string openAnimationTrigger = "Open";

    private Transform playerTransform;
    internal bool enRango = false;
    internal bool isOpen = false;
    //private float mensajeTimer = 0f;

    void Start()
    {
        playerTransform = Camera.main.transform;
        GetComponent<Collider>().isTrigger = true;

        panelInteractuar.SetActive(false);
    }

    void Update()
    {
        if (!enRango || isOpen) return;

        // Comprobar distancia
        float distance = Vector3.Distance(transform.position, playerTransform.position);
        if (distance > distancia_interactuar)
        {
            enRango = false;
            return;
        }

        // Intentar abrir
        if (Input.GetKeyDown(Tecla_deInteraccion))
        {
            TryOpen();
        }
    }


 
    // Intenta abrir la puerta
    void TryOpen()
    {
        // Comprobar si el jugador tiene el objeto necesario
        if (InventoryManager.Instance.HasItem(ItemNecesario))
        {
            OpenDoor();

            // Consumir el objeto si está configurado
            if (consumirItem)
            {
                InventoryManager.Instance.RemoveItem(ItemNecesario);
            }
        }
        else
        {
            MostrarMensaje($"Necesitas: {ItemNecesario.nombre}");
        }
    }



    // Abre la puerta
    void OpenDoor()
    {
        isOpen = true;
        MostrarMensaje("Puerta abierta");

        // Reproducir animación si existe
        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger(openAnimationTrigger);
        }
        else
        {
            // Si no hay animación, simplemente desactivar
            gameObject.SetActive(false);
           
        }


        Debug.Log("¡Puerta abierta!");
    }

    // Muestra un mensaje temporal en pantalla
    void MostrarMensaje(string mensaje)
    {

        scr_MensajeManager.Instance.MostrarMensaje(mensaje, messageDuration);
        Debug.Log(mensaje);
        

        if (isOpen == true)
        {
            panelInteractuar.gameObject.SetActive(false);
        }
        Debug.Log(mensaje);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            panelInteractuar.SetActive(true);
            enRango = true;

            /*if (myscr_PuertaController.isOpen == true) //Intento de ocultar el panel cuando el objeto se destruya
            {
                panel_Interactuar.SetActive(false);
            }*/
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            panelInteractuar.SetActive(false);
            enRango = false;

        }
    }

}