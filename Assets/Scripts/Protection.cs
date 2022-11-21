using UnityEngine;

public class Protection : MonoBehaviour
{
    private bool isDown;
    private Vector3 delta;
    private Camera cam;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && isDown == false && GameController.Instance.State == GameController.GameState.PLAY)
        {
            isDown = true;
            delta = transform.position - MousePos();
        }
        if (isDown)
        {
            rb.MovePosition(delta + MousePos());
        }
        if(Input.GetMouseButtonUp(0) && isDown == true)
        {
            isDown = false;
            delta = Vector3.zero;
        }
    }

    private Vector3 MousePos()
    {
        return cam.ScreenToWorldPoint(Input.mousePosition + Vector3.forward);
    }
}
