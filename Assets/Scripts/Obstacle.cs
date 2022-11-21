using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private float obstacle_fast = 1.5f;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.Instance.State == GameController.GameState.GAME_OVER)
        {
            obstacle_fast = 1.5f;
        }
        if (rb.bodyType == RigidbodyType2D.Kinematic && GameController.Instance.State == GameController.GameState.PLAY)
        {
            transform.localPosition += Vector3.down * Time.deltaTime * obstacle_fast;
            obstacle_fast += 0.001f;
        }
        if(transform.position.y < -8 || transform.position.x > 6 || transform.position.x < -6)
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(gameObject.CompareTag("Obstacle") && (collision.collider.CompareTag("Obstacle") ||
            collision.collider.CompareTag("Protection")))
        {
            rb.bodyType= RigidbodyType2D.Dynamic; 
        }
    }
}
