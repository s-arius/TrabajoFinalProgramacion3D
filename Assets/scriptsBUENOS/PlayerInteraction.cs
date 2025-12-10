using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionDistance = 3f; // distancia máxima de interacción
    public Camera playerCamera;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // tecla de interacción
        {
            InteractWithObject();
        }
    }

    void InteractWithObject()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                interactable.Interact();
                CrystalEraser eraser = hit.collider.GetComponent<CrystalEraser>();
                if (eraser != null)
                {
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;

                    eraser.enabled = true; // activar borrado
                }


                // 🔐 Bloquear movimiento del jugador
                PlayerMovement pm = GetComponent<PlayerMovement>();
                if (pm != null)
                {
                    pm.LockInput();
                }
            }
        }
    }
}
