using TMPro;
using UnityEngine;
using System.Collections;

public class scr_MensajeManager : MonoBehaviour
{
    public static scr_MensajeManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private GameObject messagePanel; // El panel contenedor
    [SerializeField] private TextMeshProUGUI messageText; // El texto dentro del panel
    [SerializeField] private float defaultDuration = 2f;

    private Coroutine currentMessageCoroutine;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Ocultar el panel al inicio
        if (messagePanel != null)
        {
            messagePanel.SetActive(false);
        }
    }

    /// <summary>
    /// Muestra un mensaje temporal en pantalla
    /// </summary>
    public void MostrarMensaje(string message, float duration = -1f) // Este de activa y modifica dependiendo de lo que le llegue del script scr_ObjetoBloqueador 
    {
        if (messagePanel == null || messageText == null) return;

        // Usar duración por defecto si no se especifica
        if (duration < 0)
        {
            duration = defaultDuration;
        }

        // Cancelar mensaje anterior si existe
        if (currentMessageCoroutine != null)
        {
            StopCoroutine(currentMessageCoroutine);
        }

        // Actualizar texto y mostrar panel
        messageText.text = message;
        messagePanel.SetActive(true);

        currentMessageCoroutine = StartCoroutine(HideMessageAfterDelay(duration));
    }

    /// <summary>
    /// Oculta el mensaje inmediatamente
    /// </summary>
    public void HideMessage()
    {
        if (currentMessageCoroutine != null)
        {
            StopCoroutine(currentMessageCoroutine);
            currentMessageCoroutine = null;
        }

        if (messagePanel != null)
        {
            messagePanel.SetActive(false);
        }
    }

    private IEnumerator HideMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (messagePanel != null)
        {
            messagePanel.SetActive(false);
        }

        currentMessageCoroutine = null;
    }
}