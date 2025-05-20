using System.Collections;
using UnityEngine;

public class CharacterStats : MonoBehaviour, IDamage
{
    [SerializeField] SkinnedMeshRenderer model;

    [SerializeField] protected int health;
   [SerializeField] protected int maxHealth;

   [SerializeField] protected bool isDead;
    [SerializeField] Animator animator;
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
            if (model != null)
            {
                StartCoroutine(flashDamage(Color.blue));
            }
            if (animator != null)
            {
                animator.SetTrigger("damage");
            }
            Debug.Log("Enemy took damage " + damage);
            int healthAfterDamage = health - ((int)damage);
            SetHealthTo(healthAfterDamage);
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
