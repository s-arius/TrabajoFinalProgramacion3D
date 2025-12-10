using UnityEngine;

public class CrystalEraser : MonoBehaviour
{
    public StaminaSystem staminaSystem;
    public Camera playerCamera;
    public Material crystalMaterial;
    public float brushSize = 0.1f;

    RenderTexture eraseMask;
    Texture2D brush;
    int eraseMaskID;

    CustomCursor cursor;
    bool isErasingCrystal = false;

    void Start()
    {
        cursor = FindObjectOfType<CustomCursor>();

        // Crear RenderTexture
        eraseMask = new RenderTexture(1024, 1024, 0, RenderTextureFormat.R8);
        eraseMask.Create();

        RenderTexture.active = eraseMask;
        GL.Clear(true, true, Color.white);
        RenderTexture.active = null;

        eraseMaskID = Shader.PropertyToID("_EraseMask");
        crystalMaterial.SetTexture(eraseMaskID, eraseMask);

        // Crear brocha circular
        brush = new Texture2D(64, 64, TextureFormat.RGBA32, false);

        for (int y = 0; y < brush.height; y++)
        {
            for (int x = 0; x < brush.width; x++)
            {
                float dx = (x - brush.width / 2f) / (brush.width / 2f);
                float dy = (y - brush.height / 2f) / (brush.height / 2f);
                float d = Mathf.Sqrt(dx * dx + dy * dy);

                brush.SetPixel(x, y, d <= 1 ? Color.black : Color.clear);
            }
        }

        brush.Apply();
    }

    void Update()
    {
        // Si no hay estamina → parar
        if (!staminaSystem.CanErase())
        {
            StopErasing();
            return;
        }

        // Clic inicial
        if (Input.GetMouseButtonDown(0))
        {
            TryStartErasing();
        }

        // Mantener clic → borrar
        if (isErasingCrystal && Input.GetMouseButton(0))
        {
            staminaSystem.isErasing = true;
            Paint();
        }

        // Soltar clic → parar
        if (Input.GetMouseButtonUp(0))
        {
            StopErasing();
        }

        // Si no se mantiene clic → no consumir
        if (!Input.GetMouseButton(0))
            staminaSystem.isErasing = false;
    }

    void TryStartErasing()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit hit))
        {
            StopErasing();
            return;
        }

        if (hit.collider.gameObject != gameObject)
        {
            StopErasing();
            return;
        }

        // Ahora sí estás sobre el cristal
        isErasingCrystal = true;
        staminaSystem.isErasing = true;
        cursor?.ActivateCursor();
    }

    void StopErasing()
    {
        isErasingCrystal = false;
        staminaSystem.isErasing = false;
        cursor?.DeactivateCursor();
    }

    void Paint()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit hit)) return;
        if (hit.collider.gameObject != gameObject) return;

        Vector2 uv = hit.textureCoord;
        uv.y = 1f - uv.y; // corregir inversión vertical

        RenderTexture.active = eraseMask;

        Graphics.DrawTexture(
            new Rect(
                uv.x * eraseMask.width - (brush.width * brushSize),
                uv.y * eraseMask.height - (brush.height * brushSize),
                brush.width * brushSize * 2f,
                brush.height * brushSize * 2f),
            brush
        );

        RenderTexture.active = null;
    }
}
