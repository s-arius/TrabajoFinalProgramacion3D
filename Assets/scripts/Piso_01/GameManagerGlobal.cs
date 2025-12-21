using UnityEngine;
using UnityEngine.Events;

public class GameManagerGlobal : MonoBehaviour
{
    public static GameManagerGlobal Instance;

    [Header("Estado global de la llave")]
    public bool teclaRecogida = false;   // true si ya recogiste la tecla
    public bool lucesApagadas = false;   // true si la luz ya fue apagada
    public bool teclaColocada = false;   // true si la tecla ya se colocó
    public bool objetoVisible = false;   // true si el objeto ya apareció
    public bool llaveRecogida = false;
    public bool maletinAbierto = false;



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
