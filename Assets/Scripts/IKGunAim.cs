using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class IKGunAim : MonoBehaviour
{
    Animator animator;
    [SerializeField] GameObject torso;
    [SerializeField] GameObject gun;
    Quaternion gunIdleRot;
    [HideInInspector] public Vector3 target;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        gunIdleRot = gun.transform.localRotation;
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (animator)
        {
            if (animator.GetCurrentAnimatorStateInfo(layerIndex).IsName("ShootRevolver") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Roll"))
            {
                //Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * Vector3.Distance(transform.position, Camera.main.transform.position));
                target.y = torso.transform.position.y;
                Vector3 lookDir = (target - torso.transform.position).normalized;
                animator.SetLookAtWeight(1);
                animator.SetLookAtPosition(target);
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                animator.SetIKPosition(AvatarIKGoal.RightHand, target);
                target.y = gun.transform.position.y;
                gun.transform.LookAt(gun.transform);
            }
            else
            {
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
                animator.SetLookAtWeight(0);
                gun.transform.localRotation = gunIdleRot;
            }
        }

    }
}
