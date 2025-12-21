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
        Cuerda,
        Maletin
    }

    public bool cuadroActivado = false;

    [Header("Tipo de Objeto")]
    [SerializeField] private TipoInteraccion tipoInteraccion = TipoInteraccion.Puerta;

    [Header("Requerimientos")]
    [SerializeField] private scr_ItemData ItemNecesario; // La "llave" necesaria para desbloquear el objeto

    [Header("Mensajes Personalizados")]
    [Tooltip("Escribe los mensajes que se mostraran en la UI al interactuar")]
    [SerializeField][TextArea(2, 3)] private string mensajeSinItem = "Ejemplo: Necesitas un objeto para interactuar";
    [SerializeField][TextArea(2, 3)] private string mensajeConItem = "Ejemplo: Objeto desbloqueado";


    [Header("Configuración")]
    [SerializeField] private KeyCode Tecla_deInteraccion = KeyCode.E;

    [Header("UI")]
    [SerializeField] private GameObject panelMensaje;
    [SerializeField] private float mensaje_Duracion = 2f;
    [SerializeField] private GameObject panelInteractuar;
    [SerializeField] private TextMeshProUGUI text_Interactuar;

    [Header("Animación (Opcional)")] //Actualmente esta puesto como ejemplo. Si no se quiere usar, pues no se usa.
    [Tooltip ("Solo si quieres usar animaciones")]
    [SerializeField] private Animator miAnimator;
    [SerializeField] private string activaTrigger = "Activate";

    //Variables 
    //private Transform playerTransform;
    private bool enRango = false;
    private bool estaActivada = false;
    
    //private float mensajeTimer = 0f;


    // referencia codigos
   // private Maletin codigoMaletin;
    public bool maletinActivado = false;




    void Start()
    {
        //playerTransform = Camera.main.transform;
        GetComponent<Collider>().isTrigger = true;
        panelInteractuar.SetActive(false);
    }

    void Update()
    {
        if (!enRango || estaActivada) return;


        // Intentar abrir
        if (Input.GetKeyDown(Tecla_deInteraccion))
        {
            intentarEjecutar();
        }
    }


 
    // Intenta Activar el objeto
    void intentarEjecutar()
    {
        // Comprobar si el jugador tiene el objeto necesario
        if (InventoryManager.Instancia.TieneElItem(ItemNecesario))
        {
            InventoryManager.Instancia.EliminarItem(ItemNecesario);
            Debug.Log("ItemConsumido, Bro");

            EjecutarObjeto();
        }
        else
        {
            MostrarMensaje(mensajeSinItem);
        }
    }



    // Activa el objeto
    void EjecutarObjeto()
    {
        estaActivada = true;
        MostrarMensaje(mensajeConItem);

        // Realiza el comportamiento según el tipo (Gracias Enums por existir)
        switch (tipoInteraccion)
        {
            case TipoInteraccion.Puerta:
                PuertaEjecucion();
                break;

            case TipoInteraccion.Cuerda:
                CuerdaEjecucion();
                break;

        
            case TipoInteraccion.Maletin:
                MaletinEjecucion();
                break;
        
        }


        Debug.Log($"¡{tipoInteraccion} activado!");
    }

    // Muestra un mensaje temporal en pantalla
    void MostrarMensaje(string mensaje)
    {

        scr_MensajeManager.Instance.MostrarMensaje(mensaje, mensaje_Duracion);
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
    void PuertaEjecucion()
    {
        // Reproducir animación si existe
        if (miAnimator != null)
        {
            miAnimator.SetTrigger(activaTrigger);
        }
        else
        {
            // Si no hay animación, simplemente desactivar
            gameObject.SetActive(false);
        }
    }

    //Comportamiento para cuerdas
    void CuerdaEjecucion()
    {
        Debug.Log("CUADRO ACTIVADO");
        cuadroActivado = true;
        // Reproducir animación si existe
        /*if (miAnimator != null)
        {
            miAnimator.SetTrigger(activaTrigger);
        }*/
        // Si no hay animación, simplemente desactivar
        //gameObject.SetActive(false);
            //gameObject.GetComponent<CuadroRoto>().breakPainting();

        // AQUÍ se debe añadir la lógica. Actualmente es un ejemplo en el que objeto desaparece,
        // sin embargo, eso dependera del tipo de objeto que tengamos y como queremos que funcione
        // Podriamos hacer caer objetos anadiendo un Rigidbody
        // o crear animaciones

        // Ejemplo:
        // GameObject objetoColgado = GameObject.Find("ObjetoColgado");
        // objetoColgado.GetComponent<Rigidbody>().useGravity = true;
        // Debug.Log("El objeto ha caido")

        //En pocas palabras, las funciones de comportamiento son lo que diferencian una puerta de un cuadro que cae.
    }

    
   void MaletinEjecucion()
{
   
        Debug.Log("Maletín abierto a través de scr_ObjetoBloqueador");
        
        maletinActivado = true;
        // panelInteractuar.SetActive(false);


}
}