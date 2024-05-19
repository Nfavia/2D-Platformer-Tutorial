using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [Header("Movement Speeds")]
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;

    [Header("Death Settings")]
    [SerializeField] Vector2 deathFlingForce = new Vector2(0f, 5f);
    [SerializeField] PhysicsMaterial2D deathMaterial;
    [SerializeField] float deathDelay = 1f;

    [Header("Gun Settings")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform gunTransform;

    float playerDefaultGravity = 2f;

    bool isClimbing = false;
    bool isAlive = true;

    Vector2 moveInput;

    Rigidbody2D myRB2D;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    ParticleSystem myParticleSystem;
    GameManager myGM;

    private void Awake()
    {
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        myRB2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myParticleSystem = GetComponentInChildren<ParticleSystem>();
        myGM = FindObjectOfType<GameManager>();
    }

    void Start()
    {
        playerDefaultGravity = myRB2D.gravityScale;
    }

    void FixedUpdate()
    {
        if (!isAlive) { return; }

        Run();
        FlipSprite();
        ClimbLadder();

        PlayerDies();
    }

    private void OnMove(InputValue value)
    {
        if (!isAlive) { return; }
        moveInput = value.Get<Vector2>();
    }

    private void OnJump(InputValue value)
    {
        if (!isAlive) { return; }
        if (value.isPressed && IsFeetTouchingLayers("Ground"))
        {
            myRB2D.velocity += new Vector2(0f, jumpSpeed);
        }
    }

    private void OnFire(InputValue value)
    {
        if(!isAlive) { return; }

        GameObject bullet = Instantiate(bulletPrefab, gunTransform.position, gunTransform.rotation);
    }

    private void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, myRB2D.velocity.y);
        myRB2D.velocity = playerVelocity;

        if(PlayerHasHorizSpeed() && !isClimbing)
            myAnimator.SetBool("isRunning", true);
        else
            myAnimator.SetBool("isRunning", false);
    }

    void ClimbLadder()
    {
        if (!IsFeetTouchingLayers("Ladder")) 
        { 
            isClimbing = false;
            myAnimator.SetBool("isClimbing", false);
            myRB2D.gravityScale = playerDefaultGravity;
            return;
        }
        isClimbing = true;
        Vector2 playerVelocity = new Vector2(myRB2D.velocity.x, moveInput.y * climbSpeed);
        myRB2D.velocity = playerVelocity;
        myRB2D.gravityScale = 0f;

        myAnimator.SetBool("isClimbing", PlayerHasVertSpeed());
    }
    void PlayerDies()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy","Hazards")))      
        {
            isAlive = false;

            myAnimator.SetTrigger("Death");
            myRB2D.constraints = RigidbodyConstraints2D.None;
            myBodyCollider.sharedMaterial = deathMaterial;
            myRB2D.AddForce(myRB2D.velocity + (deathFlingForce * 100));
            myParticleSystem.Play();
            StartCoroutine(ProcessDeath());
        }
    }

    // Easy way to check if touching layer
    bool IsFeetTouchingLayers(string layer)
    {
        return myFeetCollider.IsTouchingLayers(LayerMask.GetMask(layer));
    }

    void FlipSprite()
    {
        if (PlayerHasHorizSpeed())
            transform.localScale = new Vector2(Mathf.Sign(myRB2D.velocity.x), 1f);
    }

    bool PlayerHasHorizSpeed()
    {
        return Mathf.Abs(myRB2D.velocity.x) > Mathf.Epsilon;
    }

    bool PlayerHasVertSpeed()
    {
        return Mathf.Abs(myRB2D.velocity.y) > Mathf.Epsilon;
    }    

    IEnumerator ProcessDeath()
    {
        yield return new WaitForSeconds(deathDelay);
        myGM.ProcessPlayerDeath();
    }
    

}
