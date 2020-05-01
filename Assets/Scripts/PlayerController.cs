using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    Animator playerAnimator;
    public Transform projectileSpawnLocation;
    public Vector3 adjustedProjectileSpawnLocation;
    [SerializeField] GameObject lClavicle, torso, lHand, lElbow;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] GameObject projectileEffectTest;
    bool spawnProjectile = false;


    // Start is called before the first frame update
    void Start()
    {
        adjustedProjectileSpawnLocation = projectileSpawnLocation.position;
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

            adjustedProjectileSpawnLocation = projectileSpawnLocation.position + (inputDirection * Time.deltaTime * 4.0f);

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
            adjustedProjectileSpawnLocation = projectileSpawnLocation.position;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (muzzleFlash)
                muzzleFlash.Play();
            playerAnimator.SetTrigger("Shoot");
            spawnProjectile = true;
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

    private void LateUpdate()
    {
        // Rotate arm when shooting to correct animation
        if (playerAnimator.GetCurrentAnimatorStateInfo(1).IsName("ShootRevolver") || playerAnimator.GetCurrentAnimatorStateInfo(1).IsName("ShootRevolver 0"))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * Vector3.Distance(transform.position, Camera.main.transform.position));
            mousePos.y = torso.transform.position.y - 2.2f; // height offset to make sure arm faces forward
            Vector3 lookDir = (mousePos - new Vector3(transform.position.x, torso.transform.position.y, transform.position.z)).normalized;
            lClavicle.transform.forward = lookDir;
            Vector3 prevUp = lHand.transform.forward;
            //lHand.transform.localPosition = Vector3.MoveTowards(lHand.transform.localPosition, mousePos - lHand.transform.position, 1f);

            //lHand.transform.right = -lookDir;
            //lHand.transform.forward = prevUp;
            lElbow.transform.right = -(mousePos - new Vector3(lElbow.transform.position.x, torso.transform.position.y - 2.2f, lElbow.transform.position.z)).normalized;
            //lHand.transform.forward = prevUp;

            /*if (spawnProjectile)
            {
                Instantiate(projectilePrefab, adjustedProjectileSpawnLocation, Quaternion.identity);
                if (muzzleFlash)
                    muzzleFlash.Play();
                spawnProjectile = false;
            }*/
        }
    }
}
