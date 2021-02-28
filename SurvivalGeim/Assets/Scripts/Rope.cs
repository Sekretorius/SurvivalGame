using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    private List<Vector2> ropePoints = new List<Vector2>();
    private float distanceBetweenPoints = 0.5f;
    private float speed = .01f;
    private void Start()
    {
        ropePoints.Add(new Vector2(0, 0));
        for (int i = 1; i < 30; i++)
        {
            ropePoints.Add(ropePoints[i - 1] + new Vector2(0, 0));
        }
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            ropePoints[0] = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    private void FixedUpdate()
    {
        //if (!Input.GetMouseButton(0))
        //{
        //    Vector2 cfd = ropePoints[1] - ropePoints[0];
        //    float dtc = distanceBetweenPoints - Vector2.Distance(ropePoints[0], ropePoints[1]);
        //    ropePoints[0] += cfd.normalized * (dtc > 0 ? 0 : Mathf.Abs(dtc));
        //}
        Debug.DrawLine(ropePoints[0], ropePoints[1], Color.red);
        for (int i = 1; i < ropePoints.Count; i++)
        {
            Vector2 parentForceDirection = ropePoints[i - 1] - ropePoints[i];
            float distanceToParent = distanceBetweenPoints - Vector2.Distance(ropePoints[i - 1], ropePoints[i]);
            Vector2 moveDirection = Vector2.zero;
            if (i + 1 < ropePoints.Count)
            {
                Vector2 childForceDirection = ropePoints[i + 1] - ropePoints[i];
                float distanceToChild = distanceBetweenPoints - Vector2.Distance(ropePoints[i], ropePoints[i + 1]);
                moveDirection = parentForceDirection.normalized * (distanceToParent > 0 ? 0 : Mathf.Abs(distanceToParent)) + childForceDirection.normalized * (distanceToChild > 0 ? 0 : Mathf.Abs(distanceToChild));
                Debug.DrawLine(ropePoints[i], ropePoints[i + 1], Color.red);
                Debug.DrawRay(ropePoints[i], moveDirection, Color.green);
                Debug.DrawRay(ropePoints[i], parentForceDirection.normalized * (distanceToParent > 0 ? 0 : Mathf.Abs(distanceToParent)), Color.yellow);
                Debug.DrawRay(ropePoints[i], childForceDirection.normalized * (distanceToChild > 0 ? 0 : Mathf.Abs(distanceToChild)), Color.blue);
                Debug.DrawRay(ropePoints[i], Vector2.ClampMagnitude((ropePoints[i + 1] - ropePoints[i]), 0.01f), Color.green);
            }
            else
            {
                //moveDirection = parentForceDirection.normalized * (distanceToParent > 0 ? 0 : Mathf.Abs(distanceToParent));
            }
            ropePoints[i] += moveDirection; //Vector2.ClampMagnitude(moveDirection, speed);
        }
    }
}
