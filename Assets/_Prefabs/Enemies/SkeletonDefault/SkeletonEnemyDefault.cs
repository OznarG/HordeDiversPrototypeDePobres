using BasicEnemySkeletonMissions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorTreeMila;

public class SkeletonEnemyDefault : BehaviorTreeMila.Tree
{
    [Header("--- References ---")]
    public CharacterStats characterStats;
    [SerializeField] Transform[] waypoints;
    [SerializeField] GameObject damageSource;
    [SerializeField] DamagePlayerOntrigger damageSourceScrpt;
    private FatherSpawner fatherSpawner;

    [SerializeField] bool stopAgent;
    private Vector3 playerDir;
    [SerializeField] Transform headPos;

    protected override void Start()
    {
        base.Start();
        fatherSpawner = GetComponent<FatherSpawner>();
        if(fatherSpawner != null )
        {
            waypoints = new Transform[8];
            for (int i = 0; i < waypoints.Length; i++)
            {
                waypoints[i] = fatherSpawner.GetTransform(i);
            }
        }

        characterStats.agent = GetComponent<NavMeshAgent>();
        characterStats.animator = GetComponentInChildren<Animator>();
        damageSourceScrpt.SetDamage(10);
    }
    protected override BehaviorTreeMila.Node SetupTree()
    {

        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>{new EnemyDead(this), new DestroyEnemy(this)}),
            new Sequence(new List<Node>{new IsInAttackRange(this), new AttackPlayer(this)}),
            new Sequence(new List<Node> { new CheckEnemyInChaseRange(this),new ChasePlayer(this),}),
            new TaskPatrol(transform, waypoints, characterStats.agent),

        });

        return root;
    }
    protected override void Update()
    {
        base.Update();
        if (stopAgent)
        {
            characterStats.agent.enabled = false;
        }
        else
        {
            characterStats.agent.enabled = true;
        }
        characterStats.animator.SetFloat("Speed", characterStats.agent.velocity.magnitude);
        if (characterStats.onAttackCoolDown)
        {
            characterStats.attackCounter += Time.deltaTime;
            if (characterStats.attackCounter >= characterStats.attackRate)
            {
                characterStats.onAttackCoolDown = false;
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
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * characterStats.playerFaceSpeed);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            characterStats.playerInRange = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            characterStats.playerInRange = false;
        }
    }

    //Anim and condition Functions for trees
    public void IsTakingDamage()
    {
        characterStats.isTakingDamage = true;
    }
    public void DoneTakingDamage()
    {
        characterStats.isTakingDamage = false;
    }
    public void IsAttaking()
    {
        characterStats.isAttacking = true;
        damageSource.SetActive(true);
    }
    public void IsNotAttacking()
    {
        characterStats.isAttacking = true;
        damageSource.SetActive(false);
        characterStats.attackCounter = 0;
        characterStats.onAttackCoolDown = true;
    }

    public void EnemyDead()
    {
        Destroy(gameObject, 10);
    }

    public void SetCountToTrue()
    {
        characterStats.onAttackCoolDown = true;
    }

    public void AttackMelle()
    {
        if (characterStats.onAttackCoolDown)
        {

        }
        else
        {
            characterStats.onAttackCoolDown = true;
            characterStats.animator.SetTrigger("attack");
        }

    }
}
