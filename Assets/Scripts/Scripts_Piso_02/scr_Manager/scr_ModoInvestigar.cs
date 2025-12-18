using NUnit.Framework.Interfaces;
using TMPro;
using UnityEngine;
//using static UnityEditor.Progress;

// Controla el modo de investigación donde el jugador puede observar un objeto en 3D.
public class scr_ModoInvestigar : MonoBehaviour
{
    public static scr_ModoInvestigar Instancia { get; private set; }

    [Header("Configuración")]
    [SerializeField] private Transform PuntoInvestigacion_Prefab; // Punto delante de la cámara
    [SerializeField] private float velocidadRotacion = 1000f;

    [Header("Panel de Pensamiento")]
    [SerializeField] private GameObject Panel_Pensamiento; // Panel que contiene el pensamiento
    [SerializeField] private TextMeshProUGUI Texto_Pensamiento; // Texto del pensamiento


    private GameObject objetoActual;
    private bool estaInvestigando = false;

    void Awake()
    {
        if (Instancia == null)
        {
            Instancia = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Panel_Pensamiento.SetActive(false);
    }

    void Update()
    {
        if (!estaInvestigando) return;

        // Rotar el objeto con el ratón
        if (Input.GetMouseButton(0))
        {
            float rotX = Input.GetAxis("Mouse X") * velocidadRotacion * Time.deltaTime;
            float rotY = Input.GetAxis("Mouse Y") * velocidadRotacion * Time.deltaTime;

            objetoActual.transform.Rotate(Vector3.up, -rotX, Space.World);
            objetoActual.transform.Rotate(Vector3.right, rotY, Space.World);
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

        estaInvestigando = true;

        // Crear el objeto delante de la cámara
        objetoActual = Instantiate(item.investigationPrefab, PuntoInvestigacion_Prefab.position, Quaternion.identity);
        objetoActual.transform.SetParent(PuntoInvestigacion_Prefab);

        // Desactivar el movimiento del jugador
        var playerController = Object.FindFirstObjectByType<scr_PlayerMovimiento>(); 
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        //Activar panel y actualizar texto 
        if (Panel_Pensamiento != null && Panel_Pensamiento != null && !string.IsNullOrEmpty(item.pensamiento))
        {
            Texto_Pensamiento.text = item.pensamiento;
            Panel_Pensamiento.SetActive(true);
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
        if (!estaInvestigando) return;

        estaInvestigando = false;

        // Destruir el objeto
        if (objetoActual != null)
        {
            Destroy(objetoActual);
        }

        // Reactivar el movimiento del jugador
        var playerController = Object.FindFirstObjectByType<scr_PlayerMovimiento>();
        if (playerController != null)
        {
            playerController.enabled = true;
        }

        // Ocultar el panel de pensamiento
        if (Texto_Pensamiento != null)
        {
            Panel_Pensamiento.SetActive(false);
        }

        // Restaurar el cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log("Modo investigación desactivado");
    }

    public bool EstaInvestigando_Funcion() => estaInvestigando;
}