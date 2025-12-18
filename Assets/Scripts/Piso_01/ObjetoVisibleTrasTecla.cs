using UnityEngine;

public class ObjetoVisibleTrasTecla : MonoBehaviour
{
    void Start()
    {
        // Desactivado al inicio
        gameObject.SetActive(false);
    }

    public void Activar()
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
            Debug.Log("Objeto activado tras colocar la tecla.");
        }
    }
}
