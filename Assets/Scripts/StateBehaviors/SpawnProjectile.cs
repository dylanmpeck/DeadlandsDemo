﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class SpawnProjectile : StateMachineBehaviour
{

    public AssetReference projectilePrefab;
   // public GameObject projectilePrefab;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       //Instantiate(projectilePrefab, animator.GetComponent<PlayerController>().adjustedProjectileSpawnLocation, Quaternion.identity);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        if (Mathf.Approximately(stateInfo.normalizedTime, (.10f / stateInfo.length)))
        {
            // projectilePrefab.InstantiateAsync(animator.GetComponent<PlayerController>().projectileSpawnLocation.position, Quaternion.identity);
            PlayerController playerController = animator.GetComponent<PlayerController>();
            Vector3 spawnLocation = playerController.projectileSpawnLocation.position;
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * Vector3.Distance(spawnLocation, Camera.main.transform.position));
            mousePos.y = spawnLocation.y;

            GameObject projectile = playerController.objectPoolHandler.GetCurrentActiveObject();
            projectile.transform.SetPositionAndRotation(playerController.projectileSpawnLocation.position, Quaternion.LookRotation((mousePos - spawnLocation).normalized));
            projectile.SetActive(true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}