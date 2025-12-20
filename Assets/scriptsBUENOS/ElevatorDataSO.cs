using UnityEngine;

[CreateAssetMenu(fileName = "ElevatorDataSO", menuName = "GameData/ElevatorDataSO")]
public class ElevatorDataSO : ScriptableObject
{
    private int tempFloor = -1;

    public bool HasTemporaryData => tempFloor != -1;

    // Guardar piso temporal (solo durante la ejecución)
    public void SaveTemporaryFloor(int floor)
    {
        tempFloor = floor;
    }

    // Cargar piso temporal
    public int LoadFloor()
    {
        return tempFloor;
    }

    // Resetear datos temporales al salir del juego
    public void ResetData()
    {
        tempFloor = -1;
    }
}
