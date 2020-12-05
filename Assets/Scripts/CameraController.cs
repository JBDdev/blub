using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float lerpSpeed;
    [SerializeField] float maxCameraOffset;
    [SerializeField] GameObject player;

    Rigidbody2D playerRB;

    // Start is called before the first frame update
    void Start()
    {
        playerRB = player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        // TODO:
        //  
        // Interpolate camera movement on x based on velocity
        // Revert camera back to camera origin when within a certain low-velocity threshold
        //
        transform.position = new Vector3
            (
                Mathf.Clamp
                (
                    player.transform.position.x + ((player.transform.position.x + maxCameraOffset) * lerpSpeed), 
                    player.transform.position.x -maxCameraOffset, 
                    player.transform.position.x + maxCameraOffset
                ),
                player.transform.position.y,
                transform.position.z
            );
           
    }
}
