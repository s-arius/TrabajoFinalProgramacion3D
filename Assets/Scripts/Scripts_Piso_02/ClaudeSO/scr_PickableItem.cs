using NUnit.Framework.Interfaces;
using UnityEngine;

/// <summary>
/// Script para objetos que el jugador puede recoger.
/// Al interactuar, se añade al inventario y se activa el modo investigación.
/// </summary>
[RequireComponent(typeof(Collider))]
public class PickableItem : MonoBehaviour
{
    [Header("Datos del Objeto")]
    [SerializeField] private scr_ItemData scr_ItemData;

    [Header("Configuración")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private float interactionDistance = 3f;

    private Transform playerTransform;
    private bool isInRange = false;

    void Start()
    {
        // Buscar al jugador
        playerTransform = Camera.main.transform;

        // El trigger debe estar activado
        GetComponent<Collider>().isTrigger = true;
    }

    void Update()
    {
        if (!isInRange) return;

        // Comprobar distancia
        float distance = Vector3.Distance(transform.position, playerTransform.position);
        if (distance > interactionDistance)
        {
            isInRange = false;
            return;
        }

        // Recoger objeto
        if (Input.GetKeyDown(interactKey))
        {
            PickUp();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = true;
            Debug.Log($"Presiona {interactKey} para recoger: {scr_ItemData.itemName}");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = false;
        }
    }

   
    // Recoge el objeto y activa el modo investigación
    void PickUp()
    {
        // Añadir al inventario
        InventoryManager.Instance.AddItem(scr_ItemData);

        // Activar modo investigación
        InvestigationMode.Instance.StartInvestigation(scr_ItemData);

        // Destruir el objeto del mundo
        Destroy(gameObject);
    }

    // Visualizar el rango de interacción en el editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }
}