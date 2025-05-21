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

    [Header("--- Movement Stats ---")]
    public float playerFaceSpeed;

    [Header("--- Character States ---")]
    public bool isDead;
    public bool inDanger;
    public bool onAttackCoolDown;
    public bool isAttacking;
    public bool playerInRange;
    public bool isTakingDamage;


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
                animator.SetTrigger("damage");
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
