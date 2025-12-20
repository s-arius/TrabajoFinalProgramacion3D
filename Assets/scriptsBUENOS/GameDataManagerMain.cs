using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Estado del ascensor")]
    public int currentFloor = 100; // Piso inicial por defecto

    [Header("Estado del jugador")]
    public float playerY = 0f; // Posición Y del jugador

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persiste entre escenas
            Debug.Log("[GameManager] Creado y persistente.");
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("[GameManager] Duplicado destruido.");
        }
    }
}
