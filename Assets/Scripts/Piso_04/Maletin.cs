using UnityEngine;

public class Maletin : MonoBehaviour
{
    public GameObject maletinCerrado;
    public GameObject maletinAbierto;
    
    private bool jugadorCerca = false;

    //llamar script de objeto bloqueador
    public scr_ObjetoBloqueador block;
    
    void Start()
    {
        maletinCerrado.SetActive(true);
        maletinAbierto.SetActive(false);
    }
    
    void Update()
    {


        if (block != null && block.maletinActivado && !GameManagerGlobal.Instance.maletinAbierto)
        {
            AbrirMaletin();
        }

    }
    

    /*
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

    */
    
    void AbrirMaletin()
    {
       GameManagerGlobal.Instance.maletinAbierto = true;
        maletinCerrado.SetActive(false);
        maletinAbierto.SetActive(true);
        Debug.Log("Maletín abierto automáticamente");


        

    }
    /*
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

        */
}