using UnityEngine;
using System.Collections.Generic;

public class ElevatorController : MonoBehaviour
{
    public float moveDistance = 100f;
    public float moveSpeed = 5f;

    [Header("Sonidos")]
    public AudioSource errorSound;

    [Header("Configuración inicial")]
    public int startFloor = 100;

    private Vector3 basePosition;
    private Vector3 targetPosition;

    public int currentFloor;
    private int downCount = 0;
    public int maxDown = 8;

    private HashSet<int> unlockedFloors = new HashSet<int>();

    void Awake()
    {
        basePosition = transform.position;
        targetPosition = basePosition;
    }

    void Start()
    {
        // Restaurar el piso desde GameManager
        if (GameManager.Instance != null)
            currentFloor = GameManager.Instance.currentFloor;

        // Ajustar posición según el piso
        int offset = currentFloor - startFloor;
        targetPosition = basePosition + Vector3.up * moveDistance * offset;
        transform.position = targetPosition;

        Debug.Log($"[Elevator] Posición inicial ajustada a piso {currentFloor}.");
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);
    }

    public void MoveUp()
    {
        if (!unlockedFloors.Contains(currentFloor))
        {
            PlayError();
            return;
        }

        targetPosition += Vector3.up * moveDistance;
        currentFloor++;

        if (GameManager.Instance != null)
            GameManager.Instance.currentFloor = currentFloor;

        Debug.Log($"[Elevator] Subiendo a piso {currentFloor}");
    }

    public void MoveDown()
    {
        if (!unlockedFloors.Contains(currentFloor))
        {
            PlayError();
            return;
        }

        if (downCount < maxDown)
        {
            targetPosition += Vector3.down * moveDistance;
            downCount++;
            currentFloor--;

            if (GameManager.Instance != null)
                GameManager.Instance.currentFloor = currentFloor;

            Debug.Log($"[Elevator] Bajando a piso {currentFloor}");
        }
    }

    public void UnlockCurrentFloor()
    {
        unlockedFloors.Add(currentFloor);
        Debug.Log($"[Elevator] Piso {currentFloor} desbloqueado.");
    }

    private void PlayError()
    {
        if (errorSound != null && !errorSound.isPlaying)
            errorSound.Play();

        Debug.Log("[Elevator] Error: Piso no desbloqueado.");
    }
}
