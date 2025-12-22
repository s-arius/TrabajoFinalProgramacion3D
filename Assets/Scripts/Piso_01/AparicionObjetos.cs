using UnityEngine;
using System.Collections;

public class AparicionProgresivaObjetos : MonoBehaviour
{
    [Header("Objetos a mostrar (4)")]
    public GameObject[] objetos;

    [Header("Tiempo antes de desaparecer")]
    public float delayDesaparicion = 1f;

    private int indiceActual = 0;
    private bool enProceso = false;

    void Start()
    {
        OcultarTodos();
    }

    void Update()
    {
        if (!enProceso)
            DetectarNumero();
    }

    void DetectarNumero()
    {
        for (int i = 1; i <= 9; i++)
        {
            if (Input.GetKeyDown(i.ToString()) ||
                Input.GetKeyDown((KeyCode)((int)KeyCode.Keypad1 + i - 1)))
            {
                MostrarSiguiente();
                return;
            }
        }

        if (Input.GetKeyDown("0") || Input.GetKeyDown(KeyCode.Keypad0))
        {
            MostrarSiguiente();
        }
    }

    void MostrarSiguiente()
    {
        if (indiceActual < objetos.Length)
        {
            objetos[indiceActual].SetActive(true);
            indiceActual++;

            if (indiceActual == objetos.Length)
            {
                StartCoroutine(DesaparecerConDelay());
            }
        }
    }

    IEnumerator DesaparecerConDelay()
    {
        enProceso = true;

        yield return new WaitForSeconds(delayDesaparicion);

        OcultarTodos();
        indiceActual = 0;
        enProceso = false;
    }

    void OcultarTodos()
    {
        foreach (GameObject obj in objetos)
        {
            if (obj != null)
                obj.SetActive(false);
        }
    }
}
