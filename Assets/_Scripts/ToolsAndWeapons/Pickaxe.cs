using UnityEngine;

public class Pickaxe : MonoBehaviour
{
    [SerializeField] float damage;

    private void OnTriggerEnter(Collider other)
    {
        FarmeableItem farmeableItem = other.GetComponent<FarmeableItem>();
        Debug.Log("Stone was hit"+ farmeableItem);                                                              
        if(farmeableItem != null )
        {
            //NEED TO CHANGE THIS TO PICKAXE ---------------------------------<=================================>----------------
            if(farmeableItem.damagableBy == ItemType.Tool)
            {
                farmeableItem.TakeDamage(damage, gameManager.instance.thirdPersonPlayerController.weaponIndex);
            }
        }
    }
}
