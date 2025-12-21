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
        /*
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
        }

        */

                    AbrirMaletin();

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
         if (block.maletinActivado) {
        GameManagerGlobal.Instance.maletinAbierto = true;
        maletinCerrado.SetActive(false);
        maletinAbierto.SetActive(true);
                Debug.Log("maletin abierto en script maletin");
         }


        

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