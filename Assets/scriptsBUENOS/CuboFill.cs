using UnityEngine;
using UnityEngine.UI;

public class CuboFill : MonoBehaviour
{
    public Slider slider;

    public void SetTo50()
    {
        slider.gameObject.SetActive(true);
        slider.value = 0.5f;
    }

    public void SetTo100()
    {
        slider.gameObject.SetActive(true);
        slider.value = 1f;
    }
}
