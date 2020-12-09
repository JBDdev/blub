using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Inspector Fields
    [Space]

    [Header("Speed Controls")]
    [SerializeField] float moveSpeed = 0.1f;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float stretchSpeed = 0.1f;
    

    [Space]

    [Header("Slam Controls")]
    [SerializeField] float aerialRotationSpeed = 0.1f;
    [SerializeField] float slamGravity = 1f;
    [SerializeField] float bounceModifier = 0.1f;
    [SerializeField] PhysicsMaterial2D floorPhysicsMaterial;

    [Header("Player Constraints")]
    [SerializeField] float maxStretch = 5f;
    [SerializeField] float maxVelocity = 10f;

    [Space]

    [Header("Dash Controls")]
    [SerializeField] float dashCooldown = 1f;
    [SerializeField] float dashForce = 10f;
    [SerializeField] float dashRotationalSpeed = 5f;
    [SerializeField] float rotationalForceOffset = 1f;

    [Space]

    [Header("Input Registration")]
    [SerializeField] bool holdingRight = false;
    [SerializeField] bool holdingLeft = false;
    [SerializeField] bool holdingSpace = false;
    [SerializeField] bool holdingShift = false;
    [SerializeField] bool holdingDown = false;
    [Space]
    [SerializeField] bool usedJump = false;
    #endregion

    #region Private Fields
    bool canDash = true;
    bool rotationCanceled = false;
    float baseDiameter;
    float baseGravityScale;
    #endregion

    #region Component References
    Rigidbody2D rb;
    CircleCollider2D circleHitbox;
    PolygonCollider2D ovalHitbox;
    Animator eyesAnimator;
    #endregion


    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        eyesAnimator = transform.parent.GetComponentInChildren<Animator>();
        circleHitbox = transform.GetComponent<CircleCollider2D>();
        ovalHitbox = transform.GetComponent<PolygonCollider2D>();
        baseDiameter = transform.localScale.x;
        baseGravityScale = rb.gravityScale;
    }

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
            rotationCanceled = false;
            rb.gravityScale = baseGravityScale;
            Invoke("resetBounce", 0.1f);
        }
        
    }

    void readInput()
    {
        #region Jump Controls
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (!usedJump)
            {
                rb.AddForce(new Vector2(0f, jumpForce));
                usedJump = true;
            }
        }
        #endregion

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

        #region Stretch Controls
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
        #endregion

        #region Rotation Freeze Controls
        if (Input.GetKey(KeyCode.S))
        {
            holdingDown = true;
        }
        else
        {
            holdingDown = false;
        }
        #endregion
    }

    void handleMovement()
    {
        #region Left / Right Movement
        if (holdingRight && (rb.velocity.x <= maxVelocity))
        {
            rb.AddForce(new Vector2(moveSpeed * Time.deltaTime, 0.0f));
        }

        if (holdingLeft && (rb.velocity.x >= -maxVelocity))
        {
            rb.AddForce(new Vector2(-moveSpeed * Time.deltaTime, 0.0f));
        }
        #endregion

        #region Ground Slam
        if (holdingDown && rotationCanceled)
        {
            rb.gravityScale = slamGravity;
            floorPhysicsMaterial.bounciness = bounceModifier;
        }

        #endregion

        #region Rotation Controls
        if (holdingDown)
        {
            rb.freezeRotation = true;
            if (usedJump)
            {
                rotationCanceled = true;
            }
        }
        else
        {
            rb.freezeRotation = false;
            if (holdingRight && rotationCanceled)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z - aerialRotationSpeed);
            }
            else if (holdingLeft && rotationCanceled)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + aerialRotationSpeed);
            }
        }
        #endregion

    }

    void handleStretch()
    {
        #region Input Handling
        if (holdingSpace)
        {
            transform.localScale = new Vector2(transform.localScale.x + (stretchSpeed * Time.deltaTime), baseDiameter);
        }

        if (holdingShift)
        {
            transform.localScale = new Vector2(transform.localScale.x + (-stretchSpeed * Time.deltaTime), baseDiameter);
        }
        #endregion

        #region Stretch Constraints
        if (transform.localScale.x > maxStretch)
        {
            transform.localScale = new Vector2(maxStretch, baseDiameter);
        }

        if (transform.localScale.x < baseDiameter)
        {
            transform.localScale = new Vector2(baseDiameter, baseDiameter);
        }
        #endregion

        #region Collider Swapping
        if (transform.localScale.x <= baseDiameter)
        {
            circleHitbox.enabled = true;
            ovalHitbox.enabled = false;
        }
        else
        {
            ovalHitbox.enabled = true;
            circleHitbox.enabled = false;
        }
        #endregion

    }

    void resetDash()
    {
        canDash = true;
    }

    void resetBounce()
    {
        floorPhysicsMaterial.bounciness = 0f;
    }
}
