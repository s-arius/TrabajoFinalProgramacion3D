using UnityEngine;
using System.Collections;

public class AparicionProgresivaObjetos : MonoBehaviour
{
    [Header("Objetos a mostrar (4)")]
    public GameObject[] objetos; // Deben ser 4

    [Header("Tiempo antes de desaparecer")]
    public float delayDesaparicion = 1f;

    private int indiceActual = 0;
    private bool enProceso = false; // Evita pulsaciones durante el delay

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
        for (int i = 0; i <= 9; i++)
        {
            if (Input.GetKeyDown(i.ToString()) ||
                (i > 0 && Input.GetKeyDown((KeyCode)((int)KeyCode.Keypad0 + i))))
            {
                MostrarSiguiente();
                break;
            }
        }
    }

    void MostrarSiguiente()
    {
        if (indiceActual < objetos.Length)
        {
            objetos[indiceActual].SetActive(true);
            indiceActual++;

            // Si acabamos de mostrar el último
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
