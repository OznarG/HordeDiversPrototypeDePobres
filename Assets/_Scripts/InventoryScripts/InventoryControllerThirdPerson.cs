using UnityEngine;

public class InventoryControllerThirdPerson : MonoBehaviour
{
    private void Update()
    {
        //NEED TO MAKE THIS FOR BOTH, PLAYERCONTROLLER AND THIRD PERSON
        //if (Input.GetAxis("Mouse ScrollWheel") > 0 && gameManager.instance.thirdPersonPlayerController.currentBeltSelection < gameManager.instance.thirdPersonPlayerController.beltAmount - 1 && !gameManager.instance.isPaused)
        //{
        //    //Set current selected to false and move forward one activating the next as active 
        //    gameManager.instance.selectedSlot.GetComponentInParent<SlotBackground>().selected = false;
        //    gameManager.instance.selectedSlot.GetComponentInParent<SlotBackground>().UpdateSelection();
        //    gameManager.instance.thirdPersonPlayerController.currentBeltSelection++;
        //    gameManager.instance.selectedSlot = gameManager.instance.thirdPersonPlayerController.playerBelt[gameManager.instance.thirdPersonPlayerController.currentBeltSelection];
        //    gameManager.instance.selectedSlot.GetComponentInParent<SlotBackground>().selected = true;
        //    gameManager.instance.selectedSlot.GetComponentInParent<SlotBackground>().UpdateSelection();
        //}
        //else if (Input.GetAxis("Mouse ScrollWheel") < 0 && gameManager.instance.thirdPersonPlayerController.currentBeltSelection > 0 && !gameManager.instance.isPaused)
        //{
        //    gameManager.instance.selectedSlot.GetComponentInParent<SlotBackground>().selected = false;
        //    gameManager.instance.selectedSlot.GetComponentInParent<SlotBackground>().UpdateSelection();
        //    gameManager.instance.thirdPersonPlayerController.currentBeltSelection--;
        //    gameManager.instance.selectedSlot = gameManager.instance.thirdPersonPlayerController.playerBelt[gameManager.instance.thirdPersonPlayerController.currentBeltSelection];
        //    gameManager.instance.selectedSlot.GetComponentInParent<SlotBackground>().selected = true;
        //    gameManager.instance.selectedSlot.GetComponentInParent<SlotBackground>().UpdateSelection();
        //}
        if(Input.GetKeyUp(KeyCode.Alpha1)) 
        {
            gameManager.instance.thirdPersonPlayerController.SelectEquipOne();
        }
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {

        }
    }

}
