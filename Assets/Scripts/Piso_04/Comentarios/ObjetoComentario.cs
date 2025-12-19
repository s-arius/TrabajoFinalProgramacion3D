using UnityEngine;
using TMPro;

public class ObjetoComentario : MonoBehaviour
{
    public ComentarioData comentarioData;

    public KeyCode tecla = KeyCode.E;
        public GameObject prefabTextoInteractuar;
    public GameObject prefabTextoComentario;
    
    private GameObject uiInteractuar;
    private GameObject uiComentario;

    
    private bool enRango = false;
    private bool viendoComentario = false;
    

    void Start()
    {
        GetComponent<Collider>().isTrigger = true;


    }
    
    void Update()
    {
        if (enRango && Input.GetKeyDown(tecla) && !viendoComentario)
        {
            MostrarComentario();
        }

    }
    
    void MostrarComentario()
    {
        viendoComentario = true;
        
                if (prefabTextoComentario != null && comentarioData != null)
        {
            uiComentario = Instantiate(prefabTextoComentario, 
                transform.position + Vector3.up * 2f,Quaternion.identity);
            
            TextMeshPro tmp = uiComentario.GetComponent<TextMeshPro>();
            if (tmp != null) tmp.text = comentarioData.comentario;
            



            Destroy(uiComentario, comentarioData.duracionTexto);//destruir
        }
        
        Invoke("Reactivar", comentarioData.duracionTexto + 1f); //volver a enseñar despues de un rato para no spammear
    }
    
    void Reactivar()
    {
        viendoComentario = false;
        if (enRango) UIInteractuar();
    }
    

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enRango = true;
            if (!viendoComentario) UIInteractuar();
        }
    }

void UIInteractuar()
    {
        if (prefabTextoInteractuar != null && !viendoComentario)
        {
            uiInteractuar = Instantiate(prefabTextoInteractuar,
                transform.position + Vector3.up * 1.5f,Quaternion.identity);
        }
    }
    

    

}