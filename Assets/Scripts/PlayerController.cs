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

    Plane mousePlane;

    [SerializeField] GameObject testCube;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        aimer = GetComponent<IKGunAim>();
        mousePlane = new Plane(Vector3.up, Vector3.zero);
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

        LookAtMousePos(GetMousePos());

        // call projectile spawning code in FixedUpdate to help sync with animator
        SpawnProjectile(GetMousePos());
    }

    private void LateUpdate()
    {
        if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Roll"))
        {
            if (movementAxis.x != 0.0f || movementAxis.z != 0.0f)
                transform.forward = movementAxis;
            else
                movementAxis = transform.forward;
            rb.AddForce(movementAxis.normalized * 10, ForceMode.Acceleration);
        }
    }

    void LookAtMousePos(Vector3 mousePos)
    {
        //Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * Vector3.Distance(transform.position, Camera.main.transform.position));
        //mousePos.y = transform.position.y;
        //mousePos.y = projectileSpawnLocation.transform.position.y;
        Vector3 lookDir = (mousePos - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(lookDir, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 5.5f * Time.deltaTime);

        testCube.transform.position = mousePos;

        // update IK aiming target
        aimer.target = mousePos;
    }

    Vector3 GetMousePos()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float ent = 100.0f;
        if (mousePlane.Raycast(ray, out ent))
        {
            return ray.GetPoint(ent);
        }
        return Vector3.zero;
    }

    private void SpawnProjectile(Vector3 target)
    {
        if (spawningProjectile)
        {
            target.z -= projectileSpawnLocation.transform.position.y - target.y;
            target.y = projectileSpawnLocation.transform.position.y;
            target.y += Random.Range(-0.2f, 0.2f);
            target.x += Random.Range(-0.2f, 0.2f);
            projectiles.SpawnAndLookAt(projectileSpawnLocation.transform.position, target);

            spawningProjectile = false;
        }
    }
}
