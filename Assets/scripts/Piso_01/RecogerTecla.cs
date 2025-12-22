using UnityEngine;

public class RecogerTecla : MonoBehaviour
{
    private void Start()
    {
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

            GameManagerGlobal.Instance.teclaRecogida = true;

            gameObject.SetActive(false);

            Debug.Log("Tecla recogida.");
        }
    }
}