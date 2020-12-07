using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeControls : MonoBehaviour
{

    [SerializeField] float yOffset;
    Transform bodyReference;

    void Start()
    {
        bodyReference = transform.parent.GetChild(0);
    }

    void Update()
    {
        bodyReference = transform.parent.GetChild(0);

        transform.position = new Vector3(bodyReference.position.x, bodyReference.position.y + yOffset);
    }
}
