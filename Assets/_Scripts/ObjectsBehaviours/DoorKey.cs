using UnityEngine;

public class DoorKey : MonoBehaviour
{
    int code = 100;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AddToPlayer();
        }

    }
    public void AddToPlayer()
    {
        gameManager.instance.playerController.keys.Add(code);
        Destroy(gameObject);
    }
}
