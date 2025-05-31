using UnityEngine;

public class ToolSeed : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        FarmingLand target = other.gameObject.GetComponent<FarmingLand>();
        if (other != null && target != null)
        {
            
            if (target != null && target.IsReadyToPlant())
            {
                //target.TakeDamage(damage);
                Slot selected = gameManager.instance.selectedSlot.GetComponent<Slot>(); //NEED TO ADJUST THIS IS TOO EXPENSIVE
                if (SeedsReferences.instance.AddSeed(selected.GetID(), target))
                {
                    selected.DecrementStackBy(1);  

                    selected.UpdateSlot(); 
                }
                else
                {
                    Debug.Log("Is Growing Something already");
                }

            }
            else
            {
                Debug.Log("nothing farming land");
            }
        }
       // Debug.Log("Shotting");
    }
}
