using UnityEngine;

public class Nota : MonoBehaviour
{
    public Texture2D imagenNota;
    private bool mostrando = false;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Pulsa F para leer la nota");
        }
    }
    
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.F))
        {
            if (!mostrando)
            {

                mostrando = true;
                
            }
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            mostrando = false;
        }
    }
}