using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 0.1f;
    [SerializeField] float stretchSpeed = 0.1f;
    [SerializeField] float maxStretch = 5f;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float maxVelocity = 10f;
    [SerializeField] bool usedJump = false;

    [SerializeField] bool holdingRight = false;
    [SerializeField] bool holdingLeft = false;
    [SerializeField] bool holdingSpace = false;
    [SerializeField] bool holdingShift = false;
    [SerializeField] bool holdingDown = false;
    

    Rigidbody2D rb;
    CircleCollider2D circleHitbox;
    PolygonCollider2D ovalHitbox;

    Animator eyesAnimator;

    float baseDiameter;

    // Start is called before the first frame update
    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        eyesAnimator = transform.parent.GetComponentInChildren<Animator>();
        circleHitbox = transform.GetComponent<CircleCollider2D>();
        ovalHitbox = transform.GetComponent<PolygonCollider2D>();
        baseDiameter = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        readInput();
    }

    void FixedUpdate()
    {
        handleMovement();
        handleStretch();

        eyesAnimator.SetFloat("Y Velocity", rb.velocity.y);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Floor") && col.GetContact(0).normal.y > 0)
        {
            usedJump = false;
        }
        
    }

    // TODO: Do the Animator SetBool call by index rather than by string name
    void readInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (!usedJump)
            {
                rb.AddForce(new Vector2(0f, jumpForce));
                usedJump = true;
            }
            
        }

        if (Input.GetKey(KeyCode.D))
        {
            holdingRight = true;
            eyesAnimator.SetBool("Look Left", false);
        }
        else
        {
            holdingRight = false;
        }

        if (Input.GetKey(KeyCode.A))
        {
            holdingLeft = true;
            eyesAnimator.SetBool("Look Left", true);
        }
        else
        {
            holdingLeft = false;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            holdingSpace = true;
        }
        else
        {
            holdingSpace = false;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            holdingShift = true;
        }
        else
        {
            holdingShift = false;
        }

        if (Input.GetKey(KeyCode.S))
        {
            holdingDown = true;
        }
        else
        {
            holdingDown = false;
        }
    }

    void handleMovement()
    {
        if (holdingRight && (rb.velocity.x <= maxVelocity))
        {
            rb.AddForce(new Vector2(moveSpeed * Time.deltaTime, 0.0f));
        }

        if (holdingLeft && (rb.velocity.x >= -maxVelocity))
        {
            rb.AddForce(new Vector2(-moveSpeed * Time.deltaTime, 0.0f));
        }

        if (holdingDown)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        else
        {
            rb.constraints = RigidbodyConstraints2D.None;
        }
    }

    void handleStretch()
    {
        //React to player input
        if (holdingSpace)
        {
            transform.localScale = new Vector2(transform.localScale.x + (stretchSpeed * Time.deltaTime), baseDiameter);
        }

        if (holdingShift)
        {
            transform.localScale = new Vector2(transform.localScale.x + (-stretchSpeed * Time.deltaTime), baseDiameter);
        }

        //Constrain the stretch of the rigid body
        if (transform.localScale.x > maxStretch)
        {
            transform.localScale = new Vector2(maxStretch, baseDiameter);
        }

        if (transform.localScale.x < baseDiameter)
        {
            transform.localScale = new Vector2(baseDiameter, baseDiameter);
        }

        //Swaps to circle collider for no stretch at all and polygon collider for any amount of stretch
        if (transform.localScale.x <= baseDiameter)
        {
            //Enable Circle collider
            circleHitbox.enabled = true;
            //Disable polygon collider
            ovalHitbox.enabled = false;
        }
        else
        {
            //Enable polygon collider
            ovalHitbox.enabled = true;
            //Disable Circle collider
            circleHitbox.enabled = false;
        }


    }
}
