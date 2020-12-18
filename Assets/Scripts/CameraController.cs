using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Range(0.0001f, 1f)] public float lerpSpeed;
    [SerializeField] GameObject player;
   
    Rigidbody2D playerRB;

    Vector3 direction;

    void Start()
    {
        playerRB = player.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        transform.position = Vector3.LerpUnclamped(transform.position, new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z), lerpSpeed);
    }
}
