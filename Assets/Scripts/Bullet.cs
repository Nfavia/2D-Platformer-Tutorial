using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 1f;

    float xSpeed;

    Rigidbody2D myRB2D;
    PlayerMove player;

    private void Awake()
    {
        myRB2D = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMove>();
        xSpeed = player.transform.localScale.x * bulletSpeed;
    }

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        myRB2D.velocity = new Vector2(xSpeed, myRB2D.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))       
            Destroy(collision.gameObject);        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
            Destroy(collision.gameObject);

        Destroy(gameObject);
    }
}
