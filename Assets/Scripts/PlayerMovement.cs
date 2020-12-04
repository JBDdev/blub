using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 0.1f;
    [SerializeField] float stretchSpeed = 0.1f;
    [SerializeField] float maxStretch = 5f;

    [SerializeField] bool holdingRight = false;
    [SerializeField] bool holdingLeft = false;
    [SerializeField] bool holdingSpace = false;
    [SerializeField] bool holdingShift = false;

    Rigidbody2D rb;

    float baseDiameter;

    // Start is called before the first frame update
    void Start()
    {
        rb = transform.GetComponentInChildren<Rigidbody2D>();
        baseDiameter = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        readInput();
    }

    void FixedUpdate()
    {
        handleLeftRight();
        handleStretch();
        
    }

    void readInput()
    {
        //TODO: Move keyboard input to separate function call
        if (Input.GetKey(KeyCode.D))
        {
            holdingRight = true;
        }
        else
        {
            holdingRight = false;
        }

        if (Input.GetKey(KeyCode.A))
        {
            holdingLeft = true;
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
    }

    void handleLeftRight()
    {
        if (holdingRight)
        {
            rb.AddForce(new Vector2(moveSpeed * Time.deltaTime, 0.0f));
        }

        if (holdingLeft)
        {
            rb.AddForce(new Vector2(-moveSpeed * Time.deltaTime, 0.0f));
        }
    }

    void handleStretch()
    {
        if (holdingSpace)
        {
            transform.localScale = new Vector2(transform.localScale.x + (stretchSpeed * Time.deltaTime), baseDiameter);
        }

        if (holdingShift)
        {
            transform.localScale = new Vector2(transform.localScale.x + (-stretchSpeed * Time.deltaTime), baseDiameter);
        }

        if (transform.localScale.x > maxStretch)
        {
            transform.localScale = new Vector2(maxStretch, baseDiameter);
        }

        if (transform.localScale.x < baseDiameter)
        {
            transform.localScale = new Vector2(baseDiameter, baseDiameter);
        }

    }
}
