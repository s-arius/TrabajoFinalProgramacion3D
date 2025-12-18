using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTriggerLoader : MonoBehaviour
{
    public string sceneName;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Algo ha entrado en el trigger: " + other.name);

        if (!other.CompareTag("Player"))
            return;

        Debug.Log("PLAYER detectado, cargando escena...");
        SceneManager.LoadScene(sceneName);
    }
}
