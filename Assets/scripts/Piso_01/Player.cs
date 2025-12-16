using UnityEngine;

public class Player : MonoBehaviour
{
    public float velocidad = 5f;
    public bool tengoTecla = false;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        // Recuperar estado global
        tengoTecla = GameManagerGlobal.Instance.teclaRecogida;
    }

    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 mov = new Vector3(h, 0, v);
        rb.MovePosition(rb.position + mov * velocidad * Time.fixedDeltaTime);
    }
}
