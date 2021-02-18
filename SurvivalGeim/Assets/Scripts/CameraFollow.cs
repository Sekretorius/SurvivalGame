using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow instance;

    [SerializeField]
    private CompositeCollider2D collider;

    [SerializeField]
    private GameObject target;

    private Camera camera;
    private float halfExtent;
    


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(this);

        if (collider == null)
            collider = GameObject.FindGameObjectWithTag("Ground").GetComponent<CompositeCollider2D>();

    }

    void Start()
    {
        camera = GetComponent<Camera>();
        halfExtent = camera.orthographicSize * Screen.width / Screen.height;

    }

    private void LateUpdate()
    {
        transform.position = new Vector3(
            Mathf.Clamp(target.transform.position.x, collider.bounds.min.x + halfExtent, collider.bounds.max.x - halfExtent),
            gameObject.transform.position.y, gameObject.transform.position.z);
    }

    public void RefreshPosition()
    {
        if(this != null)
            gameObject.transform.position = new Vector3(target.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
    }
}
