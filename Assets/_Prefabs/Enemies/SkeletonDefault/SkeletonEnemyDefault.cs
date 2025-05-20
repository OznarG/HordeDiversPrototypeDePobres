using BasicEnemySkeletonMissions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorTreeMila;

public class SkeletonEnemyDefault : BehaviorTreeMila.Tree
{
    [Header("--- References ---")]
    public NavMeshAgent agent;
    public Animator anim;
    public CharacterStats characterStats;
    [SerializeField] Transform[] waypoints;
    [SerializeField] GameObject damageSource;
    [SerializeField] DamagePlayerOntrigger damageSourceScrpt;

    [Header("--- Booleans and States ---")]
    [SerializeField] float speed;
    [SerializeField] bool inDanger;

    public bool onAttackCoolDown;
    public bool isAttacking;
    [SerializeField] float attackCounter;

    public bool isDead;
    public bool playerInRange;
    public bool isTakingDamage;

    public float attackRate;


    [SerializeField] bool stopAgent;
    private Vector3 playerDir;
    [SerializeField] Transform headPos;
    [SerializeField] float playerFaceSpeed;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        damageSourceScrpt.SetDamage(10);
    }
    protected override BehaviorTreeMila.Node SetupTree()
    {

        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>{new EnemyDead(this), new DestroyEnemy(this)}),
            new Sequence(new List<Node>{new IsInAttackRange(this), new AttackPlayer(this)}),
            new Sequence(new List<Node> { new CheckEnemyInChaseRange(this),new ChasePlayer(this),}),
            new TaskPatrol(transform, waypoints, agent),

        });

        return root;
    }
    protected override void Update()
    {
        base.Update();
        if (stopAgent)
        {
            agent.enabled = false;
        }
        else
        {
            agent.enabled = true;
        }
        anim.SetFloat("Speed", agent.velocity.magnitude);
        if (onAttackCoolDown)
        {
            attackCounter += Time.deltaTime;
            if (attackCounter >= attackRate)
            {
                onAttackCoolDown = false;
                //_animator.SetBool("Walking", true);
            }
        }
        //Debug.Log(agent.remainingDistance);
    }
    public void facePlayer()
    {
        playerDir = gameManager.instance.thirdPersonPlayerController.transform.position - headPos.position;
        playerDir.y = 0;
        Quaternion rot = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * playerFaceSpeed);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    //Anim and condition Functions for trees
    public void IsTakingDamage()
    {
        isTakingDamage = true;
    }
    public void DoneTakingDamage()
    {
        isTakingDamage = false;
    }
    public void IsAttaking()
    {
        isAttacking = true;
        damageSource.SetActive(true);

    }
    public void IsNotAttacking()
    {
        isAttacking = true;
        damageSource.SetActive(false);
        attackCounter = 0;
        onAttackCoolDown = true;
    }

    public void EnemyDead()
    {
        Destroy(gameObject, 10);
    }

    public void SetCountToTrue()
    {
        onAttackCoolDown = true;
    }

    public void AttackMelle()
    {
        if (onAttackCoolDown)
        {

        }
        else
        {
            onAttackCoolDown = true;
            anim.SetTrigger("attack");



        }

    }
}
