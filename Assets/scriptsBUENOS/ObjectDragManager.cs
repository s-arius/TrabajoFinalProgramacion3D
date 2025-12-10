using UnityEngine;

public class ObjectDragManager : MonoBehaviour
{
    public Camera playerCamera;
    public float dragSpeed = 10f;
    public float maxDistance = 3f;

    private Transform draggedObject;
    private Vector3 originalPosition;
    private Rigidbody rb;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryPickObject();
        }

        if (Input.GetMouseButton(0) && draggedObject != null)
        {
            DragObject();
        }

        if (Input.GetMouseButtonUp(0) && draggedObject != null)
        {
            ReleaseObject();
        }
    }

    void TryPickObject()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Draggable"))
            {
                draggedObject = hit.collider.transform;
                originalPosition = draggedObject.position;

                rb = draggedObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                }
            }
        }
    }

    void DragObject()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        Vector3 targetPos = ray.GetPoint(1.5f);

        draggedObject.position = Vector3.Lerp(draggedObject.position, targetPos, Time.deltaTime * dragSpeed);
    }

    void ReleaseObject()
    {
        if (rb != null)
        {
            rb.isKinematic = false;
        }

        draggedObject = null;
    }
}
