using UnityEngine;

public class Inventario : MonoBehaviour
{
    public bool tienesLlave = false;
    public bool tienesTecla = false;
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    
    public void AddItem(string itemName)
    {
        if (itemName.ToLower() == "llave")
        {
            tienesLlave = true;


            Debug.Log("tienes una llave");
        }
        else if (itemName.ToLower() == "tecla")
        {
            tienesTecla = true;
            Debug.Log("tienes una tecla");
        }
    }
    

}