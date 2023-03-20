using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMove : MonoBehaviour
{

    private float speed;
    public GameObject end;
    public GameObject start;
    // Start is called before the first frame update
    void Start()
    {
        RandomizeSpeed();
    }

    private void RandomizeSpeed()
    {
        speed = Random.Range(0f, 10f);
        float size = Random.Range(0.7f, 1.3f);
        transform.localScale = new Vector3(size, size, size);
    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.right * Time.deltaTime * (speed/10));
        if ( transform.position.x > end.transform.position.x)
        {
            transform.position = new Vector3(start.transform.position.x,Random.Range(-2f,6f), transform.position.z);
            RandomizeSpeed();
        }
    }
}
