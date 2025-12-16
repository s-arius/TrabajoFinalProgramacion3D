using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CrystalTarget : MonoBehaviour
{
    [Header("Material del cristal")]
    public Material crystalMaterial;

    [Header("Borrado")]
    public float brushSize = 0.1f;
    [Range(0.7f, 0.98f)]
    public float eraseThreshold = 0.9f; // % borrado necesario

    [Header("Destrucción")]
    public GameObject objectToDestroy; // 🔥 OBJETO A DESTRUIR

    RenderTexture eraseMask;
    Texture2D brush;
    Texture2D readbackTex;

    int eraseMaskID;
    float erasedAmount = 0f;
    bool destroyed = false;

    void Awake()
    {
        if (crystalMaterial == null)
        {
            Debug.LogWarning($"CrystalTarget en '{gameObject.name}' sin material.");
            enabled = false;
            return;
        }

        if (objectToDestroy == null)
            objectToDestroy = gameObject;

        eraseMaskID = Shader.PropertyToID("_EraseMask");

        eraseMask = new RenderTexture(1024, 1024, 0, RenderTextureFormat.R8);
        eraseMask.Create();

        RenderTexture.active = eraseMask;
        GL.Clear(true, true, Color.white);
        RenderTexture.active = null;

        crystalMaterial.SetTexture(eraseMaskID, eraseMask);

        CreateBrush();

        // textura para leer píxeles (MUY IMPORTANTE)
        readbackTex = new Texture2D(64, 64, TextureFormat.R8, false);
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

    public void Paint(Vector2 uv)
    {
        if (destroyed) return;

        uv.y = 1f - uv.y;

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

        // 🔍 Medir borrado
        MeasureErase();

        RenderTexture.active = null;
    }

    void MeasureErase()
    {
        // Leemos una región pequeña (optimizado)
        RenderTexture.active = eraseMask;
        readbackTex.ReadPixels(
            new Rect(0, 0, readbackTex.width, readbackTex.height),
            0, 0
        );
        readbackTex.Apply();

        int erasedPixels = 0;
        Color32[] pixels = readbackTex.GetPixels32();

        foreach (Color32 c in pixels)
        {
            if (c.r < 20) erasedPixels++; // casi negro
        }

        erasedAmount = erasedPixels / (float)pixels.Length;

        if (erasedAmount >= eraseThreshold)
            DestroyCrystal();
    }

    void DestroyCrystal()
    {
        if (destroyed) return;
        destroyed = true;

        Destroy(objectToDestroy);
    }

    void OnDestroy()
    {
        if (eraseMask != null)
            eraseMask.Release();
    }
}
