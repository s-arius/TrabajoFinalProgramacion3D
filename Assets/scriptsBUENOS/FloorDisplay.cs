using UnityEngine;
using TMPro;

public class FloorDisplay : MonoBehaviour
{
    public int currentFloor = 100;   // piso inicial
    public int minFloor = 92;        // piso mínimo
    public int maxFloor = 100;       // piso máximo

    public TextMeshPro textMesh;    // asigna tu TextMeshPro del objeto 3D

    void Start()
    {
        UpdateText();
    }

    public void SetFloor(int floor)
    {
        currentFloor = Mathf.Clamp(floor, minFloor, maxFloor);
        UpdateText();
    }

    void UpdateText()
    {
        if (textMesh != null)
        {
            textMesh.text = currentFloor.ToString();
        }
    }
}
