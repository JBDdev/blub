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

    //Dash Controls
    [SerializeField] float dashCooldown = 1f;
    [SerializeField] float dashForce = 10f;
    [SerializeField] float dashRotationalSpeed = 5f;
    [SerializeField] float rotationalForceOffset = 1f;

    [SerializeField] bool holdingRight = false;
    [SerializeField] bool holdingLeft = false;
    [SerializeField] bool holdingSpace = false;
    [SerializeField] bool holdingShift = false;
    [SerializeField] bool holdingDown = false;

    float storedRotationalSpeed = 0f;
    bool canDash = true;

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

    void readInput()
    {
        //Jump controls
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (!usedJump)
            {
                rb.AddForce(new Vector2(0f, jumpForce));
                usedJump = true;
            }
        }



        #region Left / Right Controls
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
        #endregion

        #region Dash Controls
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E))
        {
            if (canDash)
            {
                rb.freezeRotation = true;
                rb.freezeRotation = false;

                if (rb.velocity.x >= 0)
                {
                    #region Rotational forces
                    rb.AddForceAtPosition
                        (
                            Vector2.right * dashRotationalSpeed,
                            new Vector2
                            (
                                transform.position.x ,
                                transform.position.y + rotationalForceOffset
                            ), 
                            ForceMode2D.Impulse
                        );

                    rb.AddForceAtPosition
                        (
                            Vector2.left * dashRotationalSpeed,
                            new Vector2
                            (
                                transform.position.x,
                                transform.position.y - rotationalForceOffset
                            ),
                            ForceMode2D.Impulse
                        );
                    #endregion

                    rb.AddForce(Vector2.right * dashForce, ForceMode2D.Impulse);
                }
                else
                {
                    #region Rotational forces
                    rb.AddForceAtPosition
                        (
                            Vector2.left * dashRotationalSpeed,
                            new Vector2
                            (
                                transform.position.x,
                                transform.position.y + rotationalForceOffset
                            ),
                            ForceMode2D.Impulse
                        );

                    rb.AddForceAtPosition
                        (
                            Vector2.right * dashRotationalSpeed,
                            new Vector2
                            (
                                transform.position.x,
                                transform.position.y - rotationalForceOffset
                            ),
                            ForceMode2D.Impulse
                        );
                    #endregion

                    rb.AddForce(Vector2.left * dashForce, ForceMode2D.Impulse);
                }

                canDash = false;
                Invoke("resetDash", dashCooldown);
            }
        }
        #endregion

        // Stretch & Shrink
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

        //Rotational controls
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
        //Rightward Movement
        if (holdingRight && (rb.velocity.x <= maxVelocity))
        {
            rb.AddForce(new Vector2(moveSpeed * Time.deltaTime, 0.0f));
        }

        //Leftward Movement
        if (holdingLeft && (rb.velocity.x >= -maxVelocity))
        {
            rb.AddForce(new Vector2(-moveSpeed * Time.deltaTime, 0.0f));
        }

        //Rotational controls

        if (holdingDown)
        {
            if (storedRotationalSpeed == 0f)
            {
                storedRotationalSpeed = rb.angularVelocity;
            }
            rb.freezeRotation = true;
        }
        else
        {
            rb.freezeRotation = false;

            if (storedRotationalSpeed != 0f)
            {
                rb.angularVelocity = storedRotationalSpeed;
                storedRotationalSpeed = 0f;
            }
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

    void resetDash()
    {
        canDash = true;
    }
}
