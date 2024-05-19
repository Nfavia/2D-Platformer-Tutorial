using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2f;

    Rigidbody2D myRB2D;

    private void Awake()
    {
        myRB2D = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        
    }


    void FixedUpdate()
    {
        myRB2D.velocity = new Vector2(moveSpeed, myRB2D.velocity.y);

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        moveSpeed = -moveSpeed;
        FlipEnemyFacing();
    }

    void FlipEnemyFacing()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(myRB2D.velocity.x)), 1f);
        GetComponent<SpriteRenderer>().flipX = false;
    }


}
