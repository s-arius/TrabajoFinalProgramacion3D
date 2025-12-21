using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Estado del ascensor")]
    public int currentFloor = 100; // Piso inicial por defecto

    [Header("Estado del jugador")]
    public float playerY = 0f; // Posición Y del jugador

    [Header("Progreso especial")]
    public bool hasReachedFloor92 = false;

    [Header("Cristales limpiados")]
    public HashSet<string> cleanedCrystals = new HashSet<string>();

    // =========================
    // UI MOSTRADAS (NUEVO)
    // =========================
    [Header("UI mostradas")]
    public HashSet<string> shownUIs = new HashSet<string>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("[GameManager] Creado y persistente.");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // =========================
    // CRISTALES (SIN TOCAR)
    // =========================
    public void RegisterCleanedCrystal(string crystalID)
    {
        if (!cleanedCrystals.Contains(crystalID))
            cleanedCrystals.Add(crystalID);
    }

    public bool IsCrystalCleaned(string crystalID)
    {
        return cleanedCrystals.Contains(crystalID);
    }

    // =========================
    // UI MOSTRADAS (NUEVO)
    // =========================
    public void RegisterUIShown(string uiID)
    {
        if (!shownUIs.Contains(uiID))
            shownUIs.Add(uiID);
    }

    public bool WasUIShown(string uiID)
    {
        return shownUIs.Contains(uiID);
    }
}
