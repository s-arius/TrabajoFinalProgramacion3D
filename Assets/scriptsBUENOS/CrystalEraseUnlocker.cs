using UnityEngine;
using System.Collections.Generic;

public class CrystalEraseUnlocker : MonoBehaviour
{
    [Header("Referencias")]
    public StaminaSystem staminaSystem;
    public Camera playerCamera;
    public Material crystalMaterial;
    public float brushSize = 0.1f;

    [Header("Ascensor")]
    public ElevatorController elevator;
    [Range(0.9f, 1f)]
    public float eraseThreshold = 0.99f;

    [Header("Sonidos")]
    public AudioClip unlockClip;
    public AudioClip eraseClip;
    public float eraseVolume = 0.6f;
    public float unlockVolume = 1f;

    [Header("Efectos opcionales")]
    public ParticleSystem eraseParticles;
    [Range(0f, 1f)] public float eraseStrength = 1f;

    [Header("Objetos a destruir al completar borrado")]
    public List<GameObject> objectsToDestroy = new List<GameObject>();

    [Header("Identificador único del cristal")]
    public string objectID;

    private RenderTexture eraseMask;
    private Texture2D brush;
    private Texture2D readbackTex;
    private int eraseMaskID;
    private bool isErasing = false;
    private bool unlocked = false;
    private CustomCursor cursor;
    private AudioSource audioSource;

    void Start()
    {
        cursor = FindObjectOfType<CustomCursor>();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        // 🧠 SI YA ESTÁ LIMPIO SEGÚN EL GAMEMANAGER → NO APARECE
        if (!string.IsNullOrEmpty(objectID) &&
            GameManager.Instance != null &&
            GameManager.Instance.IsCrystalCleaned(objectID))
        {
            Debug.Log($"🧼 Cristal ya limpio al cargar escena: {objectID}");
            DestroyObjectsInstant();
            return;
        }

        // Crear máscara de borrado
        eraseMask = new RenderTexture(1024, 1024, 0, RenderTextureFormat.R8);
        eraseMask.Create();
        RenderTexture.active = eraseMask;
        GL.Clear(true, true, Color.white);
        RenderTexture.active = null;

        eraseMaskID = Shader.PropertyToID("_EraseMask");
        crystalMaterial.SetTexture(eraseMaskID, eraseMask);

        // Crear pincel
        brush = new Texture2D(64, 64, TextureFormat.RGBA32, false);
        for (int y = 0; y < brush.height; y++)
        {
            for (int x = 0; x < brush.width; x++)
            {
                float dx = (x - brush.width / 2f) / (brush.width / 2f);
                float dy = (y - brush.height / 2f) / (brush.height / 2f);
                float d = Mathf.Sqrt(dx * dx + dy * dy);
                brush.SetPixel(x, y, d <= 1f ? new Color(0, 0, 0, eraseStrength) : Color.clear);
            }
        }
        brush.Apply();

        readbackTex = new Texture2D(128, 128, TextureFormat.R8, false);
    }

    void Update()
    {
        if (!staminaSystem.CanErase() || unlocked)
        {
            StopErasing();
            return;
        }

        if (Input.GetMouseButtonDown(0))
            TryStartErasing();

        if (isErasing && Input.GetMouseButton(0))
        {
            staminaSystem.isErasing = true;
            Paint();
            PlayEraseEffects();
        }

        if (Input.GetMouseButtonUp(0))
            StopErasing();

        if (!Input.GetMouseButton(0))
            staminaSystem.isErasing = false;
    }

    Ray CreateRayFromCenter()
    {
        return playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
    }

    void TryStartErasing()
    {
        Ray ray = CreateRayFromCenter();
        if (!Physics.Raycast(ray, out RaycastHit hit) || hit.collider.gameObject != gameObject)
        {
            StopErasing();
            return;
        }

        isErasing = true;
        staminaSystem.isErasing = true;
        cursor?.ActivateCursor();
    }

    void StopErasing()
    {
        isErasing = false;
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

        CheckIfUnlocked();
    }

    void CheckIfUnlocked()
    {
        if (unlocked) return;

        RenderTexture.active = eraseMask;
        readbackTex.ReadPixels(new Rect(0, 0, readbackTex.width, readbackTex.height), 0, 0);
        readbackTex.Apply();
        RenderTexture.active = null;

        Color32[] pixels = readbackTex.GetPixels32();
        int erased = 0;

        foreach (Color32 c in pixels)
        {
            if (c.r < 20)
                erased++;
        }

        float erasedPercent = erased / (float)pixels.Length;

        if (erasedPercent >= eraseThreshold)
            DestroyObjects();
    }

    void DestroyObjects()
    {
        if (unlocked) return;
        unlocked = true;

        // 🧠 GUARDAR CRISTAL COMO LIMPIO EN EL GAMEMANAGER
        if (!string.IsNullOrEmpty(objectID) && GameManager.Instance != null)
            GameManager.Instance.RegisterCleanedCrystal(objectID);

        cursor?.DeactivateCursor();
        StopEffects();

        foreach (GameObject obj in objectsToDestroy)
        {
            if (obj != null)
                Destroy(obj);
        }

        if (elevator != null)
            elevator.UnlockCurrentFloor();

        if (unlockClip != null)
            audioSource.PlayOneShot(unlockClip, unlockVolume);

        Debug.Log($"🔓 Cristal limpiado y guardado: {objectID}");
    }

    void DestroyObjectsInstant()
    {
        unlocked = true;

        foreach (GameObject obj in objectsToDestroy)
        {
            if (obj != null)
                Destroy(obj);
        }

        Destroy(gameObject);
    }

    void PlayEraseEffects()
    {
        if (eraseParticles != null && !eraseParticles.isPlaying)
        {
            eraseParticles.transform.position = GetErasePoint();
            eraseParticles.Play();
        }

        if (eraseClip != null && !audioSource.isPlaying)
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
