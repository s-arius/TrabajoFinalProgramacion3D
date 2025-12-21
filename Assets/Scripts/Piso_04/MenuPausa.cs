using UnityEngine;
using UnityEngine.UI;

public class MenuPausa : MonoBehaviour
{
    public GameObject panelPausa;
    public Button botonContinuar;
    public Button botonSalir;
    


    public KeyCode teclaPausa = KeyCode.Escape;
    
    private bool juegoPausado = false;

    private FPSController fpsController;
    
    void Start()
    {
        if (panelPausa != null)
            panelPausa.SetActive(false);
            
        if (botonContinuar != null)
            botonContinuar.onClick.AddListener(ContinuarJuego);
            
        if (botonSalir != null)
            botonSalir.onClick.AddListener(SalirDelJuego);


//para rotacion camara con el raton
    GameObject jugador = GameObject.FindGameObjectWithTag("Player");
        if (jugador != null)
        {
            fpsController = jugador.GetComponent<FPSController>();
        }
    }
    



            void Update()
    {
        if (Input.GetKeyDown(teclaPausa) && !modoInvestigacion())
        {
            TogglePausa();
        }
    }

    /*
        if (Input.GetKeyDown(teclaPausa))
        {
            TogglePausa();
        }

        */

    

    
    void TogglePausa()
    {
        juegoPausado = !juegoPausado;
        
        if (panelPausa != null)
            panelPausa.SetActive(juegoPausado);
        
        //Time.timeScale = juegoPausado ? 0f : 1f; //pausar tiempo del juego
        
         Cursor.lockState = juegoPausado ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = juegoPausado;

        JugadorQuieto(juegoPausado);
        
    }
    
    public void ContinuarJuego()
    {
        juegoPausado = false;
        
        if (panelPausa != null)
            panelPausa.SetActive(false);
            
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        JugadorQuieto(false);
        
        Debug.Log("continuar juego");
    }
    
    public void SalirDelJuego()
    {
        Debug.Log("saliendo del juego");
        

        
        Application.Quit(); //solo funciona en la build final creo
    }
    
    void JugadorQuieto(bool bloquear)
    {
        GameObject jugador = GameObject.FindGameObjectWithTag("Player");
        
  if (jugador != null)
        {
            CharacterController controller = jugador.GetComponent<CharacterController>();
            if (controller != null)
            {
                controller.enabled = !bloquear;
            }

        
            
            if (fpsController != null)
            {
                fpsController.enabled = !bloquear;
            }
        }
    }


        private bool modoInvestigacion() //para que no overlapee el esc del modo investigacion
    {
        if (scr_ModoInvestigar.Instancia != null)
        {
            return scr_ModoInvestigar.Instancia.EstaInvestigando_Funcion();
        }
        return false;
    }
 


}