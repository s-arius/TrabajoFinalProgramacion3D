using UnityEngine;

public class Llave : MonoBehaviour
{

    void Start()
    {
        if (GameManagerGlobal.Instance.llaveRecogida)
        {
            gameObject.SetActive(false);


        }
    }

    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManagerGlobal.Instance.llaveRecogida = true;
            gameObject.SetActive(false);
            Debug.Log("has recogido una llave");
        }
    }
}