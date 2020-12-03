using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 0.1f;
    [SerializeField] float stretchSpeed = 0.1f;
    [SerializeField] float maxStrech = 10f;
    [SerializeField] float minStretch = 3f;

    [SerializeField] bool holdingRight = false;
    [SerializeField] bool holdingLeft = false;
    [SerializeField] bool holdingSpace = false;
    [SerializeField] bool holdingShift = false;

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = transform.GetComponentInChildren<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
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

    void FixedUpdate()
    {
        if (holdingRight)
        {
            rb.AddForce(new Vector2(moveSpeed * Time.deltaTime, 0.0f));
        }

        if (holdingLeft)
        {
            rb.AddForce(new Vector2(-moveSpeed * Time.deltaTime, 0.0f));
        }

        if (holdingSpace)
        {
            transform.localScale = new Vector2(transform.localScale.x + (stretchSpeed * Time.deltaTime), 3.0f);
        }

        if (holdingShift)
        {
            transform.localScale = new Vector2(transform.localScale.x + (-stretchSpeed * Time.deltaTime), 3.0f);
        }

    }
}
