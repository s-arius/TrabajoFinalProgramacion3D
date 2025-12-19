using UnityEngine;

public class Maletin : MonoBehaviour
{
    //modelos
    public GameObject maletinCerrado;
    public GameObject maletinAbierto; 

    public GameObject botonEsc;

    //audios
    public AudioSource audioCerrado;
    public AudioSource audioAbierto;
    
    //estados
    public bool estaAbierto = false;
    private bool jugadorCerca = false;
    private Inventario inventario;

    //nota UI
    public GameObject panelNota;
    private bool notaVisible = false;

    void Start()
    {
        maletinCerrado.SetActive(true);
        maletinAbierto.SetActive(false);
        inventario = FindObjectOfType<Inventario>();

        // esconder nota al principio
        if (panelNota != null)
            panelNota.SetActive(false);
    }

    void Update()
    {
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            if (!estaAbierto)
            {

                IntentarAbrir();
            }
            else
            {
                ToggleNota();
            }
        }
        
        if (notaVisible && Input.GetKeyDown(KeyCode.Escape))
        {
            OcultarNota();
        }
    }

    void IntentarAbrir()
    {
        if (inventario != null && inventario.hasKey)
        {
            abrirMaletin();
        }
        else
        {
            if (audioCerrado != null)
                audioCerrado.Play();
            Debug.Log("necesitas la llave para abrir el maletin");
        }
    }

    void abrirMaletin()
    {
        estaAbierto = true;
        maletinCerrado.SetActive(false);
        maletinAbierto.SetActive(true);
        
        if (audioAbierto != null)
            audioAbierto.Play();
            
        Debug.Log("maletin abiertoo");
    }

    void ToggleNota()
    {
        if (notaVisible)
        {
            OcultarNota();
        }
        else
        {
            VerNota();
        }
    }
    
    void VerNota()
    {
        if (panelNota != null)
        {
            notaVisible = true;
            panelNota.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;


            JugadorQuieto(true);


        }
    }
    
    void OcultarNota()
    {
        if (panelNota != null)
        {
            notaVisible = false;
            panelNota.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;



        JugadorQuieto(false);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            if (!estaAbierto)
                Debug.Log("E para abrir maletin");
            else
                Debug.Log("E para ver la nota");
        }
    }

    void JugadorQuieto(bool bloquear)
{
    GameObject jugador = GameObject.FindGameObjectWithTag("Player");
    
    if (jugador != null)
    {
        CharacterController controller = jugador.GetComponent<CharacterController>();
        if (controller != null)
        {
            controller.enabled = !bloquear;
        }
    }
}


/*
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            // ocultar nota cuando el jugador se aleja
            if (notaVisible)
                OcultarNota();
        }
    }
    */
}