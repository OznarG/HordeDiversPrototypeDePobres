using UnityEngine;

public class FarmeableItem : MonoBehaviour
{
    [SerializeField]  float health;
    [SerializeField] Item itemToDrop;
    public ItemType damagableBy;
    [SerializeField] int indexTool;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void TakeDamage(float _damage)
    {
        health -= _damage;
        if(health <= 0 )
        {
            itemToDrop.amountToAdd = 5;
            gameManager.instance.playerInventoryScript.AddItem(itemToDrop);
            Destroy(gameObject, 5);
        }
    }
}
