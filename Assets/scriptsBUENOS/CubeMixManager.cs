using UnityEngine;
using UnityEngine.UI;

public class CubeMixManager : MonoBehaviour
{
    [Header("Referencias")]
    public Slider slider;                  // Slider (World Space) sobre el cubo
    public StaminaSystem staminaSystem;    // Referencia al sistema de stamina
    public DraggableObject agua;           // Objeto "agua" (poner en Inspector)
    public DraggableObject jabon;          // Objeto "jabon" (poner en Inspector)

    bool aguaInside = false;
    bool jabonInside = false;

    void Start()
    {
        if (slider != null)
        {
            slider.minValue = 0f;
            slider.maxValue = 1f; // trabajamos en 0..1
            slider.value = 0f;
            slider.gameObject.SetActive(false); // oculto hasta que entre algo
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Asegurarnos de que el objeto que entra sea uno de los dos (por Tag o por referencia)
        if (other.gameObject == agua?.gameObject && !aguaInside)
        {
            aguaInside = true;
            UpdateSliderOnEnter();
            CheckMixComplete();
            return;
        }

        if (other.gameObject == jabon?.gameObject && !jabonInside)
        {
            jabonInside = true;
            UpdateSliderOnEnter();
            CheckMixComplete();
            return;
        }

        // Alternativa si quieres usar tags en vez de referencias directas:
        // if (other.CompareTag("Agua")) { ... } etc.
    }

    void OnTriggerExit(Collider other)
    {
        // Si sale, desmarcamos y ajustamos la barra visual
        if (other.gameObject == agua?.gameObject && aguaInside)
        {
            aguaInside = false;
            UpdateSliderOnExit();
        }
        else if (other.gameObject == jabon?.gameObject && jabonInside)
        {
            jabonInside = false;
            UpdateSliderOnExit();
        }
    }

    void UpdateSliderOnEnter()
    {
        if (slider == null) return;
        slider.gameObject.SetActive(true);

        // Si ninguno estaba dentro antes -> poner 50%
        // Si el otro ya estaba dentro -> se manejará en CheckMixComplete
        if (!aguaInside || !jabonInside)
        {
            // Si solo uno está dentro (cualquiera) -> 50%
            if (aguaInside ^ jabonInside) slider.value = 0.5f;
        }
    }

    void UpdateSliderOnExit()
    {
        if (slider == null) return;

        // Si ninguno está dentro -> ocultar y poner 0
        if (!aguaInside && !jabonInside)
        {
            slider.value = 0f;
            slider.gameObject.SetActive(false);
            return;
        }

        // Si uno sigue dentro -> mostrar 50%
        if (aguaInside ^ jabonInside)
        {
            slider.value = 0.5f;
            slider.gameObject.SetActive(true);
        }
    }

    void CheckMixComplete()
    {
        if (aguaInside && jabonInside)
        {
            // Mezcla completa: recuperar estamina al 100%
            if (staminaSystem != null)
            {
                // Asegúrate de que StaminaSystem tenga el método FullRestore()
                staminaSystem.FullRestore();
            }

            // Visualmente la barra se resetea a 0 (tal como pediste)
            if (slider != null)
            {
                slider.value = 0f;
                slider.gameObject.SetActive(false); // opcionalmente ocultar
            }

            // Volver los objetos a su posición inicial (ResetToStart)
            if (agua != null) agua.ResetToStart();
            if (jabon != null) jabon.ResetToStart();

            // Resetear estado para permitir una nueva mezcla
            aguaInside = false;
            jabonInside = false;
        }
    }
}
