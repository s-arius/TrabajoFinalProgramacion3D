using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorLuces : MonoBehaviour
{
    public static ControladorLuces Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AplicarEstadoLuces();
    }

    public void ApagarLuces()
    {
        GameManagerGlobal.Instance.lucesApagadas = true;
        AplicarEstadoLuces();
    }

    public void EncenderLuces()
    {
        GameManagerGlobal.Instance.lucesApagadas = false;
        AplicarEstadoLuces();
    }

    private void AplicarEstadoLuces()
    {
        GameObject[] luces = GameObject.FindGameObjectsWithTag("LuzApagable");

        foreach (GameObject go in luces)
        {
            Light l = go.GetComponent<Light>();
            if (l != null)
                l.enabled = !GameManagerGlobal.Instance.lucesApagadas;
        }
    }
}
