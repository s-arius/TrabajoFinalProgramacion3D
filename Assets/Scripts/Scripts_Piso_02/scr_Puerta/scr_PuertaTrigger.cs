using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class scr_PuertaTrigger : MonoBehaviour
{

    //NOTA: POR HACER: Debo de pasar el funcionamiento del trigger en la puerta para que funcione aqui.
    
    [SerializeField] private scr_doorController myscr_PuertaController;
    [SerializeField] private GameObject panel_Interactuar; 

    void Start()
    {
        myscr_PuertaController = GetComponentInParent<scr_doorController>();
        

        if (panel_Interactuar == null) Debug.LogWarning("PanelInteractuar vacio");

        panel_Interactuar.SetActive(false);
    }
    void Update()
    {
       
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            panel_Interactuar.SetActive(true);
            myscr_PuertaController.enRango = true;

            /*if (myscr_PuertaController.isOpen == true) //Intento de ocultar el panel cuando el objeto se destruya
            {
                panel_Interactuar.SetActive(false);
            }*/
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            panel_Interactuar.SetActive(false);
            myscr_PuertaController.enRango = false;
            
        }
    }



    

}