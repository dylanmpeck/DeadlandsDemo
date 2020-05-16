using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField] GameObject spawnLocation;
    Plane plane;

    // Start is called before the first frame update
    void Start()
    {
        plane = new Plane(Vector3.up, Vector3.zero);
    }

    // Update is called once per frame
    void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float ent = 100.0f;
        if (plane.Raycast(ray, out ent))
        {
            var hitpoint = ray.GetPoint(ent);
            transform.position = hitpoint;
        }
    }
}
