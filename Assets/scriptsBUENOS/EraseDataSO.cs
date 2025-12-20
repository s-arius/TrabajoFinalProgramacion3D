using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EraseDataSO", menuName = "GameData/EraseDataSO")]
public class EraseDataSO : ScriptableObject
{
    // Diccionario temporal para almacenar qué objetos fueron borrados
    private Dictionary<string, bool> erasedObjects = new Dictionary<string, bool>();

    // Establecer un objeto como borrado
    public void SetErased(string objectID, bool erased)
    {
        if (erasedObjects.ContainsKey(objectID))
            erasedObjects[objectID] = erased;
        else
            erasedObjects.Add(objectID, erased);
    }

    // Consultar si un objeto fue borrado
    public bool GetErased(string objectID)
    {
        if (erasedObjects.ContainsKey(objectID))
            return erasedObjects[objectID];
        return false;
    }

    // Limpiar los datos temporales al cerrar el juego
    public void ResetData()
    {
        erasedObjects.Clear();
    }
}
