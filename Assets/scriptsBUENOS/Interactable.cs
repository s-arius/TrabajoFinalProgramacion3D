using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool isInteracted = false;

    // Método que se llamará cuando el jugador interactúe
    public virtual void Interact()
    {
        if (!isInteracted)
        {
            isInteracted = true;
            Debug.Log("Has interactuado con el cristal!");

            // Ejemplo: cambiar color o destruir
            GetComponent<Renderer>().material.color = Color.blue;
            // Destroy(gameObject); // Descomenta si quieres que desaparezca
        }
    }
}
