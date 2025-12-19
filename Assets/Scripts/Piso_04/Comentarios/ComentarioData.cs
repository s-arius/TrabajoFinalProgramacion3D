using UnityEngine;

[CreateAssetMenu(fileName = "NuevoComentario", menuName = "ScriptableObjects/Comentario")]
public class ComentarioData : ScriptableObject
{

    public string nombre;

    public GameObject comentarioPrefab;

    [TextArea(2,2)] public string comentario;

    public float duracionTexto = 3f;


  
}
