using UnityEngine;

public class PlayerDamageSource : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        CharacterStats stats = other.GetComponent<CharacterStats>();
        if(stats != null )
        {
            float damage = gameManager.instance.thirdPersonPlayerController.meleDamage;
            if (gameManager.instance.thirdPersonPlayerController.attacking)
            {
                stats.TakeDamage(damage);
            }
            
        }
    }
}
