using UnityEngine;

public class ToolOz : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        FarmingLand target = other.gameObject.GetComponent<FarmingLand>();
        if (other != null && target != null)
        {
            
            if (target != null)
            {
                //target.TakeDamage(damage);
                target.GetLandReady();

            }
            else
            {
                Debug.Log("nothing farming land");
            }
        }
    }
}
