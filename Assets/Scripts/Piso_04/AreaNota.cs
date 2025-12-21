using UnityEngine;

public class AreaNota : MonoBehaviour
{
    public Maletin maletin;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && maletin != null)
        {
            maletin.EntrarAreaNota();
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && maletin != null)
        {
            maletin.SalirAreaNota();
        }
    }
}