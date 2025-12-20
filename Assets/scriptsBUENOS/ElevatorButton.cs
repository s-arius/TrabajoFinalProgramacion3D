using UnityEngine;

public class ElevatorButton : MonoBehaviour
{
    public ElevatorController elevator;
    public bool moveUpButton = false; // true = subir, false = bajar
    public FloorDisplay floorDisplay;  // referencia a FloorDisplay

    void OnMouseDown()
    {
        if (elevator == null) return;

        if (moveUpButton)
            elevator.MoveUp();
        else
            elevator.MoveDown();

        // Actualizar visualización del piso y guardar
        if (floorDisplay != null)
        {
            floorDisplay.SetFloor(elevator.currentFloor);
        }
    }
}
