using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CharacterStats : MonoBehaviour, IDamage
{
    [Header("--- References ---")]
    public SkinnedMeshRenderer model;
    public Animator animator;
    public NavMeshAgent agent;

    [Header("--- Helpfull Variables ---")]
    public float attackCounter;

    [Header("--- Character Stats ---")]
    public int health;
    public int maxHealth;
    public float speed;
    public float attackRate;
    public float attackSpeed;
    public int attackType;
    public float damage;
    public float strongDamage;

    [Header("--- Movement Stats ---")]
    public float playerFaceSpeed;

    [Header("--- Character States ---")]
    public bool isDead;
    public bool inDanger;
    public bool onAttackCoolDown;
    public bool isAttacking;
    public bool playerInRange;
    public bool isTakingDamage;

    [Header("--- Drop / Loot ---")]
    public float xpPointDrop;

    //Delegates and functions variables?
    public Action actionDamageAnimation;
    public Action actionDeadAnimation;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }
    public virtual void Start()
    {
        maxHealth = 100;
        SetHealthTo(maxHealth);
        isDead = false;
    }
    public virtual void CheckHealth()
   {
       if(health <= 0)
       {
           health = 0;
           Die();
       }
       if(health >= maxHealth)
       {
           health = maxHealth;
       }
   }
   private void Die()
   {
       isDead = true;
        actionDeadAnimation?.Invoke();
        gameManager.instance.thirdPersonPlayerController.AddExp(xpPointDrop);
   }
   private void SetHealthTo(int healthToSetTo)
   {
        health = healthToSetTo;
        CheckHealth();
   }
   public void TakeDamage(float damage)
   {
        if(!isDead)
        {
            Debug.Log("Enemy took damage " + damage);
            int healthAfterDamage = health - ((int)damage);
            SetHealthTo(healthAfterDamage);
            if (model != null)
            {
                StartCoroutine(flashDamage(Color.blue));
            }
            if (animator != null && !isDead)
            {
                actionDamageAnimation?.Invoke();
            }
            
        }
        
   }
   public void Heal(int heal)
   {
        int healthAfterHeal = health + heal;
        SetHealthTo(healthAfterHeal);
    }

    public IEnumerator flashDamage(Color color)
    {
        model.material.color = color;
        yield return new WaitForSeconds(0.15f);
        model.material.color = Color.white;
    }

    public bool IsDead()
    {
        return isDead;
    }
}
