using Unity.VisualScripting;
using UnityEngine;

public class scr_PuertaTrigger : MonoBehaviour
{

    //NOTA: POR HACER: Debo de pasar el funcionamiento del trigger en la puerta para que funcione aqui.
    
    [SerializeField] private scr_PuertaTrigger myscr_PuertaTrigger;
    [SerializeField] private GameObject panel_Interactuar; //Arrastar en el inspector

    void Start()
    {
        myscr_PuertaTrigger = GetComponentInParent<scr_PuertaTrigger>();

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
            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            panel_Interactuar.SetActive(false);
            
        }
    }


}