using UnityEngine;

public class ObjetoVisibleTrasTecla : MonoBehaviour
{
    [Header("Estado")]
    public bool estaVisible = false;

    void Start()
    {
        // Al iniciar, el objeto NO está visible
        estaVisible = GameManagerGlobal.Instance.objetoVisible;
        gameObject.SetActive(estaVisible);
    }

    public void Activar()
    {
        if (estaVisible)
            return;

        estaVisible = true;
        gameObject.SetActive(true);

        // Informamos al GameManager
        GameManagerGlobal.Instance.objetoVisible = true;

        Debug.Log("Objeto activado tras colocar la tecla.");
    }
}
