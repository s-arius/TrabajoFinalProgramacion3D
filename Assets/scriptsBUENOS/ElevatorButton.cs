using UnityEngine;

public class ElevatorButton : MonoBehaviour
{
    public ElevatorController elevator;
    public bool moveUpButton = false; // true = up, false = down

    void OnMouseDown()
    {
        if (elevator == null) return;

        if (moveUpButton)
            elevator.MoveUp();
        else
            elevator.MoveDown();
    }
}
