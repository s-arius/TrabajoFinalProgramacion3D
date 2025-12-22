using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class CuadroRoto : MonoBehaviour
{
    GameObject cuadro;
    GameObject cuadroRoto;
    public GameObject llavecitas;
    public scr_ObjetoBloqueador bloq;
    Renderer[] cuadroRenderer;

    float timer = 0f;
    bool waiting = false;

    bool isBlinking = false;
    float delay = 1f;
    float visibleTime = 2f;
    float blink = 0.5f;
    float blinkTimer = 0;

    float offTime = 0.3f;
    bool isOff = false;

    public float appearSpeed =2f; 
    bool isMoving = false;



    void Start()
    {
        cuadro = transform.GetChild(0).gameObject;
        cuadroRoto = transform.GetChild(1).gameObject;
        
        cuadroRoto.SetActive(false);
        //llavecitas.SetActive(false);
        llavecitas.GetComponent<Collider>().enabled = false;
        cuadroRenderer = cuadroRoto.GetComponentsInChildren<Renderer>();

        foreach (Renderer r in cuadroRenderer)
        {
            r.enabled = false;
        }
    }

    void Update()
    {
        breakPainting();
        if (isMoving)
        {
            // move forward in local z-axis
            cuadroRoto.transform.Translate(Vector3.forward * appearSpeed * Time.deltaTime);

            // optional: stop after moving a certain distance
            // e.g., move 1 unit forward
            if (cuadroRoto.transform.localPosition.z >= 6f)
            {
                isMoving = false;
            }
        }
    }

    public void SetRenderers(bool value)
    {
        foreach (Renderer r in cuadroRenderer)
        {
            r.enabled = value;
        }
    }


    public void breakPainting()
    {
        //if (Input.GetKeyDown(KeyCode.Space) && !waiting)
        if(bloq.cuadroActivado && !waiting)
        {
            waiting = true;
            timer = 0f;
        }

        if (!waiting) return;

            timer += Time.deltaTime;

            if (timer >= delay && !cuadroRoto.activeSelf)
            {
                cuadro.SetActive(false);
                cuadroRoto.SetActive(true);
                //llavecitas.SetActive(true);
                llavecitas.GetComponent<Collider>().enabled = true;

                SetRenderers(true);

                isMoving = true;

        }

            if (timer >= delay + visibleTime && !isBlinking)
        {
            isBlinking = true;
            blinkTimer = 0f;
        }

        if (isBlinking)
        {
            blinkTimer += Time.deltaTime;

            if (!isOff && blinkTimer >= blink)
            {
                SetRenderers(false); // OFF
                isOff = true;
                blinkTimer = 0f;
            }
            else if (isOff && blinkTimer >= offTime)
            {
                SetRenderers(true); // ON
                isOff = false;
                blinkTimer = 0f;
            }

            if (timer >= delay + visibleTime + 3f)
            {
                SetRenderers(false);
                waiting = false;
                isBlinking = false;
                isOff = false;
                blinkTimer = 0f;
                Destroy(gameObject);
            }
        }

    }
}

