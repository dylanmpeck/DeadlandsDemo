using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] float keepAwayDistance;

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 followPos = target.transform.position + Vector3.back * keepAwayDistance;
        transform.position = new Vector3(followPos.x, transform.position.y, followPos.z);
        transform.LookAt(target.transform, Vector3.up);
        transform.RotateAround(target.transform.position, Vector3.right, 55.0f - transform.eulerAngles.x);
    }
}
