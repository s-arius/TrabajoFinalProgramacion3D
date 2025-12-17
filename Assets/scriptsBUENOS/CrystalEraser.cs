using UnityEngine;

public class CrystalEraser : MonoBehaviour
{
    [Header("Referencias")]
    public StaminaSystem staminaSystem;
    public Camera playerCamera;
    public Material crystalMaterial;
    public float brushSize = 0.1f;

    [Header("Ascensor")]
    public ElevatorController elevator;

    [Header("Destrucción de objetos externos")]
    public GameObject[] objectsToDestroy;
    [Range(0.0f, 1f)]
    public float eraseThreshold = 0.99f;

    [Header("Efectos")]
    public ParticleSystem eraseParticles;

    [Header("Sonido de borrado")]
    public AudioClip eraseClip;
    public float eraseVolume = 1f;

    [Range(0f, 1f)] public float eraseStrength = 1f;

    private AudioSource audioSource;

    RenderTexture eraseMask;
    Texture2D brush;
    Texture2D readbackTex;

    int eraseMaskID;
    bool isErasingCrystal = false;
    bool destroyed = false;

    CustomCursor cursor;

    void Start()
    {
        cursor = FindObjectOfType<CustomCursor>();

        // 🎧 AudioSource interno
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;

        if (objectsToDestroy == null)
            objectsToDestroy = new GameObject[0];

        // RenderTexture
        eraseMask = new RenderTexture(1024, 1024, 0, RenderTextureFormat.R8);
        eraseMask.Create();

        RenderTexture.active = eraseMask;
        GL.Clear(true, true, Color.white);
        RenderTexture.active = null;

        eraseMaskID = Shader.PropertyToID("_EraseMask");
        crystalMaterial.SetTexture(eraseMaskID, eraseMask);

        // Brocha
        brush = new Texture2D(64, 64, TextureFormat.RGBA32, false);
        for (int y = 0; y < brush.height; y++)
        {
            for (int x = 0; x < brush.width; x++)
            {
                float dx = (x - brush.width / 2f) / (brush.width / 2f);
                float dy = (y - brush.height / 2f) / (brush.height / 2f);
                float d = Mathf.Sqrt(dx * dx + dy * dy);

                brush.SetPixel(
                    x,
                    y,
                    d <= 1f ? new Color(0, 0, 0, eraseStrength) : Color.clear
                );
            }
        }
        brush.Apply();

        readbackTex = new Texture2D(128, 128, TextureFormat.R8, false);
    }

    void Update()
    {
        if (destroyed) return;

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
        if (!Physics.Raycast(ray, out RaycastHit hit) || hit.collider.gameObject != gameObject)
            return;

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

        CheckIfFullyErased();
    }

    void CheckIfFullyErased()
    {
        if (destroyed || objectsToDestroy.Length == 0) return;

        RenderTexture.active = eraseMask;
        readbackTex.ReadPixels(
            new Rect(0, 0, readbackTex.width, readbackTex.height),
            0, 0
        );
        readbackTex.Apply();
        RenderTexture.active = null;

        Color32[] pixels = readbackTex.GetPixels32();
        int erased = 0;

        foreach (Color32 c in pixels)
        {
            if (c.r < 20) erased++;
        }

        float erasedPercent = erased / (float)pixels.Length;

        if (erasedPercent >= eraseThreshold)
            DestroyObjects();
    }

    void DestroyObjects()
    {
        if (destroyed) return;
        destroyed = true;

        StopEffects();
        cursor?.DeactivateCursor();

        foreach (GameObject obj in objectsToDestroy)
        {
            if (obj != null)
                Destroy(obj);
        }

        // 🔑 desbloquear ascensor
        if (elevator != null)
            elevator.UnlockCurrentFloor();
    }

    void PlayEffects()
    {
        if (eraseParticles != null && !eraseParticles.isPlaying)
        {
            eraseParticles.transform.position = GetErasePoint();
            eraseParticles.Play();
        }

        if (eraseClip != null)
            audioSource.PlayOneShot(eraseClip, eraseVolume);
    }

    void StopEffects()
    {
        if (eraseParticles != null && eraseParticles.isPlaying)
            eraseParticles.Stop();
    }

    Vector3 GetErasePoint()
    {
        Ray ray = CreateRayFromCenter();
        if (Physics.Raycast(ray, out RaycastHit hit))
            return hit.point;

        return playerCamera.transform.position + playerCamera.transform.forward * 2f;
    }

    void OnDestroy()
    {
        if (eraseMask != null)
            eraseMask.Release();
    }
}
