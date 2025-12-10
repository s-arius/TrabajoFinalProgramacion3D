using UnityEngine;
using UnityEngine.UI;

public class MixArea : MonoBehaviour
{
    public Slider progressBar;          // Barra que está sobre el cubo
    public StaminaSystem staminaSystem; // Referencia al sistema de stamina

    public DraggableObject agua;        // Arrastra aquí tu objeto "agua"
    public DraggableObject jabon;       // Arrastra aquí tu objeto "jabón"

    private bool aguaInside = false;
    private bool jabonInside = false;

    void Start()
    {
        progressBar.minValue = 0f;
        progressBar.maxValue = 100f;
        progressBar.value = 0f;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == agua.gameObject)
        {
            aguaInside = true;
            progressBar.value = 50f;
        }
        else if (other.gameObject == jabon.gameObject)
        {
            jabonInside = true;
            progressBar.value = aguaInside ? 100f : 50f;
        }

        CheckMixComplete();
    }

    void CheckMixComplete()
    {
        if (aguaInside && jabonInside)
        {
            // Recuperar estamina al 100%
            staminaSystem.FullRestore();

            // Reset barra
            progressBar.value = 0f;

            // Reset variables
            aguaInside = false;
            jabonInside = false;

            // Volver objetos a su posición original
            agua.ResetToStart();
            jabon.ResetToStart();
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Si se sale, la mezcla ya no cuenta
        if (other.gameObject == agua.gameObject)
        {
            aguaInside = false;
            progressBar.value = jabonInside ? 50f : 0f;
        }

        if (other.gameObject == jabon.gameObject)
        {
            jabonInside = false;
            progressBar.value = aguaInside ? 50f : 0f;
        }
    }
}
