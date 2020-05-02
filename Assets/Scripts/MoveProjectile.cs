using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class MoveProjectile : MonoBehaviour
{
    float speed = 16.0f;
    [SerializeField] TrailRenderer trailRenderer;

    // Start is called before the first frame update
   /* void Start()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * Vector3.Distance(transform.position, Camera.main.transform.position));
        mousePos.y = transform.position.y;
        transform.LookAt(mousePos);
    }*/

    private void OnEnable()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * Vector3.Distance(transform.position, Camera.main.transform.position));
        mousePos.y = transform.position.y;
        transform.LookAt(mousePos);
        if (trailRenderer)
            trailRenderer.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        //transform.position = Vector3.MoveTowards(transform.position, startingMousePos, Time.deltaTime * speed);
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        if (!onScreen)
            gameObject.SetActive(false);
    }
}
