using UnityEngine;

public class CuboTrigger : MonoBehaviour
{
    public CuboFill cuboFill;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Agua"))
        {
            cuboFill.SetTo50();
        }

        if (other.CompareTag("Jabon"))
        {
            cuboFill.SetTo100();
        }
    }
}
