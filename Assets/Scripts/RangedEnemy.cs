using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;

[RequireComponent(typeof(ObjectPoolHandler))]
public class RangedEnemy : BasicEnemyAI
{
    ObjectPoolHandler projectiles;
    [SerializeField] GameObject projectileSpawnLocation;
    [SerializeField] ParticleSystem muzzleEffect;
    float shootTimer = 0.0f;
    float fireRate = 1.0f;
    float playerYOffset = 1.0f; // want projectiles to target center of player, not root

    IKGunAim aimer;
    bool spawningProjectile = false;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        projectiles = GetComponent<ObjectPoolHandler>();
        aimer = GetComponent<IKGunAim>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    private void FixedUpdate()
    {
        aimer.target = player.transform.position;

        if (spawningProjectile)
        {
            spawningProjectile = false;
            projectiles.SpawnAndLookAt(projectileSpawnLocation.transform.position, player.transform.position + new Vector3(0.0f, playerYOffset, 0.0f));
        }
    }

    [Task]
    bool Shoot()
    {
        shootTimer += Time.deltaTime;

        if (shootTimer >= fireRate)
        {
            animator.SetTrigger("Shoot");
            if (muzzleEffect)
                muzzleEffect.Play();
            if (!spawningProjectile)
                spawningProjectile = true;
            shootTimer = 0.0f;
            return true;
        }
        return false;
    }
}
