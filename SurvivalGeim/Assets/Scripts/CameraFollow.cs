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
    private float halfExtentH;

    [SerializeField]
    private bool topDown = false;

    public bool block = false;

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
        halfExtentH = Camera.main.orthographicSize;

    }

    private void LateUpdate()
    {
        if(!block)
            SetCameraPosition();
    }
    
    private void SetCameraPosition()
    {
        transform.position = new Vector3(
            Mathf.Clamp(target.transform.position.x, collider.bounds.min.x + halfExtent, collider.bounds.max.x - halfExtent),
            topDown == true ?
            Mathf.Clamp(target.transform.position.y, collider.bounds.min.y + halfExtentH, collider.bounds.max.y - halfExtentH) :
            gameObject.transform.position.y
            ,
            gameObject.transform.position.z);
    }

    public void RefreshPosition()
    {
        if (this != null)
            SetCameraPosition();
    }
}
