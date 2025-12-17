using UnityEngine;

public class Llave : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
       
            Inventario inventario = other.GetComponent<Inventario>();
            
            if (inventario != null)
            {
                inventario.AddItem("llave");
                Destroy(gameObject);
            }
            else
            {
            }
        }
    }
}