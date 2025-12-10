using System.Collections;
using UnityEngine;

public class CuboTriggerReposition : MonoBehaviour
{
    [Header("REFERENCIAS")]
    public CuboFill cuboFill;
    public Transform elevator;   // El transform del elevador (padre que se mueve)
    public Transform agua;       // Transform del objeto "Agua"
    public Transform jabon;      // Transform del objeto "Jabón"

    [Header("AJUSTES")]
    public float postMixDisableSeconds = 0.2f; // tiempo que el trigger queda desactivado para evitar bucles

    // estados internos
    private bool aguaInside = false;
    private bool jabonInside = false;
    private bool mixtureDone = false;        // indica que ya se procesó la mezcla (evita repetir)
    private bool mixtureInProgress = false;  // indica que estamos reposicionando/deshabilitando trigger

    // posiciones locales originales (respecto al elevador)
    private Vector3 aguaInitialLocalPos;
    private Vector3 jabonInitialLocalPos;

    Collider myCollider;

    void Start()
    {
        if (agua == null || jabon == null)
            Debug.LogWarning("CuboTriggerReposition: asigna 'agua' y 'jabon' en el inspector.");

        // Guardamos la posición LOCAL original (respecto a su padre actual).
        // Si quieres que sea siempre relativo al elevador, asegúrate de que al inicio
        // los objetos estén parentados correctamente, o ajusta aquí.
        aguaInitialLocalPos = agua.localPosition;
        jabonInitialLocalPos = jabon.localPosition;

        myCollider = GetComponent<Collider>();
        if (myCollider == null)
            Debug.LogError("CuboTriggerReposition: el objeto Cubo necesita un Collider (Is Trigger).");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (mixtureInProgress) return;

        if (other.CompareTag("Agua"))
        {
            aguaInside = true;

            // Si el jabón NO está dentro → 50%
            if (!jabonInside)
            {
                cuboFill.SetTo50();
            }
            else
            {
                // Ambos dentro → 100% y mezclar
                cuboFill.SetTo100();
                TryMakeMixture();
            }
        }

        if (other.CompareTag("Jabon"))
        {
            jabonInside = true;

            // Si el agua NO está dentro → 50%
            if (!aguaInside)
            {
                cuboFill.SetTo50();
            }
            else
            {
                // Ambos dentro → 100% y mezclar
                cuboFill.SetTo100();
                TryMakeMixture();
            }
        }
    }

    void TryMakeMixture()
    {
        if (mixtureDone || mixtureInProgress) return;

        // Solo si ambos están dentro
        if (aguaInside && jabonInside)
        {
            mixtureDone = true;
            StartCoroutine(HandlePostMix());
        }
    }


    private void OnTriggerExit(Collider other)
    {
        // Si sale del cubo, actualizamos flags (no desactivamos mezcla aquí)
        if (other.CompareTag("Agua"))
            aguaInside = false;

        if (other.CompareTag("Jabon"))
            jabonInside = false;
    }

    IEnumerator HandlePostMix()
    {
        // Evitar nuevas entradas mientras movemos objetos
        mixtureInProgress = true;

        // Desactivar collider del cubo para evitar que al mover los objetos
        // vuelvan a generar OnTriggerEnter/Exit inmediatamente.
        if (myCollider != null) myCollider.enabled = false;

        // Reposicionar los objetos al "piso" del elevador (misma posición local que al inicio)
        // Los hacemos hijos del elevador para que sigan el movimiento del mismo.
        if (agua != null)
        {
            agua.SetParent(elevator, worldPositionStays: false); // no preservar world pos
            agua.localPosition = aguaInitialLocalPos;
            // opcional: agua.localRotation = Quaternion.identity; // si quieres reset rot
        }

        if (jabon != null)
        {
            jabon.SetParent(elevator, worldPositionStays: false);
            jabon.localPosition = jabonInitialLocalPos;
            // opcional: jabon.localRotation = Quaternion.identity;
        }

        // Si hay rigidbodies, detener cualquier velocidad residual (seguro)
        Rigidbody rbAg = agua != null ? agua.GetComponent<Rigidbody>() : null;
        Rigidbody rbJa = jabon != null ? jabon.GetComponent<Rigidbody>() : null;
        if (rbAg != null) { rbAg.linearVelocity = Vector3.zero; rbAg.angularVelocity = Vector3.zero; rbAg.isKinematic = false; rbAg.useGravity = true; }
        if (rbJa != null) { rbJa.linearVelocity = Vector3.zero; rbJa.angularVelocity = Vector3.zero; rbJa.isKinematic = false; rbJa.useGravity = true; }

        // Esperar un poco para que la física se estabilice y evitar re-detección
        yield return new WaitForSeconds(postMixDisableSeconds);

        // Reactivar el collider del cubo (permitir nuevas mezclas después)
        if (myCollider != null) myCollider.enabled = true;

        // Reset de estados: ahora ya no hay objetos dentro (hemos movido fuera)
        aguaInside = false;
        jabonInside = false;

        // Permitir mezclar nuevamente en el futuro
        mixtureInProgress = false;
        mixtureDone = false;

        // Si quieres que la mezcla solo ocurra con cooldown, aquí podrías añadir un delay mayor
        yield break;
    }
}
