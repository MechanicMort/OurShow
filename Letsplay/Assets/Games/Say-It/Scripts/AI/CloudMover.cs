using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMover : MonoBehaviour
{
    public float cloudSpeed = 1;

    void Update()
    {
        transform.position += new Vector3(cloudSpeed, 0, 0) * Time.deltaTime;

        if (transform.position.x > 10)
        {
            transform.position = new Vector3(-10, transform.position.y, transform.position.z);
        }
    }
}