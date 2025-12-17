using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    public float moveDistance = 100f;
    public float moveSpeed = 5f;
    [Header("Sonidos")]
    public AudioSource errorSound;   // ❌ error al bajar sin permiso


    private Vector3 targetPosition;
    private Vector3 initialPosition;
    private int downCount = 0;
  


    public int maxDown = 8;
    public int currentFloor = 100;

    // 🔒 BLOQUEO POR CRISTAL
    private bool canGoDown = false;

    void Start()
    {
        targetPosition = transform.position;
        initialPosition = transform.position;
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
        if (downCount > 0)
        {
            targetPosition += Vector3.up * moveDistance;
            downCount--;
            currentFloor++;

            // al subir, el siguiente cristal vuelve a bloquear
            canGoDown = false;
        }
        else
        {
            Debug.Log("No se puede subir más de la planta 100");
        }
    }

    public void MoveDown()
    {
        if (!canGoDown)
        {
            // ❌ sonido de error
            if (errorSound != null && !errorSound.isPlaying)
                errorSound.Play();

            Debug.Log("❌ No puedes bajar: cristal no completado");
            return;
        }

        if (downCount < maxDown)
        {
            targetPosition += Vector3.down * moveDistance;
            downCount++;
            currentFloor--;

            canGoDown = false; // 🔒 vuelve a bloquear hasta el siguiente cristal
        }
        else
        {
            Debug.Log("⛔ Límite inferior alcanzado");
        }
    }


    public void UnlockNextFloor()
    {
        canGoDown = true;
    }

}
