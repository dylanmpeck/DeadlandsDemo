using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Panda;

public class BasicEnemyAI : MonoBehaviour
{
    [SerializeField] protected GameObject player;
    public float wanderRadius;
    public float wanderTimer;
    public float attackRange;

    protected NavMeshAgent agent;
    protected Animator animator;
    private float timer;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
        animator = GetComponent<Animator>();

        if (!player)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (agent.isStopped)
            animator.SetBool("Running", false);
        else
            animator.SetBool("Running", true);

        Vector3 moveDirection = transform.InverseTransformDirection(agent.velocity.normalized);
        animator.SetFloat("XDirection", moveDirection.x);
        animator.SetFloat("ZDirection", moveDirection.z);

        transform.forward = (player.transform.position - transform.position).normalized;
    }

    [Task]
    protected bool Moving()
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    agent.isStopped = true;
                    return false;
                }
            }
        }
        return true;
    }

    [Task]
    protected bool Stop()
    {
        agent.isStopped = true;
        return true;
    }

    [Task]
    protected bool InAttackRange()
    {
        if (!player)
            return false;

        if (Vector3.Distance(transform.position, player.transform.position) <= attackRange)
            return true;
        return false;
    }

    [Task]
    protected bool CanSeePlayer()
    {
        if (!player)
            return false;

        Vector3 dir = (player.transform.position - this.transform.position).normalized;
        // if there's an obstacle in between me and player, I can't see player
        if (Physics.Raycast(transform.position, dir, Vector3.Distance(transform.position, player.transform.position), 1 << LayerMask.NameToLayer("Obstacle")))
            return false;
        // TODO add a fov and sight distance
        return true;
    }

    [Task]
    protected bool Wander()
    {
        timer += Time.deltaTime;

        if (timer >= wanderTimer)
       {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            agent.isStopped = false;
            timer = 0;
            return true;
        }
        else
          return false;
    }

    [Task]
    protected bool MoveTowardsPlayer()
    {
        if (!player)
            return false;
        agent.SetDestination(player.transform.position);
        return true;
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
}
