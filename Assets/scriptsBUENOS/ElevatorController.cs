using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    public float moveDistance = 100f;
    public float moveSpeed = 5f;

    [Header("Sonidos")]
    public AudioSource errorSound;

    [Header("Configuración pisos")]
    public int startFloor = 100;   // Piso donde empieza el juego
    public int maxDown = 8;

    public int currentFloor;

    private Vector3 initialPosition;
    private Vector3 targetPosition;

    private int downCount = 0;

    // Plantas desbloqueadas por limpieza
    private HashSet<int> unlockedFloors = new HashSet<int>();

    void Start()
    {
        initialPosition = transform.position;

        // 🔁 SINCRONIZAR CON GAMEMANAGER
        if (GameManager.Instance != null)
            currentFloor = GameManager.Instance.currentFloor;
        else
            currentFloor = startFloor;

        // 🔢 Calcular cuántos pisos se ha movido
        int floorOffset = startFloor - currentFloor;
        downCount = Mathf.Clamp(floorOffset, 0, maxDown);

        // 📍 Recolocar ascensor correctamente
        Vector3 offset = Vector3.down * moveDistance * downCount;
        transform.position = initialPosition + offset;
        targetPosition = transform.position;

        Debug.Log($"[Elevator] Posición restaurada. Piso {currentFloor}, downCount {downCount}");
    }

    void Update()
    {
        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            Time.deltaTime * moveSpeed
        );
    }

    public void MoveUp()
    {
        if (!CanMoveFromCurrentFloor())
        {
            PlayError();
            Debug.Log("❌ Debes limpiar el cristal antes de subir.");
            return;
        }

        if (downCount > 0)
        {
            targetPosition += Vector3.up * moveDistance;
            downCount--;
            currentFloor++;

            UpdateGameManager();
            CheckFloor92();

            Debug.Log($"⬆ Subiste a la planta {currentFloor}");
        }
    }

    public void MoveDown()
    {
        if (!CanMoveFromCurrentFloor())
        {
            PlayError();
            Debug.Log("❌ Debes limpiar el cristal antes de bajar.");
            return;
        }

        if (downCount < maxDown)
        {
            targetPosition += Vector3.down * moveDistance;
            downCount++;
            currentFloor--;

            UpdateGameManager();
            CheckFloor92();

            Debug.Log($"⬇ Bajaste a la planta {currentFloor}");
        }
    }

    // 🔓 Llamado al limpiar un cristal
    public void UnlockCurrentFloor()
    {
        if (!unlockedFloors.Contains(currentFloor))
        {
            unlockedFloors.Add(currentFloor);
            Debug.Log($"🔓 Planta {currentFloor} desbloqueada.");
        }
    }

    // 🚦 Lógica de bloqueo
    bool CanMoveFromCurrentFloor()
    {
        if (GameManager.Instance != null &&
            GameManager.Instance.hasReachedFloor92)
            return true;

        return unlockedFloors.Contains(currentFloor);
    }

    // 🏁 Piso especial
    void CheckFloor92()
    {
        if (GameManager.Instance != null &&
            currentFloor <= 92 &&
            !GameManager.Instance.hasReachedFloor92)
        {
            GameManager.Instance.hasReachedFloor92 = true;
            Debug.Log("🏁 Piso 92 alcanzado. Ascensor desbloqueado permanentemente.");
        }
    }

    void UpdateGameManager()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.currentFloor = currentFloor;
    }

    void PlayError()
    {
        if (errorSound != null && !errorSound.isPlaying)
            errorSound.Play();
    }
}
