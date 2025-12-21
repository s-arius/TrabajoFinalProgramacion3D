using UnityEngine;

public class NotaMensaje : MonoBehaviour
{
  public GameObject panelNota;
    private bool jugadorCerca = false;
    private bool notaAbierta = false;
    


    void Start()
    {
        if (panelNota != null)
            panelNota.SetActive(false);

    }
    
    void Update()
    {
   
        
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            if (!notaAbierta)

                AbrirNota();

            else
                CerrarNota();

        }
        

/*
        if (notaAbierta && Input.GetKeyDown(KeyCode.Escape))
        {
            CerrarNota();
        }
        */
    }
    
    void AbrirNota()
    {
        notaAbierta = true;
        if (panelNota != null) panelNota.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;


    }
    
    void CerrarNota()
    {
        notaAbierta = false;
        if (panelNota != null) panelNota.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;


    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            Debug.Log("E para leer nota");
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            if (notaAbierta) CerrarNota();


        }
}
}