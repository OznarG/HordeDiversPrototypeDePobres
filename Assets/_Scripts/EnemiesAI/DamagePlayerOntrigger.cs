using UnityEngine;

public class DamagePlayerOntrigger : MonoBehaviour
{
    [SerializeField] float _damage;

    public void SetDamage(float damage)
    {
        _damage = damage;
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if(other.CompareTag("Player"))
        {
            Debug.Log("Player was hit");
            gameManager.instance.thirdPersonPlayerController.TakeDamage(_damage);
        }
    }
}
