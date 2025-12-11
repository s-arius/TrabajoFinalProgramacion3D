using UnityEngine;

public class CrystalEraser : MonoBehaviour
{
    [Header("Referencias")]
    public StaminaSystem staminaSystem;
    public Camera playerCamera;
    public Material crystalMaterial;
    public float brushSize = 0.1f;

    [Header("Efectos")]
    public ParticleSystem eraseParticles; // Partículas al borrar
    public AudioSource eraseSound;        // Sonido al borrar
    [Range(0f, 1f)] public float eraseStrength = 1f; // Opacidad de borrado

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

        // Crear brocha circular con opacidad inicial
        brush = new Texture2D(64, 64, TextureFormat.RGBA32, false);
        for (int y = 0; y < brush.height; y++)
        {
            for (int x = 0; x < brush.width; x++)
            {
                float dx = (x - brush.width / 2f) / (brush.width / 2f);
                float dy = (y - brush.height / 2f) / (brush.height / 2f);
                float d = Mathf.Sqrt(dx * dx + dy * dy);

                brush.SetPixel(x, y, d <= 1 ? new Color(0, 0, 0, eraseStrength) : Color.clear);
            }
        }
        brush.Apply();
    }

    void Update()
    {
        if (!staminaSystem.CanErase())
        {
            StopErasing();
            return;
        }

        if (Input.GetMouseButtonDown(0))
            TryStartErasing();

        if (isErasingCrystal && Input.GetMouseButton(0))
        {
            staminaSystem.isErasing = true;
            Paint();
            PlayEffects();
        }

        if (Input.GetMouseButtonUp(0))
            StopErasing();

        if (!Input.GetMouseButton(0))
            staminaSystem.isErasing = false;
    }

    // 🔹 Raycast desde el centro de la pantalla
    Ray CreateRayFromCenter()
    {
        Vector2 center = new Vector2(Screen.width / 2f, Screen.height / 2f);
        return playerCamera.ScreenPointToRay(center);
    }

    void TryStartErasing()
    {
        Ray ray = CreateRayFromCenter();
        if (!Physics.Raycast(ray, out RaycastHit hit) || hit.collider.gameObject != gameObject)
        {
            StopErasing();
            return;
        }

        isErasingCrystal = true;
        staminaSystem.isErasing = true;
        cursor?.ActivateCursor();
    }

    void StopErasing()
    {
        isErasingCrystal = false;
        staminaSystem.isErasing = false;
        cursor?.DeactivateCursor();
        StopEffects();
    }

    void Paint()
    {
        Ray ray = CreateRayFromCenter();
        if (!Physics.Raycast(ray, out RaycastHit hit) || hit.collider.gameObject != gameObject) return;

        Vector2 uv = hit.textureCoord;
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
        RenderTexture.active = null;
    }

    void PlayEffects()
    {
        // Partículas en el punto de borrado
        if (eraseParticles != null && !eraseParticles.isPlaying)
        {
            eraseParticles.transform.position = GetErasePoint();
            eraseParticles.Play();
        }

        // Sonido
        if (eraseSound != null && !eraseSound.isPlaying)
        {
            eraseSound.Play();
        }
    }

    void StopEffects()
    {
        if (eraseParticles != null && eraseParticles.isPlaying)
            eraseParticles.Stop();

        if (eraseSound != null && eraseSound.isPlaying)
            eraseSound.Stop();
    }

    // Obtener punto de borrado en mundo (para partículas)
    Vector3 GetErasePoint()
    {
        Ray ray = CreateRayFromCenter();
        if (Physics.Raycast(ray, out RaycastHit hit))
            return hit.point;
        return playerCamera.transform.position + playerCamera.transform.forward * 2f;
    }
}
