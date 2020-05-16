using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    float moveSpeed = 4.0f;
    public float rollSpeed;
    Animator playerAnimator;
    Vector3 movementAxis = Vector3.zero;
    Rigidbody rb;

    [Header("Projectile Variables")]
    [SerializeField] Transform projectileSpawnLocation;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] ObjectPoolHandler projectiles;

    [Header("Body References")]
    [SerializeField] GameObject torso;
    [SerializeField] GameObject lClavicle, lHand, lElbow;

    [HideInInspector] public bool canShoot = true;
    [HideInInspector] public bool canMove = true;

    IKGunAim aimer;
    bool spawningProjectile;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        aimer = GetComponent<IKGunAim>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            movementAxis = Vector3.zero;
            movementAxis.x = Input.GetAxisRaw("Horizontal");
            movementAxis.z = Input.GetAxisRaw("Vertical");
            if (movementAxis.x != 0.0f || movementAxis.z != 0.0f)
            {
                playerAnimator.SetBool("Running", true);
            }
            else
            {
                playerAnimator.SetBool("Running", false);
            }
        }


        if (canShoot && Input.GetMouseButtonDown(0))
        {
            if (muzzleFlash)
                muzzleFlash.Play();
            if (!spawningProjectile) // call projectile spawning code in FixedUpdate to help sync with animator
                spawningProjectile = true;

            playerAnimator.SetTrigger("Shoot");
        }

        if (canMove && Input.GetKeyDown(KeyCode.Space))
        {
            playerAnimator.SetTrigger("Roll");
            rb.AddForce(movementAxis.normalized * 275, ForceMode.Impulse);
        }
    }

    void LookAtMousePos()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * Vector3.Distance(transform.position, Camera.main.transform.position));
        mousePos.y = transform.position.y;
        Vector3 lookDir = (mousePos - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(lookDir, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 5.5f * Time.deltaTime);

        // update IK aiming target
        aimer.target = mousePos;
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            Vector3 inputDirection = new Vector3(movementAxis.x, 0, movementAxis.z);

            if (inputDirection.magnitude > 1.0f)
            {
                inputDirection = inputDirection.normalized;
            }

            Vector3 moveDirection = transform.InverseTransformDirection(inputDirection);
            playerAnimator.SetFloat("XDirection", moveDirection.x);
            playerAnimator.SetFloat("ZDirection", moveDirection.z);
            rb.MovePosition(rb.position + movementAxis.normalized * Time.fixedDeltaTime * moveSpeed);
        }

        LookAtMousePos();

        // call projectile spawning code in FixedUpdate to help sync with animator
        if (spawningProjectile)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * Vector3.Distance(projectileSpawnLocation.transform.position, Camera.main.transform.position));
            mousePos.y = projectileSpawnLocation.transform.position.y;
            projectiles.SpawnAndLookAt(projectileSpawnLocation.transform.position, mousePos);

            spawningProjectile = false;
        }
    }

    private void LateUpdate()
    {
        // Rotate arm to correct animation when shooting 
        if (canShoot && playerAnimator.GetCurrentAnimatorStateInfo(1).IsName("ShootRevolver"))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * Vector3.Distance(transform.position, Camera.main.transform.position));
            mousePos.y = torso.transform.position.y; //+ 2.2f; // height offset to make sure arm faces forward
            Vector3 lookDir = (mousePos - new Vector3(transform.position.x, torso.transform.position.y, transform.position.z)).normalized;
            // transform.forward = lookDir;
            //torso.transform.Rotate(Vector3.up, Vector3.Angle(-torso.transform.up, lookDir), Space.World);
            //lElbow.transform.Rotate(Vector3.up, Vector3.Angle(lElbow.transform.right, lookDir), Space.World);
            //lClavicle.transform.forward = -lookDir;
            // lElbow.transform.right = (mousePos - new Vector3(lElbow.transform.position.x, torso.transform.position.y - 2.2f, lElbow.transform.position.z)).normalized;
           // lHand.transform.Rotate(Vector3.up, Vector3.Angle(lHand.transform.right, lookDir), Space.World);
            //Quaternion lookRot = Quaternion.LookRotation(lookDir, Vector3.up);

            //lElbow.transform.rotation = Quaternion.RotateTowards(lElbow.transform.rotation, lookRot, 90f);

            //lElbow.transform.Rotate(lElbow.transform.forward, -60);
        }

        if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Roll"))
        {
            if (movementAxis.x != 0.0f || movementAxis.z != 0.0f)
                transform.forward = movementAxis;
            else
                movementAxis = transform.forward;
            rb.AddForce(movementAxis.normalized * 10, ForceMode.Acceleration);
        }
    }
}
