using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float lenght;
    private float startPos;

    [SerializeField]
    private GameObject camera;
    [SerializeField]
    public float parallaxEffect;


    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position.x;
        lenght = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float temp = camera.transform.position.x * (1 - parallaxEffect);
        float distance = camera.transform.position.x * parallaxEffect;

        transform.position = new Vector3(startPos + distance, transform.position.y,transform.position.z);

        if (temp > startPos + lenght)
            startPos += lenght;
        else if (temp < startPos - lenght)
            startPos -= lenght;
    }
}
