using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Gestiona el inventario del jugador.
// Guarda qué objetos ha recogido y permite consultar si tiene alguno específico.
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instancia { get; private set; }

    //Evento que se activa cuando se modifica algo en el inventario.
    [HideInInspector] public UnityEvent cambioEnInventarioEvento;

    private List<scr_ItemData> items = new List<scr_ItemData>();

    void Awake()
    {
        // Patrón Singleton: solo puede haber un inventario
        if (Instancia == null)
        {
            Instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Inicializar el evento
        if (cambioEnInventarioEvento == null)
        {
            cambioEnInventarioEvento = new UnityEvent();
        }
    }


    // Añade un objeto al inventario
    public void agregarItem(scr_ItemData item)
    {
        if (!items.Contains(item))
        {
            items.Add(item);
            Debug.Log($"Objeto añadido: {item.nombre}");

            //activamos el unity event, pues se ha modificado la lista
            cambioEnInventarioEvento?.Invoke();
        }


    }

    // Comprueba si el jugador tiene el objeto
    public bool TieneElItem(scr_ItemData item)
    {
        return items.Contains(item);    //Devuelve el item que el objeto bloqueador actual necesita
    }

    // Elimina un objeto del inventario (si se consume al usarlo)
    public void EliminarItem(scr_ItemData item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            Debug.Log($"Objeto usado y eliminado de lista: {item.nombre}");

            //activamos el unity event, pues se ha modificado la lista
            cambioEnInventarioEvento?.Invoke();
        }

        
    }

    // Devuelve todos los objetos del inventario
    public List<scr_ItemData> ObtenerTodosLosItems()
    {
        return new List<scr_ItemData>(items);
    }
}