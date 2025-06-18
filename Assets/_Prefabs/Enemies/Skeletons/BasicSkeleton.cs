using UnityEngine;
using BasicEnemyDefaultSkeletonMissions;
using System.Collections.Generic;
using BehaviorTreeMila;
using System;
using VFolders.Libs;

public class BasicSkeleton : BehaviorTreeMila.Tree
{
    [Header("--- References ---")]
    public CharacterStats characterStats;
    [SerializeField] GameObject damageSource;
    [SerializeField] DamagePlayerOntrigger damageSourceScrpt;
    [SerializeField] FatherSpawner fatherSpawner;
    public Transform lockInTarget;
    [SerializeField] Collider colliderCur;

    [SerializeField] bool stopAgent;
    private Vector3 playerDir;
    [SerializeField] Transform headPos;
    internal object anim;
    bool playerIn;
    protected override void Start()
    {
        base.Start();
        fatherSpawner = GetComponent<FatherSpawner>();

        damageSourceScrpt.SetDamage(characterStats.damage);

        characterStats.actionDamageAnimation = DamageAnimation;
        characterStats.actionDeadAnimation = DieAnimation;
    }

    protected override BehaviorTreeMila.Node SetupTree()
    {

        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>{new EnemyDead(this), new DestroyEnemy(this)}),
            new Sequence(new List<Node>{new IsInAttackRange(this), new AttackPlayer(this)}),
            new Sequence(new List<Node> { new CheckEnemyInChaseRange(this),new ChasePlayer(this),}),
            new TaskPatrol(transform, fatherSpawner.transforms, characterStats.agent),

        });

        return root;
    }

    protected override void Update()
    {
        base.Update();
        //THERE SHOULD BE A BETTER WAY OF DOING THIS
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
            }
        }
    }

    #region HelperFunctions
    public void facePlayer()
    {
        playerDir = gameManager.instance.thirdPersonPlayerController.transform.position - headPos.position;
        playerDir.y = 0;
        Quaternion rot = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * characterStats.playerFaceSpeed);
    }
    //FUNCTIONS TO HELP FIND IF CHASE THE PLAYER // NEEDS TO BE UPDATED
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !playerIn)
        {
            characterStats.playerInRange = true;
            playerIn = true;
            gameManager.instance.thirdPersonPlayerController.enemiesInRange.Add(lockInTarget);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && playerIn)
        {
            characterStats.playerInRange = false;
            playerIn = false;
            gameManager.instance.thirdPersonPlayerController.enemiesInRange.Remove(lockInTarget);

        }
    }
    #endregion
    #region Animation Methods
    public void IsTakingDamage()
    {
        characterStats.isTakingDamage = true;        
        RestrictMovement();
    }
    public void DoneTakingDamage()
    {
        characterStats.isTakingDamage = false;
        AllowMovement();
    }
    public void IsAttaking()
    {
        characterStats.isAttacking = true;
        stopAgent = true;
    }
    public void IsNotAttacking()
    {
        characterStats.isAttacking = false;
        stopAgent = false;
        characterStats.attackCounter = 0;
        characterStats.onAttackCoolDown = true;
    }
    public void EnemyDead()
    {
        Destroy(gameObject, 10);
    }
    public void ActivateDamage()
    {
        damageSource.SetActive(true);
    }
    public void DesactivateDamage()
    {
        damageSource.SetActive(false);
    }
    public void RestrictMovement()
    {
        stopAgent = true;
    }
    public void AllowMovement()
    {
        stopAgent = false;
    }
    public void DamageAnimation()
    {
        characterStats.animator.SetTrigger("Hit");
    }
    public void DieAnimation()
    {
        characterStats.animator.SetTrigger("Dead");
        Destroy(gameObject, 10);
        colliderCur.Destroy();
    }
    
    #endregion

    public void SetCountToTrue()
    {
        characterStats.onAttackCoolDown = true;
    }
    //NEED TO RESET THIS
    public void AttackMelle()
    {
        if (characterStats.onAttackCoolDown)
        {

        }
        else
        {
            characterStats.animator.SetFloat("attackSpeed", characterStats.attackSpeed);
            characterStats.onAttackCoolDown = true;
            characterStats.animator.SetTrigger("Attack");
        }

    }
}
