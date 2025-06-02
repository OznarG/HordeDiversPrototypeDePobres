using UnityEngine;

public class FarmeableItem : MonoBehaviour
{
    [SerializeField]  float health;
    [SerializeField] GameObject itemToDrop;
    public ItemType damagableBy;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void TakeDamage(float _damage)
    {
        health -= _damage;
        if(health <= 0 )
        {
            Destroy(gameObject);
        }
    }
}
