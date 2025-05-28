using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class SlotBackground : MonoBehaviour, IDropHandler
{
    [SerializeField] private Slot child;
    [SerializeField] SlotType slotTypeTaker;
    public int SlotID;
    public bool selected;
    [SerializeField] bool specialSlot;
    [SerializeField] Color slotColor;

    private void Awake()
    {
       
        
        selected = false;
        child = transform.GetComponentInChildren<Slot>();
    }
    public void OnDrop(PointerEventData eventData)
    {
        
        //Get the slot component of the image that the Cursor is grabbing, then calculate how much free space to stack it has
        Slot sourceSlot = eventData.pointerDrag.GetComponent<Slot>();
        if (specialSlot)
        {
            if (slotTypeTaker == sourceSlot.GetSlotType()) 
            {
                int freeSpace = child.GetFreeSpace();
                //If the item you grabing is equals to the item below it
                if (child.GetID() == sourceSlot.GetID())
                {
                    //If it can hold all items that were grabbed
                    if (freeSpace >= sourceSlot.GetItemStackAmount())
                    {
                        //Add all stock amount, delete item from source slot, Update the Slot(run checks etc), Update the source slote too
                        child.IncrementStackBy(sourceSlot.GetItemStackAmount());
                        sourceSlot.DecrementStackBy(sourceSlot.GetItemStackAmount());
                        child.UpdateSlot();
                        sourceSlot.UpdateSlot();
                    }
                    //If not all items fit
                    else
                    {
                        //Fill it to max, subtract what you place on the child slot from the source slot, update both slots
                        child.IncrementStackBy(freeSpace);
                        sourceSlot.DecrementStackBy(freeSpace);
                        child.UpdateSlot();
                        sourceSlot.UpdateSlot();
                    }
                }
                //If the items are not the same
                else
                {
                    //Swap items location
                    SwitchItemsLocation(sourceSlot);
                }                           
                                  
            }
        }
        else
        {           
            int freeSpace = child.GetFreeSpace();
            //If the item you grabing is equals to the item below it
            if (child.GetID() == sourceSlot.GetID())
            {
                //If it can hold all items that were grabbed
                if (freeSpace >= sourceSlot.GetItemStackAmount())
                {
                    //Add all stock amount, delete item from source slot, Update the Slot(run checks etc), Update the source slote too
                    child.IncrementStackBy(sourceSlot.GetItemStackAmount());
                    sourceSlot.DecrementStackBy(sourceSlot.GetItemStackAmount());
                    child.UpdateSlot();
                    sourceSlot.UpdateSlot();
                }
                //If not all items fit
                else
                {
                    //Fill it to max, subtract what you place on the child slot from the source slot, update both slots
                    child.IncrementStackBy(freeSpace);
                    sourceSlot.DecrementStackBy(freeSpace);
                    child.UpdateSlot();
                    sourceSlot.UpdateSlot();
                }
            }
            //If the items are not the same
            else
            {
                //Swap items location
                SwitchItemsLocation(sourceSlot);
            }
            
        }
        //Maybe needs better why of doing this 
        gameManager.instance.thirdPersonPlayerController.UpdateEquipSlot();

    }
    private void SwitchItemsLocation(Slot sourceSlot) /* NEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE BACK HERE    */
    {
        //NEED TO FIND OUT WHY THIS DIDN'T WORKED AS I EXPECTED

        // Create a temporary Slot to add the child information
        // (cannot call new on objects inheriting from MonoBehavior, so no copy constructor)
        int tempID = child.GetID();
        ItemType tempType = child.GetItemType();
        string tempItemName = child.GetItemName();
        string tempDescription = child.GetItemDescription();
        int tempStackMax = child.GetItemStackMax();
        int tempStackAmount = child.GetItemStackAmount();
        Sprite tempIcon = child.GetItemIcon();
        GameObject tempItemPrefab = child.GetItemPrefab();
        SlotType slotType = child.GetSlotType();

        //ADD IF IS A WEAPON HERE ===================================================< >
        int tempweaponLevel = child.GetWeaponLevel();
        float tempdamage = child.GetWeaponDamage();
        float tempstrength = child.GetWeaponStrength();
        float tempspeed = child.GetWeaponSpeed();
        string temp_name = child.GetWeaponName();
        int tempindex = child.GetWeaponIndex();

        //Slot temp = child;

        
        //Set this child information to the source slot
        child.SetItemID(sourceSlot.GetID());
        child.SetItemType(sourceSlot.GetItemType());
        child.SetItemName(sourceSlot.GetItemName());
        child.SetItemDescription(sourceSlot.GetItemDescription());
        child.SetItemStackMax(sourceSlot.GetItemStackMax());
        child.SetItemStackAmount(sourceSlot.GetItemStackAmount());
        child.SetItemIcon(sourceSlot.GetItemIcon());
        child.SetItemPrefab(sourceSlot.GetItemPrefab()); 
        child.SetSlotType(sourceSlot.GetSlotType());
        //child = sourceSlot;

        //ADD IF IS A WEAPON HERE ===================================================< >
        child.SetWeaponLevel(sourceSlot.GetWeaponLevel());
        child.SetWeaponDamage(sourceSlot.GetWeaponDamage());
        child.SetWeaponStrength(sourceSlot.GetWeaponStrength());
        child.SetWeaponSpeed(sourceSlot.GetWeaponSpeed());
        child.SetWeaponName(sourceSlot.GetWeaponName());
        child.SetWeaponIndex(sourceSlot.GetWeaponIndex());


        //Set the source slot to the temporary slot from the child
        sourceSlot.SetItemID(tempID);
        sourceSlot.SetItemType(tempType);
        sourceSlot.SetItemName(tempItemName);
        sourceSlot.SetItemDescription(tempDescription);
        sourceSlot.SetItemStackMax(tempStackMax);
        sourceSlot.SetItemStackAmount(tempStackAmount);
        sourceSlot.SetItemIcon(tempIcon);
        sourceSlot.SetItemPrefab(tempItemPrefab);
        sourceSlot.SetSlotType(slotType);
        //sourceSlot = temp;
        //ADD IF IS A WEAPON HERE ===================================================< >
        sourceSlot.SetWeaponLevel(tempweaponLevel);
        sourceSlot.SetWeaponDamage(tempdamage);
        sourceSlot.SetWeaponStrength(tempstrength);
        sourceSlot.SetWeaponSpeed(tempspeed);
        sourceSlot.SetWeaponName(temp_name);
        sourceSlot.SetWeaponIndex(tempindex);

        //Update both Slot
        child.UpdateSlot();
        sourceSlot.UpdateSlot();
    }
    public void UpdateSelection()   //NEED TO MAKE THIS FOR BOTH, PLAYERCONTROLLER AND THIRD PERSON
    {
        //if you check and select an item
        if (gameManager.instance.selectedSlot.GetComponentInParent<SlotBackground>().selected)
        {
            // THIS NEED IMPLEMENTATION TOO            -----------------------------------<=====================>
            //set this SlotBacground color to red
            transform.GetComponent<Image>().color = Color.red;
            //Add description into the description area
            //gameManager.instance.playerScript.itemDescription.text =
            //   "Descrition: \n" + child.GetItemDescription();
            if (WeaponOnHand())
            {
                gameManager.instance.thirdPersonPlayerController.currentWeapon.SetActive(false);
                int indexWeapon = gameManager.instance.selectedSlot.GetComponentInParent<SlotBackground>().GetComponentInChildren<Slot>().GetID();
                gameManager.instance.thirdPersonPlayerController.currentWeapon = gameManager.instance.thirdPersonPlayerController.playerWeapons[indexWeapon];
                gameManager.instance.thirdPersonPlayerController.currentWeapon.SetActive(true);
                Debug.Log("SE supone que se active el " + gameManager.instance.thirdPersonPlayerController.currentWeapon);
            }
        }
        else
        {
            transform.GetComponent<Image>().color = slotColor;
            //if is not selected do nothing, Set it white
            gameManager.instance.thirdPersonPlayerController.currentWeapon.SetActive(false);
            
        }

           
    }

    bool WeaponOnHand()
    {
        ItemType item = gameManager.instance.selectedSlot.GetComponentInParent<SlotBackground>().GetComponentInChildren<Slot>().GetItemType();
        if (item == ItemType.MeleWeapon || item == ItemType.Weapon || item == ItemType.Tool)
        {
            return true;
        }
        return false;
    }
}
