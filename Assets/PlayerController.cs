using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    Animator playerAnimator;
    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Horizontal") != 0.0f || Input.GetAxisRaw("Vertical") != 0.0f)
        {
            playerAnimator.SetBool("Running", true);
            //playerAnimator.SetFloat("Runspeed", Input.GetAxisRaw("Vertical"));
            Vector3 inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

            if (inputDirection.magnitude > 1.0f)
            {
                inputDirection = inputDirection.normalized;
            }

            Vector3 moveDirection = transform.InverseTransformDirection(inputDirection);
            playerAnimator.SetFloat("XAxis", moveDirection.x);
            playerAnimator.SetFloat("YAxis", moveDirection.z);
            transform.Translate(moveDirection * Time.deltaTime * 4.0f);
        }
        else
        {
            playerAnimator.SetBool("Running", false);
        }

        if (Input.GetMouseButtonDown(0))
        {
            playerAnimator.SetTrigger("Shoot");
        }

        LookAtMousePos();
    }

    void LookAtMousePos()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * Vector3.Distance(transform.position, Camera.main.transform.position));
        mousePos.y = transform.position.y;
        Vector3 lookDir = (mousePos - transform.position).normalized;
        transform.LookAt(mousePos);
    }
}
