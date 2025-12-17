using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    public float moveDistance = 100f;
    public float moveSpeed = 5f;

    [Header("Sonidos")]
    public AudioSource errorSound;   // ❌ error al intentar mover sin desbloqueo

    private Vector3 targetPosition;
    private Vector3 initialPosition;

    public int currentFloor = 100;
    public int maxDown = 8;
    private int downCount = 0;

    // Registro de qué plantas tienen el cristal borrado
    private HashSet<int> unlockedFloors = new HashSet<int>();

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
        int nextFloor = currentFloor + 1;

        // Solo permite subir si la planta actual está desbloqueada
        if (!unlockedFloors.Contains(currentFloor))
        {
            PlayError();
            Debug.Log("❌ Debes borrar el cristal de esta planta antes de subir.");
            return;
        }

        if (downCount > 0)
        {
            targetPosition += Vector3.up * moveDistance;
            downCount--;
            currentFloor++;

            Debug.Log($"⬆ Subiste a la planta {currentFloor}");
        }
        else
        {
            Debug.Log("⛔ Ya estás en la planta superior.");
        }
    }

    public void MoveDown()
    {
        int nextFloor = currentFloor - 1;

        // Solo permite bajar si la planta actual está desbloqueada
        if (!unlockedFloors.Contains(currentFloor))
        {
            PlayError();
            Debug.Log("❌ Debes borrar el cristal de esta planta antes de bajar.");
            return;
        }

        if (downCount < maxDown)
        {
            targetPosition += Vector3.down * moveDistance;
            downCount++;
            currentFloor--;

            Debug.Log($"⬇ Bajaste a la planta {currentFloor}");
        }
        else
        {
            Debug.Log("⛔ Límite inferior alcanzado.");
        }
    }

    // Método que llama CrystalEraseUnlocker cuando se borra el cristal
    public void UnlockCurrentFloor()
    {
        if (!unlockedFloors.Contains(currentFloor))
        {
            unlockedFloors.Add(currentFloor);
            Debug.Log($"🔓 Planta {currentFloor} desbloqueada.");
        }
    }

    private void PlayError()
    {
        if (errorSound != null && !errorSound.isPlaying)
            errorSound.Play();
    }
}
