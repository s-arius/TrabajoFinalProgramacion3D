using NUnit.Framework.Interfaces;
using TMPro;
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

    [Header("Panel de Pensamiento")]
    [SerializeField] private GameObject thoughtPanel; // Panel que contiene el pensamiento
    [SerializeField] private TextMeshProUGUI thoughtText; // Texto del pensamiento


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

        thoughtPanel.SetActive(false);
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

   
    // Entra en modo investigación mostrando el objeto
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

        //Activar panel y actualizar texto 
        if (thoughtPanel != null && thoughtText != null && !string.IsNullOrEmpty(item.pensamiento))
        {
            thoughtText.text = item.pensamiento;
            thoughtPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Se te olvido referenciar algo man, WT????");
        }

        // Bloquear el cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log($"Investigando: {item.nombre}");
    }

    // Sale del modo investigación
    
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

        // Ocultar el panel de pensamiento
        if (thoughtPanel != null)
        {
            thoughtPanel.SetActive(false);
        }

        // Restaurar el cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log("Modo investigación desactivado");
    }

    public bool IsInvestigating() => isInvestigating;
}