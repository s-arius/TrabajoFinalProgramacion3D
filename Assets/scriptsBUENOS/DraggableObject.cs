using UnityEngine;

public class DraggableObject : MonoBehaviour
{
    [Header("Configuración de arrastre")]
    public float distanceFromCamera = 2f;
    public float followSpeed = 10f;

    private Vector3 startPos;
    private Rigidbody rb;
    private bool isDragging = false;

    void Start()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        HandleMouseInput();

        if (isDragging)
        {
            MoveWithMouse();
        }
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    StartDrag();
                }
            }
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            StopDrag();
        }
    }

    void MoveWithMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = distanceFromCamera;
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(mousePos);

        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * followSpeed);

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    public void StartDrag()
    {
        isDragging = true;

        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }

    public void StopDrag()
    {
        isDragging = false;

        if (rb != null)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }

        // ⭐ IMPORTANTE: Guardar nueva posición como "startPos"
        startPos = transform.position;
    }

    public void ResetToStart()
    {
        // Ahora vuelve a la última posición en la que lo dejaste
        transform.position = startPos;

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.useGravity = true;
            rb.isKinematic = false;
        }
    }
}
