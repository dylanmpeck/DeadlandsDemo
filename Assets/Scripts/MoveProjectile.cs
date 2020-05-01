using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveProjectile : MonoBehaviour
{
    float speed = 16.0f;
    public GameObject muzzlePrefab;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * Vector3.Distance(transform.position, Camera.main.transform.position));
        mousePos.y = transform.position.y;
        transform.LookAt(mousePos);

        if (muzzlePrefab != null)
        {
            GameObject muzzleFX = Instantiate(muzzlePrefab, transform.position, transform.rotation);
            muzzleFX.transform.SetParent(GameObject.FindGameObjectWithTag("Player").transform);
        }

        StartCoroutine(Parenting());
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        //transform.position = Vector3.MoveTowards(transform.position, startingMousePos, Time.deltaTime * speed);
    }

    IEnumerator Parenting()
    {
        transform.SetParent(GameObject.FindGameObjectWithTag("Player").transform);
        yield return new WaitForFixedUpdate();
        transform.SetParent(null);
    }
}
