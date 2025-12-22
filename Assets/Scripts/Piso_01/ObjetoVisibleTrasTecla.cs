using UnityEngine;

public class ObjetoVisibleTrasTecla : MonoBehaviour
{
    [Header("Estado")]
    public bool estaVisible = false;

    void Start()
    {
        estaVisible = GameManagerGlobal.Instance.objetoVisible;
        gameObject.SetActive(estaVisible);
    }

    public void Activar()
    {
        if (estaVisible)
            return;

        estaVisible = true;
        gameObject.SetActive(true);

        GameManagerGlobal.Instance.objetoVisible = true;

        Debug.Log("Objeto activado tras colocar la tecla.");
    }
}
