using UnityEngine;

public class FarmeableItem : MonoBehaviour
{
    [SerializeField] AudioClip clip;
    [SerializeField]  float health;
    [SerializeField] Item itemToDrop;
    public ItemType damagableBy;
    [SerializeField] int indexDamageTool;
    [SerializeField] int dropAmount;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void TakeDamage(float _damage, int index)
    {
        if(indexDamageTool == index)
        {
            AudioManager.instance.PlaySFX(clip);
            health -= _damage;
            if(health <= 0 )
            {
                itemToDrop.amountToAdd = 5;
                gameManager.instance.playerInventoryScript.AddItem(itemToDrop, itemToDrop.amountToAdd + dropAmount);
                Destroy(gameObject, 5);
            }
        }

    }
}
