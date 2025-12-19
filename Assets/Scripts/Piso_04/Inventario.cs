using UnityEngine;

public class Inventario : MonoBehaviour
{
    public bool hasKey = false;
    public bool hasTecla = false;
    
    void Start()
    {
    }
    
    public void AddItem(string itemName)
    {
        if (itemName.ToLower() == "llave")
        {
            hasKey = true;
            Debug.Log("tienes una llave");
        }
        else if (itemName.ToLower() == "tecla")
        {
            hasTecla = true;
            Debug.Log("has recogido la tecla para el keypad");
        }
    }
    
    // Método opcional para verificar si tienes un item
    public bool HasItem(string itemName)
    {
        if (itemName.ToLower() == "llave") return hasKey;
        if (itemName.ToLower() == "tecla") return hasTecla;
        return false;
    }
}