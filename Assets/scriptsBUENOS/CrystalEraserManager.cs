using UnityEngine;

public class CrystalEraserManager : MonoBehaviour
{
    public StaminaSystem staminaSystem;
    public Camera playerCamera;

    CustomCursor cursor;
    bool isMouseDown = false;
    bool cursorActive = false;

    void Start()
    {
        cursor = FindObjectOfType<CustomCursor>();
        if (playerCamera == null && Camera.main != null) playerCamera = Camera.main;
    }

    void Update()
    {
        if (!staminaSystem || playerCamera == null)
        {
            // si falta algo, asegurar estado limpio
            cursor?.DeactivateCursor();
            return;
        }

        // si no hay estamina → apagar todo
        if (!staminaSystem.CanErase())
        {
            isMouseDown = false;
            if (cursorActive) { cursor?.DeactivateCursor(); cursorActive = false; }
            staminaSystem.isErasing = false;
            return;
        }

        // entrada del ratón
        if (Input.GetMouseButtonDown(0)) isMouseDown = true;
        if (Input.GetMouseButtonUp(0))
        {
            isMouseDown = false;
            if (cursorActive) { cursor?.DeactivateCursor(); cursorActive = false; }
            staminaSystem.isErasing = false;
        }

        if (!isMouseDown)
        {
            // no manteniendo click: no consumir
            staminaSystem.isErasing = false;
            return;
        }

        // si mantenemos el botón, comprobar qué tocamos
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            CrystalTarget target = hit.collider.GetComponent<CrystalTarget>();
            if (target != null)
            {
                // golpe a cristal válido → activar cursor si no activo
                if (!cursorActive)
                {
                    cursor?.ActivateCursor();
                    cursorActive = true;
                }

                staminaSystem.isErasing = true;

                // pintar; Paint devuelve true si pintó
                bool painted = target.Paint(hit.textureCoord);
                // si por algún motivo no pintó, podríamos desactivar cursor, pero lo mantenemos mientras golpee el target
                return;
            }
        }

        // si llegamos aquí: no golpeamos un CrystalTarget válido
        if (cursorActive)
        {
            cursor?.DeactivateCursor();
            cursorActive = false;
        }
        staminaSystem.isErasing = false;
    }
}
