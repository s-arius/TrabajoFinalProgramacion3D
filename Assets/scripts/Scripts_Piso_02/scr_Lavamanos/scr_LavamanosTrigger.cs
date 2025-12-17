 using UnityEngine;

public class scr_LavamanosTrigger : MonoBehaviour
{
    
    [SerializeField] private scr_espejoController myScr_EspejoController;
    [SerializeField] private GameObject panel_Interactuar; 

    void Start()
    {
        myScr_EspejoController = GetComponentInParent<scr_espejoController>();
        
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
            myScr_EspejoController.lavamanos_CanActive = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            panel_Interactuar.SetActive(false);
            myScr_EspejoController.lavamanos_CanActive = false;
        }
    }


}
