using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioEscena : MonoBehaviour
{
    [Header("Nombres de escenas")]
    public string escenaI = "Piso_01";
    public string escenaO = "Piso_04";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            SceneManager.LoadScene(escenaI);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            SceneManager.LoadScene(escenaO);
        }
    }
}
