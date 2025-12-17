using TMPro; 
using UnityEngine;

//Este es un ejemplo de una puerta que se abrirá en caso de que el jugador tenga una llave, la idea esque sirva de ejemplo para crear los otros objetos que requieren de otro para usarse.
// Ejemplo: Llave = abrir Puerta
[RequireComponent(typeof(Collider))]
public class scr_ObjetoBloqueador : MonoBehaviour
{

    // ENUM: Tipos de interacción disponibles
    public enum TipoInteraccion
    {
        Puerta,
        Cuerda
    }

    [Header("Tipo de Objeto")]
    [SerializeField] private TipoInteraccion tipoInteraccion = TipoInteraccion.Puerta;

    [Header("Requerimientos")]
    [SerializeField] private scr_ItemData ItemNecesario; // La llave necesaria
    [SerializeField] private bool consumirItem = false;  // Cuando se activehara que el objeto sea eliminado de la lista.

    [Header("Mensajes Personalizados")]
    [SerializeField][TextArea(2, 3)] private string messageWithoutItem = "Ejemplo: Necesitas un objeto para interactuar";
    [SerializeField][TextArea(2, 3)] private string messageWithItem = "Ejemplo: Objeto desbloqueado";


    [Header("Configuración")]
    [SerializeField] private KeyCode Tecla_deInteraccion = KeyCode.E;
    //[SerializeField] private float distancia_interactuar = 3f;

    [Header("UI")]
    [SerializeField] private GameObject panelMensaje;
    //[SerializeField] private TextMeshProUGUI text_Mensaje; // Mensaje en pantalla
    [SerializeField] private float messageDuration = 2f;
    [SerializeField] private GameObject panelInteractuar;
    [SerializeField] private TextMeshProUGUI text_Interactuar;

    [Header("Animación (Opcional)")]
    [SerializeField] private Animator objectAnimator;
    [SerializeField] private string activationTrigger = "Activate";

    //Variables 
    //private Transform playerTransform;
    private bool enRango = false;
    private bool estaActivada = false;
    //private float mensajeTimer = 0f;

    void Start()
    {
        //playerTransform = Camera.main.transform;
        GetComponent<Collider>().isTrigger = true;
        panelInteractuar.SetActive(false);
    }

    void Update()
    {
        if (!enRango || estaActivada) return;

        // Comprobar distancia
        //float distance = Vector3.Distance(transform.position, playerTransform.position);
        /*if (distance > distancia_interactuar)
        {
            enRango = false;
            return;
        } */

        // Intentar abrir
        if (Input.GetKeyDown(Tecla_deInteraccion))
        {
            intentarActivacion();
        }
    }


 
    // Intenta Activar el objeto
    void intentarActivacion()
    {
        // Comprobar si el jugador tiene el objeto necesario
        if (InventoryManager.Instance.HasItem(ItemNecesario))
        {
            ActivarObjeto();

            // Consumir el objeto si está configurado
            if (consumirItem)
            {
                InventoryManager.Instance.RemoveItem(ItemNecesario);
            }
        }
        else
        {
            MostrarMensaje(messageWithoutItem);
        }
    }



    // Activa el objeto
    void ActivarObjeto()
    {
        estaActivada = true;
        MostrarMensaje(messageWithItem);

        // Realiza el comportamiento según el tipo (Gracias Enums por existir)
        switch (tipoInteraccion)
        {
            case TipoInteraccion.Puerta:
                ActivatePuerta();
                break;

            case TipoInteraccion.Cuerda:
                ActivateCuerda();
                break;
        }

        Debug.Log($"¡{tipoInteraccion} activado!");
    }

    // Muestra un mensaje temporal en pantalla
    void MostrarMensaje(string mensaje)
    {

        scr_MensajeManager.Instance.MostrarMensaje(mensaje, messageDuration);
        Debug.Log(mensaje);
        

        if (estaActivada == true)
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

   
    // COMPORTAMIENTO DEPENDIENDO EL TIPO DEL MISMO; Ejemplo: Si es puerta, haz esto.

    // Comportamiento para puertas
    void ActivatePuerta()
    {
        // Reproducir animación si existe
        if (objectAnimator != null)
        {
            objectAnimator.SetTrigger(activationTrigger);
        }
        else
        {
            // Si no hay animación, simplemente desactivar
            gameObject.SetActive(false);
        }
    }

    //Comportamiento para cuerdas
    void ActivateCuerda()
    {
        // Reproducir animación si existe
        if (objectAnimator != null)
        {
            objectAnimator.SetTrigger(activationTrigger);
        }
        else
        {
            // Si no hay animación, simplemente desactivar
            gameObject.SetActive(false);
        }

        // AQUÍ puedes añadir lógica adicional:
        // - Hacer caer objetos con Rigidbody
        // - Activar partículas
        // - Reproducir sonido de corte
        // Ejemplo:
        // GameObject objetoColgado = GameObject.Find("ObjetoColgado");
        // objetoColgado.GetComponent<Rigidbody>().useGravity = true;
    }


}