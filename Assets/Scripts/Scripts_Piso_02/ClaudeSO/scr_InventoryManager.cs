using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gestiona el inventario del jugador.
/// Guarda qué objetos ha recogido y permite consultar si tiene alguno específico.
/// </summary>
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    private List<scr_ItemData> items = new List<scr_ItemData>();

    void Awake()
    {
        // Patrón Singleton: solo puede haber un inventario
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Añade un objeto al inventario
    /// </summary>
    public void AddItem(scr_ItemData item)
    {
        if (!items.Contains(item))
        {
            items.Add(item);
            Debug.Log($"Objeto añadido: {item.itemName}");
        }
    }

    /// <summary>
    /// Comprueba si el jugador tiene un objeto específico
    /// </summary>
    public bool HasItem(scr_ItemData item)
    {
        return items.Contains(item);
    }

    /// <summary>
    /// Elimina un objeto del inventario (si se consume al usarlo)
    /// </summary>
    public void RemoveItem(scr_ItemData item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            Debug.Log($"Objeto usado: {item.itemName}");
        }
    }

    /// <summary>
    /// Devuelve todos los objetos del inventario
    /// </summary>
    public List<scr_ItemData> GetAllItems()
    {
        return new List<scr_ItemData>(items);
    }
}