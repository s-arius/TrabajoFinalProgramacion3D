using UnityEngine;

public class ElevatorButton : MonoBehaviour
{
    public ElevatorController elevator;
    public bool moveUpButton = false; // true = subir, false = bajar

    public FloorDisplay floorDisplay;

    void OnMouseDown()
    {
        if (elevator == null) return;

        if (moveUpButton)
        {
            elevator.MoveUp();
        }
        else
        {
            elevator.MoveDown();
        }

        // Actualizar visualización del piso
        if (floorDisplay != null)
        {
            floorDisplay.SetFloor(elevator.currentFloor);
        }
    }
}
