using UnityEngine;

public class scr_RotacionObjeto : MonoBehaviour
{
  
    [Header("Configuración")]
    [Tooltip("Velocidad de rotación")]
    public float velocidadRotacion = 10f;


    void OnMouseDrag()
    {

        float rotX = Input.GetAxis("Mouse X") * velocidadRotacion;
        float rotY = Input.GetAxis("Mouse Y") * velocidadRotacion;


        transform.Rotate(Vector3.up, -rotX, Space.World);


        transform.Rotate(Vector3.right, rotY, Space.World);
    }
}


