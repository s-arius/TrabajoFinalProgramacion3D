using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance;

    [Header("Estado del ascensor")]
    public int currentFloor;

    [Header("Estado de borrado de cristales")]
    public bool[] crystalErased; // un array donde cada índice representa un cristal

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // persiste entre escenas
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
