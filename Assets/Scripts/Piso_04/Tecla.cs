using UnityEngine;

public class Tecla : MonoBehaviour
{

    private bool jugadorCerca = false;
    
    void Start()
    {
        //gameObject.SetActive(false);
    }
    
    void Update()
    {

        // Ahora solo verificar proximidad
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            Recoger();
        }
    }

    
    void Recoger()
    {
        if (GameManagerGlobal.Instance.teclaRecogida) return;
        
        GameManagerGlobal.Instance.teclaRecogida = true;
        

            
        gameObject.SetActive(false);

        Debug.Log("tecla recogida");
    }

    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;

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
