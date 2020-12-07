using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float speedThreshold;
    [Range(0.0001f, 0.1f)] public float lerpSpeedToCenter;
    [Range(0.0001f, 0.1f)] public float lerpSpeedToOffset;
    [SerializeField] float cameraOffsetX;
    [SerializeField] GameObject player;
   
    Rigidbody2D playerRB;

    void Start()
    {
        playerRB = player.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (playerRB.velocity.x > speedThreshold)
        {
            transform.position = Vector3.Lerp
                (
                    transform.position,
                    new Vector3
                        (
                            Mathf.Clamp(player.transform.position.x + (cameraOffsetX * 2), player.transform.position.x - cameraOffsetX, player.transform.position.x + cameraOffsetX),
                            player.transform.position.y,
                            transform.position.z
                        ),
                    lerpSpeedToOffset
                );
        }
        else if (playerRB.velocity.x < -speedThreshold)
        {
            transform.position = Vector3.Lerp
                (
                    transform.position,
                    new Vector3
                        (
                            Mathf.Clamp(player.transform.position.x - (cameraOffsetX * 2), player.transform.position.x - cameraOffsetX, player.transform.position.x + cameraOffsetX),
                            player.transform.position.y,
                            transform.position.z
                         ),
                    lerpSpeedToOffset
                );
        }
        else
        {
            transform.position = Vector3.Lerp
            (
                transform.position,
                new Vector3
                (
                    player.transform.position.x,
                    player.transform.position.y,
                    transform.position.z
                ),
                lerpSpeedToCenter
            );
        }       
    }
}
