using UnityEngine;

public class UIFinal : MonoBehaviour
{
    [Header("UI a activar")]
    public GameObject ui;

    [Header("Opciones")]
    public bool ocultarAlSalir = true;

    private void Start()
    {
        if (ui != null)
            ui.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (ui != null)
                ui.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && ocultarAlSalir)
        {
            if (ui != null)
                ui.SetActive(false);
        }
    }
}
