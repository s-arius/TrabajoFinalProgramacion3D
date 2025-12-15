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
    [SerializeField] private TextMeshProUGUI text_Mensaje; // Mensaje en pantalla
    [SerializeField] private float messageDuration = 2f;

    [Header("Animación (Opcional)")]
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private string openAnimationTrigger = "Open";

    private Transform playerTransform;
    internal bool enRango = false;
    internal bool isOpen = false;
    private float mensajeTimer = 0f;

    void Start()
    {
        playerTransform = Camera.main.transform;
        GetComponent<Collider>().isTrigger = true;

        if (text_Mensaje != null)
        {
            panelMensaje.gameObject.SetActive(false);
            text_Mensaje.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // Ocultar mensaje después del tiempo configurado
        if (mensajeTimer > 0)
        {
            mensajeTimer -= Time.deltaTime;
            if (mensajeTimer <= 0 && text_Mensaje != null)
            {
                panelMensaje.gameObject.SetActive(false);
                text_Mensaje.gameObject.SetActive(false);

            }
        }

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
            ShowMessage($"Necesitas: {ItemNecesario.itemName}");
        }
    }



    // Abre la puerta
    void OpenDoor()
    {
        isOpen = true;
        ShowMessage("Puerta abierta");

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
    void ShowMessage(string message)
    {
        if (text_Mensaje != null)
        {
            panelMensaje.gameObject.SetActive(true);
            text_Mensaje.text = message;
            text_Mensaje.gameObject.SetActive(true);
            mensajeTimer = messageDuration;
        }

        Debug.Log(message);
    }

    // Visualizar el rango de interacción
  /*  void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transfrm.position, distancia_interactuar);
    }
  */
}