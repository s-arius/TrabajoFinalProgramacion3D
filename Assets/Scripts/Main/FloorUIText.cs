using UnityEngine;
using TMPro;

public class FloorText3D : MonoBehaviour
{
    public TextMeshPro floorText;

    void Start()
    {
        if (floorText == null)
            Debug.LogError("[FloorText3D] No se ha asignado el TextMeshPro 3D");
        else

        if (GameManager.Instance != null)
            floorText.text = $" {GameManager.Instance.currentFloor}";
    }

    void Update()
    {
        if (GameManager.Instance != null && floorText != null)
        {
            floorText.text = $" {GameManager.Instance.currentFloor}";
        }
    }
}
