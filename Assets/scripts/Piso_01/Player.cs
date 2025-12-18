using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    public float mouseSensitivity = 100f;
    public bool tengoTecla = false;


    private CharacterController controller;
    private float xRotation = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

        tengoTecla = GameManagerGlobal.Instance.teclaRecogida;

    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 move = transform.right * h + transform.forward * v;
        controller.Move(move * speed * Time.deltaTime);

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        Camera cam = GetComponentInChildren<Camera>();
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }
}
