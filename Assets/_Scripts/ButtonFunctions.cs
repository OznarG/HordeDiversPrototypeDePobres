using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsFunctions : MonoBehaviour
{
    public bool craftMenuOpen;
    public GameObject falseMenu;

    public void Resume()
    {
        gameManager.instance.UnPauseGame();
    }

    public void BactToPauseMenu()
    {
        gameManager.instance.activeMenu.gameObject.SetActive(false);
        gameManager.instance.activeMenu = gameManager.instance.pauseMenu;
        gameManager.instance.activeMenu.gameObject.SetActive(true);
    }
    public void BactToPauseMenuFromIventory()
    {

        gameManager.instance.selectedSlot.GetComponentInParent<SlotBackground>().selected = false;
        gameManager.instance.selectedSlot.GetComponentInParent<SlotBackground>().UpdateSelection();
        gameManager.instance.selectedSlot = gameManager.instance.previuslySelectedSlot;
        gameManager.instance.selectedSlot.GetComponentInParent<SlotBackground>().selected = true;
        gameManager.instance.selectedSlot.GetComponentInParent<SlotBackground>().UpdateSelection();

        gameManager.instance.playerInventoryScript.isOpen = false;
        gameManager.instance.playerInventory.SetActive(false);

        gameManager.instance.activeMenu.gameObject.SetActive(false);
        gameManager.instance.activeMenu = gameManager.instance.pauseMenu;
        gameManager.instance.activeMenu.gameObject.SetActive(true);
    }
    public void OpenCharacterStats()
    {
        gameManager.instance.activeMenu.gameObject.SetActive(false);
        gameManager.instance.playerInventoryScript.isOpen = true;
        gameManager.instance.previuslySelectedSlot = gameManager.instance.selectedSlot;
        gameManager.instance.activeMenu = gameManager.instance.CharacterStatsMenu;
        gameManager.instance.activeMenu.gameObject.SetActive(true);
    }
    public void OpenCrafting()
    {
         

        gameManager.instance.activeMenu.gameObject.SetActive(false);     
        gameManager.instance.activeMenu = gameManager.instance.CraftingMenu;
        craftMenuOpen = true;
        //open Inventory and set as main
        gameManager.instance.playerInventory.SetActive(true);
        gameManager.instance.isPaused = true;
        gameManager.instance.playerInventoryScript.isOpen = true;
        //NEED TO IMPLEMENT THIS IF WE ADD A BACK BUTTON ON THE MENU  ---------<===>
        //instance.playerinventoryScrpt.backButton.SetActive(false);
        gameManager.instance.previuslySelectedSlot = gameManager.instance.selectedSlot;
        gameManager.instance.PauseGame();
        gameManager.instance.activeMenu.gameObject.SetActive(true);
    }
    public void OpenSkills()
    {
        gameManager.instance.activeMenu.gameObject.SetActive(false);
        gameManager.instance.activeMenu = gameManager.instance.skillsMenu;
        gameManager.instance.activeMenu.gameObject.SetActive(true);
    }
    public void OpenCollectibles()
    {
        gameManager.instance.activeMenu.gameObject.SetActive(false);
        gameManager.instance.activeMenu = gameManager.instance.CollectiblesMenu;
        gameManager.instance.activeMenu.gameObject.SetActive(true);
    }
    public void OpenInfo()
    {
        gameManager.instance.activeMenu.gameObject.SetActive(false);
        gameManager.instance.activeMenu = gameManager.instance.infoMenu;
        gameManager.instance.activeMenu.gameObject.SetActive(true);
    }
    public void OpenSettings()
    {
        gameManager.instance.activeMenu.gameObject.SetActive(false);
        gameManager.instance.activeMenu = gameManager.instance.settingsMenu;
        gameManager.instance.activeMenu.gameObject.SetActive(true);
    }
    public void OpenInventory()
    {
        gameManager.instance.activeMenu.gameObject.SetActive(false);
        gameManager.instance.activeMenu = gameManager.instance.playerInventory;
        gameManager.instance.activeMenu.gameObject.SetActive(true);

        gameManager.instance.playerInventoryScript.isOpen = true;
        gameManager.instance.previuslySelectedSlot = gameManager.instance.selectedSlot;
    }

    public void CloseInventory()
    {
        gameManager.instance.selectedSlot.GetComponentInParent<SlotBackground>().selected = false;
        gameManager.instance.selectedSlot.GetComponentInParent<SlotBackground>().UpdateSelection();
        gameManager.instance.selectedSlot = gameManager.instance.previuslySelectedSlot;
        gameManager.instance.selectedSlot.GetComponentInParent<SlotBackground>().selected = true;
        gameManager.instance.selectedSlot.GetComponentInParent<SlotBackground>().UpdateSelection();

        gameManager.instance.playerInventoryScript.isOpen = false;
        gameManager.instance.playerInventory.SetActive(false);
        gameManager.instance.activeMenu.SetActive(false);
        gameManager.instance.activeMenu = gameManager.instance.pauseMenu;
        gameManager.instance.activeMenu.gameObject.SetActive(true);
    }
    //public void OpenHabilitiesMenu()
    //{
    //    gameManager.instance.activeMenu.gameObject.SetActive(false);
    //    gameManager.instance.activeMenu = gameManager.instance.habilitiesMenu;
    //    gameManager.instance.activeMenu.gameObject.SetActive(true);
    //}

    //public void OpenSettingsMenu()
    //{
    //    gameManager.instance.activeMenu.gameObject.SetActive(false);
    //    gameManager.instance.activeMenu = gameManager.instance.settingsMenu;
    //    gameManager.instance.activeMenu.gameObject.SetActive(true);
    //}

    //public void OpenCollectibles()
    //{
    //    gameManager.instance.activeMenu.gameObject.SetActive(false);
    //    gameManager.instance.activeMenu = gameManager.instance.collectiblesMenu;
    //    gameManager.instance.activeMenu.gameObject.SetActive(true);
    //}

    //public void OpenCraftMenu()
    //{
    //    gameManager.instance.activeMenu.gameObject.SetActive(false);
    //    gameManager.instance.activeMenu = gameManager.instance.craftingMenu;
    //    gameManager.instance.activeMenu.gameObject.SetActive(true);
    //}
    public void PlayText()
    {
        
        if (gameManager.instance.textsToPlay[gameManager.instance.textIndex + 1] != " ")
        {
            gameManager.instance.textIndex++;
            gameManager.instance.chatText.text = gameManager.instance.textsToPlay[gameManager.instance.textIndex];

        }
        else
        {
            gameManager.instance.UnPauseGame();
            gameManager.instance.charWindow.SetActive(false);
            gameManager.instance.textIndex = 0;
        }
    }

}
