using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class scr_espejoController : MonoBehaviour
{
    [Header("FindObjects")]
    [SerializeField] private GameObject agua_Chorro;
    [SerializeField] private GameObject mensaje_Secreto;
    

    //Config Chorro de agua
    internal bool lavamanos_CanActive = false;
    private bool chorro_On = false;

    //Config mensaje secreto
    private scr_FadeMensaje miScr_FadeMensaje;






    void Start()
    {
        //Refencia de gameobjects via jerarquica //Esto no es un JSON 
        agua_Chorro = GetComponentsInChildren<Transform>(true) //True -> Sin importar si hay algun objeto desactivado, busca en todos los hijos de la herarquia
                     .First(i => i.name == "ChorroAgua")       // “Dame el primer transform encontrado que se llame ChorroAgua” // "=>" es un operador lambda,es una buena alternativa para los bucles for para recorrer/filtrar listas.
                     .gameObject;                              //Una vez encontrado el transform accede al gameobject

        mensaje_Secreto = GetComponentsInChildren<Transform>(true) 
                     .First(i => i.name == "MensajeSecreto")       
                     .gameObject;

        if (agua_Chorro == null) Debug.LogWarning("agua_Chorros vacio");
        if (mensaje_Secreto == null) Debug.LogWarning("MensajeSecreto vacio");
       

        agua_Chorro.SetActive(false);

        miScr_FadeMensaje = mensaje_Secreto.GetComponent<scr_FadeMensaje>();

    }


    void Update()
    {
        if (lavamanos_CanActive)
        {
            Debug.Log("lavamanos_CanActive: " + lavamanos_CanActive);
           

            //Activar ui "Press E to interact"
            if (Input.GetKeyDown(KeyCode.E))
            {
                agua_Chorro.SetActive(true);
                chorro_On = !chorro_On;

                if (chorro_On)
                {
                    agua_Chorro.SetActive(true);
                    miScr_FadeMensaje.fade_Activar(true);


                }
                else
                {
                    agua_Chorro.SetActive(false);
                    miScr_FadeMensaje.fade_Activar(false);
                }

            }

        }
        else
        {
            Debug.Log("lavamanos_CanActive: " + lavamanos_CanActive);
            
        }
    }

 

}
