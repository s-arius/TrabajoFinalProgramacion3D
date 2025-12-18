using UnityEngine;


[CreateAssetMenu(fileName = "NuevoItem", menuName = "ScriptableObjects/Item")] //Aquí solo estamos "creando la posibilidad de crearlo en la escena" y de forma ordenada. para crearlo le damos a Create -> scriptableObjects -> Item y esto nos creará un itemSO 
                                                                               //llamado NuevoItem 
public class scr_ItemData : ScriptableObject
{

    public string nombre;

    public GameObject investigationPrefab;

    public Sprite icono;

    [TextArea(2,2)] public string pensamiento;

  
}
