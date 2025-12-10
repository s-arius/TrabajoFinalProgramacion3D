using UnityEngine;
using UnityEngine.UI;

public class CustomCursor : MonoBehaviour
{
    public Image spongeCursor; // la imagen del cursor
    bool active = false;

    void Start()
    {
        if (spongeCursor != null)
            spongeCursor.gameObject.SetActive(false);

        Cursor.visible = true; // cursor normal visible por defecto
    }

    void Update()
    {
        if (!active || spongeCursor == null) return;

        // Seguir el ratón
        spongeCursor.rectTransform.position = Input.mousePosition;
    }

    public void ActivateCursor()
    {
        if (spongeCursor == null) return;

        active = true;
        Cursor.visible = false;
        spongeCursor.gameObject.SetActive(true);
    }

    public void DeactivateCursor()
    {
        if (spongeCursor == null) return;

        active = false;
        Cursor.visible = true;
        spongeCursor.gameObject.SetActive(false);
    }
}
