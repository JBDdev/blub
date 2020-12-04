using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeControls : MonoBehaviour
{

    [SerializeField] float yOffset;

    Transform bodyRef;
    // Reference to the body & its transform
    // Y coordinate to lock on to

    // Start is called before the first frame update
    void Start()
    {
        bodyRef = transform.parent.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        bodyRef = transform.parent.GetChild(0);

        transform.position = new Vector3(bodyRef.position.x, bodyRef.position.y + yOffset);
    }
}
