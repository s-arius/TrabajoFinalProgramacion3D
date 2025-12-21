using UnityEngine;

public class Maletin : MonoBehaviour
{
    public GameObject maletinCerrado;
    public GameObject maletinAbierto;
   // public AudioSource sonidoCerrado;
    //public AudioSource sonidoAbierto;
    
    private bool jugadorCerca = false;
    
    void Start()
    {
        maletinCerrado.SetActive(true);
        maletinAbierto.SetActive(false);
    }
    
    void Update()
    {
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            IntentarAbrir();
        }
    }
    
    void IntentarAbrir()
    {
        if (GameManagerGlobal.Instance.maletinAbierto) return;
        
        if (GameManagerGlobal.Instance.llaveRecogida)
        {
            AbrirMaletin();
        }
        else
        {
            
           // sonido
            Debug.Log("Necesitas la llave");
        }
    }
    
    void AbrirMaletin()
    {
        GameManagerGlobal.Instance.maletinAbierto = true;
        maletinCerrado.SetActive(false);
        maletinAbierto.SetActive(true);
        
        Debug.Log("Maletín abierto");
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            Debug.Log("Pulsa E para abrir maletín");
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
        }
        }

        
}