using UnityEngine;

public class CamaraSeguir : MonoBehaviour
{
    public Transform jugador;

    public Vector3 offsetNormal = new Vector3(0, 5, -7);
    public Vector3 offsetColocacion = new Vector3(0, 2, -3);

    public float suavizado = 5f;

    private bool enColocacion = false;
    private Transform slotActual;

    void LateUpdate()
    {
        Vector3 objetivo;
        Vector3 mirar;

        if (enColocacion && slotActual != null)
        {
            objetivo = slotActual.position + offsetColocacion;
            mirar = slotActual.position;
        }
        else
        {
            objetivo = jugador.position + offsetNormal;
            mirar = jugador.position;
        }

        transform.position = Vector3.Lerp(transform.position, objetivo, suavizado * Time.deltaTime);
        transform.LookAt(mirar);
    }

    public void ActivarColocacion(Transform slot)
    {
        enColocacion = true;
        slotActual = slot;
    }

    public void DesactivarColocacion()
    {
        enColocacion = false;
        slotActual = null;
    }
}
