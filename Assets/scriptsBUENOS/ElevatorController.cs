using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    public float moveDistance = 100f;
    public float moveSpeed = 5f;

    private Vector3 targetPosition;
    private Vector3 initialPosition;  // posición inicial para limitar subida
    private int downCount = 0;        // cantidad de pisos bajados desde la inicial
    public int maxDown = 8;           // máximo de pisos que se puede bajar

    public int currentFloor = 100;    // piso inicial

    void Start()
    {
        targetPosition = transform.position;
        initialPosition = transform.position;
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);
    }

    public void MoveUp()
    {
        // No subir más allá de la posición inicial
        if (downCount > 0)
        {
            targetPosition += Vector3.up * moveDistance;
            downCount--;
            currentFloor++;
        }
        else
        {
            Debug.Log("No se puede subir más desde la posición inicial");
        }
    }

    public void MoveDown()
    {
        // Solo bajar si no se ha llegado al límite
        if (downCount < maxDown)
        {
            targetPosition += Vector3.down * moveDistance;
            downCount++;
            currentFloor--;
        }
        else
        {
            Debug.Log("Se ha alcanzado el límite de pisos hacia abajo");
        }
    }
}
