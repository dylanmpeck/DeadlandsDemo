using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class MoveProjectile : MonoBehaviour
{
    float speed = 16.0f;
    public ParticleSystem hitEffect;

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        DeactivateOffScreen();
    }

    void DeactivateOffScreen()
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        if (!onScreen)
            gameObject.SetActive(false);
    }

    public void Hit()
    {
        gameObject.SetActive(false);
        if (hitEffect)
        {
            hitEffect.transform.position = transform.position;
            hitEffect.transform.forward = -transform.forward;
            hitEffect.Play();
        }
    }
}
