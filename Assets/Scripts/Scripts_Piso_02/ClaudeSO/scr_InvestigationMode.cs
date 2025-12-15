using NUnit.Framework.Interfaces;
using UnityEngine;

/// <summary>
/// Controla el modo de investigación donde el jugador puede observar un objeto en 3D.
/// </summary>
public class InvestigationMode : MonoBehaviour
{
    public static InvestigationMode Instance { get; private set; }

    [Header("Configuración")]
    [SerializeField] private Transform investigationPoint; // Punto delante de la cámara
    [SerializeField] private float rotationSpeed = 100f;

    private GameObject currentObject;
    private bool isInvestigating = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (!isInvestigating) return;

        // Rotar el objeto con el ratón
        if (Input.GetMouseButton(0))
        {
            float rotX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            float rotY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

            currentObject.transform.Rotate(Vector3.up, -rotX, Space.World);
            currentObject.transform.Rotate(Vector3.right, rotY, Space.World);
        }

        // Salir del modo investigación
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitInvestigation();
        }
    }

    /// <summary>
    /// Entra en modo investigación mostrando el objeto
    /// </summary>
    public void StartInvestigation(scr_ItemData item)
    {
        if (item.investigationPrefab == null)
        {
            Debug.LogWarning("El objeto no tiene modelo de investigación");
            return;
        }

        isInvestigating = true;

        // Crear el objeto delante de la cámara
        currentObject = Instantiate(item.investigationPrefab, investigationPoint.position, Quaternion.identity);
        currentObject.transform.SetParent(investigationPoint);

        // Desactivar el movimiento del jugador
        var playerController = Object.FindFirstObjectByType<scr_PlayerMovimiento>(); 
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        // Bloquear el cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log($"Investigando: {item.itemName}");
    }

    /// <summary>
    /// Sale del modo investigación
    /// </summary>
    public void ExitInvestigation()
    {
        if (!isInvestigating) return;

        isInvestigating = false;

        // Destruir el objeto
        if (currentObject != null)
        {
            Destroy(currentObject);
        }

        // Reactivar el movimiento del jugador
        var playerController = Object.FindFirstObjectByType<scr_PlayerMovimiento>();
        if (playerController != null)
        {
            playerController.enabled = true;
        }

        // Restaurar el cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log("Modo investigación desactivado");
    }

    public bool IsInvestigating() => isInvestigating;
}