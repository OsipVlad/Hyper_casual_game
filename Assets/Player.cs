using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public System.Action OnGameOver;
    private CircleCollider2D col;
    private Rigidbody2D rb;
    private Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Obstacle") && GameController.Instance.State == GameController.GameState.PLAY)
        {
            OnGameOver?.Invoke();
            col.isTrigger = true;
            rb.isKinematic = false;
        }
    }

    public void Reset()
    {
        col.isTrigger = false;
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;
        transform.position = startPos;
    }
}
