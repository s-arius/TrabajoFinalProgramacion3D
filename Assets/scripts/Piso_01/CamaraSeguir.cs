using UnityEngine;

public class CamaraSeguir : MonoBehaviour
{
    [Header("Referencia")]
    public Camera camaraFPS;

    [Header("Keypad")]
    public Vector3 offsetColocacion = new Vector3(0f, 0f, -1f);
    public float suavizado = 8f;

    private bool enColocacion = false;
    private Transform slotActual;

    void LateUpdate()
    {
        if (!enColocacion || slotActual == null)
            return;

        Vector3 objetivo = slotActual.position + offsetColocacion;

        camaraFPS.transform.position = Vector3.Lerp(
            camaraFPS.transform.position,
            objetivo,
            suavizado * Time.deltaTime
        );

        camaraFPS.transform.LookAt(slotActual.position);
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
