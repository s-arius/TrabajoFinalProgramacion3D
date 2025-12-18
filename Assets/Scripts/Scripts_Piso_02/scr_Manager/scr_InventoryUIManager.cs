using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Gestiona la interfaz visual del inventario.
// Muestra los iconos de los objetos que el jugador tiene en su inventario.
// Usa slots fijos que se muestran/ocultan segun necesidad.
public class scr_InventoryUIManager : MonoBehaviour
{

    // CONFIGURACION
    [Header("Slots del Inventario")]
    [Tooltip("Arrastra aquí los 5 slots (Images) en orden")]
    [SerializeField] private List<Image> slots = new List<Image>();


    void Start()
    {
        // Validar que tenemos slots configurados
        if (slots.Count == 0)
        {
            Debug.LogWarning(" InventoryUI: No hay slots asignados. Arrastra las imágenes en el Inspector.");
            return;
        }

        // Suscribirse al evento de cambio de inventario
        if (InventoryManager.Instancia != null)
        {
            InventoryManager.Instancia.cambioEnInventarioEvento.AddListener(UpdateInventory); //Cuanto ocurra este evento, avisame y ejecuta la funcion UpdateInventory; onInventoryChanged?.Invoke() -> UpdateInventory ( o Cualquiera que este suscrito)
        }
        else
        {
            Debug.LogError("InventoryUI: No se encuentra el InventoryManager en la escena.");
        }

        // Actualizar el inventario al inicio
        UpdateInventory();
    }

    void OnDestroy()
    {
        // Desuscribirse del evento al destruir
        if (InventoryManager.Instancia != null)
        {
            InventoryManager.Instancia.cambioEnInventarioEvento.RemoveListener(UpdateInventory); //Deja de avisarme cuando ocurra este evento. 
        }
    }

    // MÉTODO PRINCIPAL: ACTUALIZAR INVENTARIO
    // Actualiza la visualización del inventario
    // Se llama automáticamente cuando el inventario cambia; onInventoryChanged?.Invoke()
    public void UpdateInventory()
    {
        // Obtener los objetos del inventario
        List<scr_ItemData> items = InventoryManager.Instancia.ObtenerTodosLosItems();

        Debug.Log($"Inventario actualizado: {items.Count} objetos");

        // DEBUG: Mostrar qué objetos tenemos
        for (int i = 0; i < items.Count; i++)
        {
            Debug.Log($" Slot {i}: {items[i].nombre}");
        }

        //Siempre que se ejecute este codigo ocultara todos los slots y luego mostrara solo los que esten activos en la lista de Items indexada en esta funcion
        //Desactivamos todos los slots por precaucion

        // 1) Ocultar TODOS los slots
        for (int i = 0; i < slots.Count; i++)
        {
            OcultarSlot(i);
        }

        // 2) Mostrar solo los slots que tienen objetos
        for (int i = 0; i < items.Count && i < slots.Count; i++)
        {
            MostrarSlot(i, items[i]);
        }
    }



    // Muestra un slot con el icono del objeto
    void MostrarSlot(int index, scr_ItemData item)
    {
        Image slot = slots[index];

        // Asignar el sprite del objeto
        if (item.icono != null)
        {
            slot.sprite = item.icono;
        }
        else
        {
            Debug.LogWarning($"El objeto '{item.nombre}' no tiene icono asignado en el SO, pongale uno.");
           
        }

        // Asegurarse de que el color es visible
        Color color = slot.color;
        color.a = 1f;
        slot.color = color;

        // Activar el slot
        slot.gameObject.SetActive(true);

        Debug.Log($"Slot {index} activado con {item.nombre}");
    }

    // Oculta un slot vacío (Luego esto se llama en un bucle for, por lo que no termina siendo solo uno, pero bueno)
    void OcultarSlot(int index)
    {
        Image slot = slots[index];
        slot.gameObject.SetActive(false);

        Debug.Log($"Slot {index} desactivado");
    }
    


    //FUNCIONES ADICIONALES: Adicionales porque estan en desuso
    //No las borro porque podrian servir en un futuro


    // Fuerza una actualización manual del inventario
    // (útil para debugging o casos especiales) //No está en uso ahora, pero podría necesitarse
    public void ForceUpdate()
    {
        UpdateInventory();
    }

    // Obtiene la cantidad de slots visibles actualmente
    public int ObtenlosSlotsVisibles() //Actualmente nadie lo usa
    {
        int count = 0;
        foreach (Image slot in slots)
        {
            if (slot.gameObject.activeSelf)
            {
                count++;
            }
        }
        return count;
    }
}