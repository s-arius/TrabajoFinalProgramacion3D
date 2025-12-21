using UnityEngine;

public class Nota : MonoBehaviour
{
    public Texture2D imagenNota;
    private bool viendoNota = false;
    

    /*
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(" e para leer la nota");
        }
    }

    */
    
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.F))
        {
            if (!viendoNota)
            {

                viendoNota = true;
                
            }
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            viendoNota = false;
        }
    }
}