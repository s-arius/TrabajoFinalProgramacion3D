using UnityEngine;

public class scr_LlaveDoradaTrigger : MonoBehaviour
{


    //NOTA: POR HACER: Debo de pasar el funcionamiento del trigger en la puerta para que funcione aqui.

    [SerializeField] private GameObject panel_Interactuar;

    void Start()
    {
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
