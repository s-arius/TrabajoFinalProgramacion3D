using NUnit.Framework.Interfaces;
using UnityEngine;


// Script para objetos que el jugador puede recoger.
// Al interactuar, se añade al inventario y se activa el modo investigación.
[RequireComponent(typeof(Collider))]
public class ItemDesbloqueador : MonoBehaviour
{
    [Header("Datos del Objeto")]
    [SerializeField] private scr_ItemData scr_ItemData;

    [Header("Configuración")]
    [SerializeField] private KeyCode teclaInteraccion= KeyCode.E;

    [Header("Referencia")]
    [SerializeField] private GameObject panel_Interactuar;

    //private Transform playerTransform;
    private bool estaEnRango = false;

    private void Start()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    void Update()
    {
        if (!estaEnRango) return;


        // Recoger objeto
        if (Input.GetKeyDown(teclaInteraccion))
        {
            Recoger();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            estaEnRango = true;
            Debug.Log($"Presiona {teclaInteraccion} para recoger: {scr_ItemData.nombre}");
            panel_Interactuar.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            estaEnRango = false;
            panel_Interactuar.SetActive(false);
        }
    }

   
    // Recoge el objeto y activa el modo investigación
    void Recoger()
    {
        // Añadir al inventario
        InventoryManager.Instancia.agregarItem(scr_ItemData);

        // Activar modo investigación
        scr_ModoInvestigar.Instancia.StartInvestigation(scr_ItemData);

        // Destruir el objeto del mundo
        Destroy(gameObject);
        panel_Interactuar.SetActive(false);
    }


}