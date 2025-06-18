using UnityEngine;

public class DamagePlayerOntrigger : MonoBehaviour
{
    [SerializeField] float _damage;
    bool playerIn;

    public void SetDamage(float damage)
    {
        _damage = damage;
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if(other.CompareTag("Player") && !playerIn)
        {
            //Debug.Log("Player was hit");
            gameManager.instance.thirdPersonPlayerController.TakeDamage(_damage);
            
            playerIn = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            playerIn = false;
        }
    }
}
