using UnityEngine;

public class Maletin : MonoBehaviour
{
    //modelos
    public GameObject maletinCerrado;
    public GameObject maletinAbierto; 

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
        if (jugadorCerca && Input.GetKeyDown(KeyCode.F))
        {
            if (!estaAbierto)
            {

                TryOpen();
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

    void TryOpen()
    {
        if (inventario != null && inventario.hasKey)
        {
            OpenCase();
        }
        else
        {
            if (audioCerrado != null)
                audioCerrado.Play();
            Debug.Log("Necesitas la llave para abrir el maletín");
        }
    }

    void OpenCase()
    {
        estaAbierto = true;
        maletinCerrado.SetActive(false);
        maletinAbierto.SetActive(true);
        
        if (audioAbierto != null)
            audioAbierto.Play();
            
        Debug.Log("¡Maletín abierto!");
    }

    void ToggleNota()
    {
        if (notaVisible)
        {
            OcultarNota();
        }
        else
        {
            MostrarNota();
        }
    }
    
    void MostrarNota()
    {
        if (panelNota != null)
        {
            notaVisible = true;
            panelNota.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Debug.Log("Nota mostrada en pantalla");
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
            Debug.Log("Nota oculta");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            if (!estaAbierto)
                Debug.Log("Pulsa F para abrir el maletín");
            else
                Debug.Log("Pulsa F para ver la nota");
        }
    }

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
}