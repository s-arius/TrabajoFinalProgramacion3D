using UnityEngine;

public class Tecla : MonoBehaviour
{
    public string nombreItem = "tecla";
    public AudioClip sonidoTecla;
    
    private bool jugadorCerca = false;
    private Inventario inventario;
    
    void Start()
    {
inventario = FindFirstObjectByType<Inventario>();

    }
    
    void Update()
    {
        if (jugadorCerca && Input.GetKeyDown(KeyCode.F))
        {
            Recoger();
        }
    }
    
    void Recoger()
    {
        if (inventario != null)
        {
            inventario.AddItem(nombreItem);
            
            if (sonidoTecla != null)
            {
                AudioSource.PlayClipAtPoint(sonidoTecla, transform.position);
            }
            

            gameObject.SetActive(false);
            
        }
        else
        {
            Debug.LogError("no puedes coger la tecla");

        }
    }
    

    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            Debug.Log("Pulsa F para recoger la tecla");
            
            // Opcional: mostrar UI feedback
            // Puedes activar un texto "Pulsa F" sobre el objeto
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            Debug.Log("Te has alejado de la tecla");
            
            // Opcional: ocultar UI feedback
        }
    }
    */
   
}