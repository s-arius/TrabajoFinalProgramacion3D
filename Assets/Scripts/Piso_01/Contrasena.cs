using UnityEngine;

public class Contrasena : MonoBehaviour
{
    [Header("Objetos a mostrar")]
    public GameObject[] objetos; // 4 objetos asignados en el inspector

    private int contador = 0;

    void Start()
    {
        // Asegurarnos de que todos los objetos estén apagados al inicio
        foreach (var obj in objetos)
        {
            if (obj != null)
                obj.SetActive(false);
        }
        contador = 0;
    }

    /// <summary>
    /// Llamar cada vez que se introduce un número
    /// </summary>
    /// <param name="numero">Número introducido (puede ser string o int)</param>
    public void RegistrarNumero(string numero)
    {
        if (objetos == null || objetos.Length == 0)
            return;

        Debug.Log($"Número introducido: {numero}");

        // Activar el objeto correspondiente
        if (contador < objetos.Length && objetos[contador] != null)
        {
            objetos[contador].SetActive(true);
            Debug.Log($"Objeto {contador} activado");
        }

        contador++;

        // Si ya se introdujeron 4 números, reiniciar
        if (contador >= objetos.Length)
        {
            ReiniciarObjetos();
        }
    }

    private void ReiniciarObjetos()
    {
        Debug.Log("Reiniciando objetos...");
        foreach (var obj in objetos)
        {
            if (obj != null)
                obj.SetActive(false);
        }
        contador = 0;
    }
}
