using UnityEngine;
using System.Collections;

public class UIDialogos : MonoBehaviour
{
    [Header("Configuración")]
    public string uiID;
    public GameObject uiObject;
    public float displayTime = 2f;

    private bool isShowing = false;

    void Awake()
    {
        // APAGAR LA UI SIEMPRE, ANTES DE START
        if (uiObject != null)
            uiObject.SetActive(false);
    }

    void Start()
    {
        // Si ya se mostró, eliminamos el trigger
        if (GameManager.Instance.WasUIShown(uiID))
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isShowing) return;
        if (!other.CompareTag("Player")) return;
        if (GameManager.Instance.WasUIShown(uiID)) return;

        StartCoroutine(ShowUIRoutine());
    }

    private IEnumerator ShowUIRoutine()
    {
        isShowing = true;

        uiObject.SetActive(true);

        yield return new WaitForSeconds(displayTime);

        uiObject.SetActive(false);

        GameManager.Instance.RegisterUIShown(uiID);

        // El trigger no vuelve a existir
        gameObject.SetActive(false);
    }
}
