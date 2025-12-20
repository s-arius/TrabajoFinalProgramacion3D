using UnityEngine;
using TMPro;

public class FloorDisplay : MonoBehaviour
{
    public int currentFloor = 100;   // piso inicial
    public int minFloor = 92;        // piso mínimo
    public int maxFloor = 100;       // piso máximo

    public TextMeshPro textMesh;     // asigna tu TextMeshPro del objeto 3D
    public ElevatorDataSO elevatorData; // referencia al ScriptableObject

    void Start()
    {
        // Cargar el piso guardado solo durante la ejecución
        if (elevatorData != null && elevatorData.HasTemporaryData)
            currentFloor = Mathf.Clamp(elevatorData.LoadFloor(), minFloor, maxFloor);

        UpdateText();
    }

    public void SetFloor(int floor)
    {
        currentFloor = Mathf.Clamp(floor, minFloor, maxFloor);
        UpdateText();

        // Guardar piso temporalmente
        if (elevatorData != null)
            elevatorData.SaveTemporaryFloor(currentFloor);
    }

    void UpdateText()
    {
        if (textMesh != null)
            textMesh.text = currentFloor.ToString();
    }
}
