using UnityEngine;

public class GameManagerGlobal : MonoBehaviour
{
    public static GameManagerGlobal Instance;

    [Header("Estado global de la llave")]
    public bool teclaRecogida = false;   // true si ya recogiste la tecla
    public bool lucesApagadas = false;   // true si la luz ya fue apagada

    private void Awake()
    {
        // Singleton: solo una instancia
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // No se destruye al cambiar de escena
        }
        else
        {
            Destroy(gameObject); // Destruye duplicados
        }
    }
}
