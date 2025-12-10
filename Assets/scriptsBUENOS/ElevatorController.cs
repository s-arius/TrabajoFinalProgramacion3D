using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    public float moveDistance = 100f;
    public float moveSpeed = 5f;

    Vector3 targetPosition;

    void Start()
    {
        targetPosition = transform.position;
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);
    }

    public void MoveUp()
    {
        targetPosition += Vector3.up * moveDistance;
    }

    public void MoveDown()
    {
        targetPosition += Vector3.down * moveDistance;
    }
}
