using UnityEngine;

public class RecogerTecla : MonoBehaviour
{
    private void Start()
    {
        // Si la tecla ya fue recogida en otra escena, la desactivamos
        if (GameManagerGlobal.Instance.teclaRecogida)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        Player player = other.GetComponent<Player>();

        if (player != null && !GameManagerGlobal.Instance.teclaRecogida)
        {
            player.tengoTecla = true;

            // Guardamos el estado global para que no reaparezca
            GameManagerGlobal.Instance.teclaRecogida = true;

            // Desactivamos la tecla en el mundo
            gameObject.SetActive(false);

            Debug.Log("Tecla recogida. Ahora el jugador la tiene y no volverá a aparecer.");
        }
    }
}
