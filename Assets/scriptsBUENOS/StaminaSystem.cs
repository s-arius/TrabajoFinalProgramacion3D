using UnityEngine;
using UnityEngine.UI;

public class StaminaSystem : MonoBehaviour
{
    public Slider staminaSlider;
    public float maxStamina = 100f;
    public float stamina;
    public float drainRate = 10f;      // velocidad de desgaste por segundo
    public float regenRate = 5f;       // velocidad de recuperación por segundo

    public bool isErasing = false;

    void Start()
    {
        stamina = maxStamina;
        staminaSlider.maxValue = maxStamina;
        staminaSlider.value = stamina;
    }

    void Update()
    {
        if (isErasing)
        {
            stamina -= drainRate * Time.deltaTime;
            stamina = Mathf.Clamp(stamina, 0, maxStamina);
        }
        else
        {
            stamina += regenRate * Time.deltaTime;
            stamina = Mathf.Clamp(stamina, 0, maxStamina);
        }

        staminaSlider.value = stamina;
    }
    public void FullRestore()
    {
        stamina = maxStamina;
        staminaSlider.value = maxStamina;
    }


    public bool CanErase()
    {
        return stamina > 0;
    }
}
