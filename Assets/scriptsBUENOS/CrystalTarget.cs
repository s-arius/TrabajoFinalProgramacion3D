using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CrystalTarget : MonoBehaviour
{
    [Header("Material del cristal (asignar el material que usa el renderer)")]
    public Material crystalMaterial;    // asigna aquí Material1 o Material2 (la instancia que usa este cristal)
    public float brushSize = 0.1f;

    RenderTexture eraseMask;
    Texture2D brush;
    int eraseMaskID;

    void Awake()
    {
        // seguridad
        if (crystalMaterial == null)
        {
            Debug.LogWarning($"CrystalTarget en '{gameObject.name}' no tiene crystalMaterial asignado.");
            return;
        }

        eraseMaskID = Shader.PropertyToID("_EraseMask");

        // Crear RenderTexture única por cada cristal
        eraseMask = new RenderTexture(1024, 1024, 0, RenderTextureFormat.R8);
        eraseMask.Create();

        // Inicializar en blanco (visible)
        RenderTexture.active = eraseMask;
        GL.Clear(true, true, Color.white);
        RenderTexture.active = null;

        // Asignar la máscara al material (asegúrate de que el material es la instancia que usa este renderer)
        crystalMaterial.SetTexture(eraseMaskID, eraseMask);

        CreateBrush();
    }

    void CreateBrush()
    {
        brush = new Texture2D(64, 64, TextureFormat.RGBA32, false);
        for (int y = 0; y < brush.height; y++)
        {
            for (int x = 0; x < brush.width; x++)
            {
                float dx = (x - brush.width / 2f) / (brush.width / 2f);
                float dy = (y - brush.height / 2f) / (brush.height / 2f);
                float d = Mathf.Sqrt(dx * dx + dy * dy);
                brush.SetPixel(x, y, d <= 1f ? Color.black : Color.clear);
            }
        }
        brush.Apply();
    }

    // Pintar en este cristal; uv viene de RaycastHit.textureCoord
    public bool Paint(Vector2 uv)
    {
        if (eraseMask == null || brush == null) return false;

        uv.y = 1f - uv.y; // corregir Y

        RenderTexture.active = eraseMask;

        Graphics.DrawTexture(
            new Rect(
                uv.x * eraseMask.width - (brush.width * brushSize),
                uv.y * eraseMask.height - (brush.height * brushSize),
                brush.width * brushSize * 2f,
                brush.height * brushSize * 2f
            ),
            brush
        );

        RenderTexture.active = null;
        return true;
    }

    void OnDestroy()
    {
        if (eraseMask != null)
            eraseMask.Release();
    }
}
