using UnityEngine;

public class InventoryController : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && gameManager.instance.playerController.currentBeltSelection < gameManager.instance.playerController.beltAmount - 1 && !gameManager.instance.isPaused)
        {
            //Set current selected to false and move forward one activating the next as active 
            gameManager.instance.selectedSlot.GetComponentInParent<SlotBackground>().selected = false;
            gameManager.instance.selectedSlot.GetComponentInParent<SlotBackground>().UpdateSelection();
            gameManager.instance.playerController.currentBeltSelection++;
            gameManager.instance.selectedSlot = gameManager.instance.playerController.playerBelt[gameManager.instance.playerController.currentBeltSelection];
            gameManager.instance.selectedSlot.GetComponentInParent<SlotBackground>().selected = true;
            gameManager.instance.selectedSlot.GetComponentInParent<SlotBackground>().UpdateSelection();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && gameManager.instance.playerController.currentBeltSelection > 0 && !gameManager.instance.isPaused)
        {
            gameManager.instance.selectedSlot.GetComponentInParent<SlotBackground>().selected = false;
            gameManager.instance.selectedSlot.GetComponentInParent<SlotBackground>().UpdateSelection();
            gameManager.instance.playerController.currentBeltSelection--;
            gameManager.instance.selectedSlot = gameManager.instance.playerController.playerBelt[gameManager.instance.playerController.currentBeltSelection];
            gameManager.instance.selectedSlot.GetComponentInParent<SlotBackground>().selected = true;
            gameManager.instance.selectedSlot.GetComponentInParent<SlotBackground>().UpdateSelection();
        }
    }
    
}
