using UnityEngine;

public class CrystalEraserManager : MonoBehaviour
{
    [Header("Referencias")]
    public StaminaSystem staminaSystem;
    public Camera playerCamera;

    CustomCursor cursor;

    bool isMouseDown = false;
    bool cursorActive = false;

    void Start()
    {
        cursor = FindObjectOfType<CustomCursor>();
        if (playerCamera == null && Camera.main != null)
            playerCamera = Camera.main;
    }

    void Update()
    {
        if (staminaSystem == null || playerCamera == null)
        {
            ForceStop();
            return;
        }

        // 🔴 Sin stamina → parar todo
        if (!staminaSystem.CanErase())
        {
            ForceStop();
            return;
        }

        // Input
        if (Input.GetMouseButtonDown(0))
            isMouseDown = true;

        if (Input.GetMouseButtonUp(0))
        {
            isMouseDown = false;
            ForceStop();
            return;
        }

        if (!isMouseDown)
        {
            staminaSystem.isErasing = false;
            return;
        }

        // 🔥 Ray SIEMPRE desde el centro de la cámara
        Ray ray = CreateRayFromCenter();

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            CrystalTarget target = hit.collider.GetComponent<CrystalTarget>();

            if (target != null)
            {
                // Activar cursor solo una vez
                if (!cursorActive)
                {
                    cursor?.ActivateCursor();
                    cursorActive = true;
                }

                staminaSystem.isErasing = true;

                // 🔥 Pintar (ya no devuelve bool)
                target.Paint(hit.textureCoord);
                return;
            }
        }

        // Si no estamos apuntando a un cristal válido
        ForceStop();
    }

    Ray CreateRayFromCenter()
    {
        Vector2 center = new Vector2(Screen.width / 2f, Screen.height / 2f);
        return playerCamera.ScreenPointToRay(center);
    }

    void ForceStop()
    {
        isMouseDown = false;
        staminaSystem.isErasing = false;

        if (cursorActive)
        {
            cursor?.DeactivateCursor();
            cursorActive = false;
        }
    }
}
