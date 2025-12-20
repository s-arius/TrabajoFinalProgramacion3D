using UnityEngine;
using TMPro; // Si usas TextMeshPro para mostrar el piso

public class FloorDisplay : MonoBehaviour
{
    public ElevatorController elevator; // Referencia al ascensor
    public TextMeshProUGUI floorText;   // Texto que muestra el piso

    void Start()
    {
        if (elevator == null)
        {
            Debug.LogError("[FloorDisplay] No se ha asignado el ElevatorController");
            return;
        }

        // Inicializar texto con el valor del GameManager
        if (GameManager.Instance != null)
        {
            elevator.currentFloor = GameManager.Instance.currentFloor;
            UpdateFloorText();
        }
    }

    void Update()
    {
        if (elevator.currentFloor != GameManager.Instance.currentFloor)
        {
            // Guardar piso en GameManager
            GameManager.Instance.currentFloor = elevator.currentFloor;

            // Actualizar texto en pantalla
            UpdateFloorText();
        }
    }

    void UpdateFloorText()
    {
        if (floorText != null)
            floorText.text = $"Piso: {elevator.currentFloor}";
    }
}
