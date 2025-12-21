using UnityEngine;

public class Maletin : MonoBehaviour
{
    public GameObject maletinCerrado;
    public GameObject maletinAbierto;
    public GameObject areaNota;
        public GameObject panelNota;
   // private Button botonEscComponent;
    


    public AudioSource audioCerrado;
    public AudioSource audioAbierto;
    


    
    
    private bool estaAbierto = false;
    private bool jugadorCercaMaletin = false;
    private bool jugadorCercaNota = false;
    private Inventario inventario;
    private bool notaVisible = false;
    


    void Start()
    {
        maletinCerrado.SetActive(true);
        maletinAbierto.SetActive(false);
        
        if (areaNota != null)
            areaNota.SetActive(false);
            
        if (panelNota != null)
            panelNota.SetActive(false);

            
inventario = FindFirstObjectByType<Inventario>();


/*
            if (botonEsc != null)
    {
        botonEscComponent = botonEsc.GetComponent<Button>();
        if (botonEscComponent != null)
        {
            botonEscComponent.onClick.AddListener(OcultarNota);
        }
        botonEsc.SetActive(false);
    }

    */
    }
    
    void Update()
    {

           // Debug temporal
    if (Input.GetKeyDown(KeyCode.P))
    {
        if (scr_MensajeManager.Instance != null)
        {
            scr_MensajeManager.Instance.MostrarMensaje("TEST: Mensaje funciona", 3f);
            Debug.Log("TEST enviado a UI");
        }
        else
        {
            Debug.LogError("scr_MensajeManager.Instance es NULL!");
        }
    }
        if (jugadorCercaMaletin && Input.GetKeyDown(KeyCode.E) && !estaAbierto)
        {
            IntentarAbrir();
        }
        



        if (jugadorCercaNota && Input.GetKeyDown(KeyCode.E) && estaAbierto && !notaVisible)
        {
            VerNota();
        }
        
        // cerrar nota con esc
        if (notaVisible && Input.GetKeyDown(KeyCode.Escape))
        {
            OcultarNota();
        }
    }
    
    public void IntentarAbrir()
    {
        if (inventario != null && inventario.tienesLlave)
        {
            AbrirMaletin();
        }
        else
        {
            if (audioCerrado != null)
                audioCerrado.Play();
            Debug.Log("necesitas la llave");
        }
    }
    
    public void AbrirMaletin()
    {
        estaAbierto = true;
        maletinCerrado.SetActive(false);
        maletinAbierto.SetActive(true);
        

        if (areaNota != null)
            areaNota.SetActive(true);
        
        if (audioAbierto != null)
            audioAbierto.Play();
            
        Debug.Log("maletin abierto");
    }
    
    public void VerNota()
    {
        notaVisible = true;
        
        if (panelNota != null)
            panelNota.SetActive(true);
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        JugadorQuieto(true);


/*

                if (botonEsc != null)
        {
            botonEsc.SetActive(true);
        }
        */
    }
    
    public void OcultarNota()
    {
        notaVisible = false;
        
        if (panelNota != null)
            panelNota.SetActive(false);
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        JugadorQuieto(false);
    }
    
    public void JugadorQuieto(bool bloquear)
    {
        GameObject jugador = GameObject.FindGameObjectWithTag("Player");
        if (jugador != null)
        {
            CharacterController cc = jugador.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = !bloquear;
        }
    }
    

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!estaAbierto)
            {
                jugadorCercaMaletin = true;
                Debug.Log("pulsa e para abrir maletín");
            }
        }
    }
    
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCercaMaletin = false;
        }
    }
    


    public void EntrarAreaNota()
    {
        if (estaAbierto)
        {
            jugadorCercaNota = true;
            Debug.Log("e para ver la nota");
        }
    }
    
    public void SalirAreaNota()
    {
        jugadorCercaNota = false;
        if (notaVisible)
            OcultarNota();
    }
}