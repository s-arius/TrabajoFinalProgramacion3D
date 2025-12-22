using UnityEngine;

public class scr_InterruptorLuces : MonoBehaviour
{

    [Header("Configuraci�n")]
    [SerializeField] private KeyCode teclaInteraccion = KeyCode.E;
    [SerializeField] private Animator animatorInterruptor;

    [Header("UI")]
    [SerializeField] private GameObject panel_Interactuar;

    private bool estaEnRango = false;

    private void Start()
    {
        ActualizarVisualPalanca();
    }


    private void Update()
    {
        if (!estaEnRango) return;


        if (Input.GetKeyDown(teclaInteraccion))
        {
            AlternarLuces();
        }
    }

    private void AlternarLuces()
    {
        bool lucesActualmenteApagadas = GameManagerGlobal.Instance.lucesApagadas;

        if (lucesActualmenteApagadas)
            ControladorLuces.Instance.EncenderLuces();
        else
            ControladorLuces.Instance.ApagarLuces();

        ActualizarVisualPalanca();
    }

    private void ActualizarVisualPalanca()
    {
        if (animatorInterruptor == null) return;

        bool lucesApagadas = GameManagerGlobal.Instance.lucesApagadas;

        if (!lucesApagadas) animatorInterruptor.SetTrigger("ON_Interruptor");
        else if(lucesApagadas) animatorInterruptor.SetTrigger("OFF_Interruptor");

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            panel_Interactuar.SetActive(true);
            estaEnRango = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            panel_Interactuar.SetActive(false);
            estaEnRango = false;
        }
    }

}
