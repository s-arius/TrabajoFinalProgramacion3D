using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEditor.VersionControl.Asset;

public class scr_PlayerMovimiento : MonoBehaviour
{

    [Header("Configuración Movimieto")]
    [Tooltip("Velocidad de movimiento personaje")][SerializeField] private float velocidad;
    private float moveInputV, moveInputH;
    private Rigidbody rb;

    [Header("Configuración Cámara")]
    [Tooltip("Cámara en primera persona")]
    [SerializeField] private Camera camaraFPS;
    [Tooltip("la veloocidad con la que el jugador rotará la camara del personaje")]
    [SerializeField] private float velocidadMouse = 2f;
    [Tooltip("Límite de rotación vertical (arriba/abajo)")]
    [SerializeField] private float limiteVertical = 80f;

    private float rotacionEnX; //Del mundo/escena 3d
    private float rotacionEnY; 

    void Start()
    {
       rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked; //Bloquear el cursor al entrar en el modo de juego.
        Cursor.visible = false;

        
        if (camaraFPS == null) // Si no se asignó una cámara, buscar la cámara principal
        {
            camaraFPS = Camera.main; //aqui estamos referenciando la camara principal, solo util con la camara principal; Si queremos referenciar otro objeto de la misma forma tendríamos que usar un GameObject.FindGameObjectWithTag("MiTag")
        }
    }

    // Update is called once per frame
    void Update()
    {
        myMove();
        rotacionCamra_Funcion();

        // Presionar ESC para liberar el cursor EJEMPLO util para el uso de rotar en investigar
        /*if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }*/

    }

    private void myMove()
    {
        //Referencia del movimietno
        moveInputV = Input.GetAxis("Vertical");
        moveInputH = Input.GetAxis("Horizontal");

        Vector3 movimiento = (camaraFPS.transform.forward * moveInputV + camaraFPS.transform.right * moveInputH);
        movimiento.y = 0f; // Ignorar inclinación vertical para no volar/caer cuando miras arriba/abajo

        rb.MovePosition(rb.position + movimiento.normalized * velocidad * Time.deltaTime);// Se usa "MovePosition" en lugar de transform porque usa el rb,asi evitamos errores de collisione

    }//End myMove()


    private void rotacionCamra_Funcion()
    {
        //Referencia de movimiento 
        float mouseX = Input.GetAxis("Mouse X") * velocidadMouse; //Captura el movimiento del mouse en x
        float mouseY = Input.GetAxis("Mouse Y") * velocidadMouse; //Captura el movimiento del mouse en y

       //Rotacion horizontal
        rotacionEnY += mouseX; //Aqui parece confuso sumarle mouse "x" a rotacion "EnY", pero realmente si giras tu raton hacia la derecha, lo que quieres es que tu personaje gire a los lados, es decir, que rote en y, si rotara en x, giraria de arriba hacia abajo
        transform.rotation = Quaternion.Euler(0f, rotacionEnY, 0f); //Rotacion en eje Y atraves del mesh del jugador

        //Rotacin Vertical
        rotacionEnX -= mouseY;
        rotacionEnX = Mathf.Clamp(rotacionEnX, -limiteVertical, limiteVertical);//Clamp, limita a rotacion X para que nunca sea mayor que limite vertical, en caso de serlo, que devuelva el limitevertical, lo mismo si es inferior.

        camaraFPS.transform.localRotation = Quaternion.Euler(rotacionEnX, 0f, 0f); //Rotacion en el eje X. Solo la camara. Mesh de jugador estatica

    }

    

}
