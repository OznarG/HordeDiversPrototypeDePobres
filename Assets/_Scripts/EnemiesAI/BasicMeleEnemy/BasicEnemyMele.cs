using UnityEngine;
using UnityEngine.AI;
using BehaviorTreeMila;
using BasicEnemyMeleMissions;
using System.Collections.Generic;
using System.Collections;

public class BasicEnemyMele : BehaviorTreeMila.Tree
{
    [Header("--- References ---")]
    public NavMeshAgent agent;
    public Animator anim;
    public CharacterStats characterStats;
    [SerializeField] Transform[] waypoints;
    [SerializeField] GameObject damageSource;

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
    // BehaviourTree tree;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        damageSource.GetComponent<DamagePlayerOntrigger>().SetDamage(1);

        /*//BASIC TREE EXAMPLE
        tree = new BehaviourTree("Enemy");
        //tree.AddChilld(new Leaf("Patrol", new PatrolStrategy(transform, agent, waypoints)));
        Leaf isTreasurePresent = new Leaf("IsTreasurePresent", new Condition(() => treasure.activeSelf));
        Leaf moveToTreasure = new Leaf("moveToTreasure", new ActionStrategy(() => agent.SetDestination(treasure.transform.position)));
        Sequence goToTreasure = new Sequence("GoToTreasure", 40);
        goToTreasure.AddChilld(isTreasurePresent);
        goToTreasure.AddChilld(moveToTreasure);
        Sequence goToTreasure2 = new Sequence("GoToSequence2", 20);
        goToTreasure2.AddChilld(new Leaf("IsTreasure2Present", new Condition(() => treasure2.activeSelf)));
        goToTreasure2.AddChilld(new Leaf("moveToTreasure2", new ActionStrategy(() => agent.SetDestination(treasure2.transform.position))));
        //Selector goToTreasures = new Selector("GoToTreasures");
        PrioritySelector goToTreasures = new PrioritySelector("GoToTreasures");
        goToTreasures.AddChilld(goToTreasure2);
        goToTreasures.AddChilld(goToTreasure);
        tree.AddChilld(goToTreasures);
        //tree = new BehaviourTree("Enemy");
        //PrioritySelector actions = new PrioritySelector("Agent Logic");
        //Sequence runToSafetySeq = new Sequence("RunToSafety", 100);
        //bool IsSafe()
        //{
        //    if (!inDanger)
        //    {
        //        runToSafetySeq.Reset();
        //        return false;
        //    }          
        //    return true;
        //}
        //runToSafetySeq.AddChild(new Leaf("isSafe?", new Condition(IsSafe)));
        //runToSafetySeq.AddChild(new Leaf("Go To Safety", new MoveToTargetSafe(transform, agent, safeSpot.transform, inDanger)));
        //actions.AddChild(runToSafetySeq);
        //Selector goToTreasure = new RandomSelector("GoToTreasure", 50);
        //Sequence getTreasure1 = new Sequence("GetTreasure1");
        //getTreasure1.AddChild(new Leaf("isTreasure1?", new Condition(() => treasure.activeSelf)));
        //getTreasure1.AddChild(new Leaf("GoToTreasure1", new MoveToTarget(transform, agent, treasure.transform)));
        //getTreasure1.AddChild(new Leaf("PickUpTreasure1", new ActionStrategy(() => treasure.SetActive(false))));
        //goToTreasure.AddChild(getTreasure1);
        //Sequence getTreasure2 = new Sequence("GetTreasure2");
        //getTreasure2.AddChild(new Leaf("isTreasure2?", new Condition(() => treasure2.activeSelf)));
        //getTreasure2.AddChild(new Leaf("GoToTreasure2", new MoveToTarget(transform, agent, treasure2.transform)));
        //getTreasure2.AddChild(new Leaf("PickUpTreasure2", new ActionStrategy(() => treasure2.SetActive(false))));
        //goToTreasure.AddChild(getTreasure2);
        //actions.AddChild(goToTreasure);
        //Leaf patrol = new Leaf("Patrol", new PatrolStrategy(transform, agent, waypoints));
        //actions.AddChild(patrol);
        //tree.AddChild(actions);*/


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
        if(stopAgent)
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
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
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
        if(onAttackCoolDown)
        {

        }
        else
        {
            onAttackCoolDown = true;
            anim.SetTrigger("attack");
            


        }

    }
}
