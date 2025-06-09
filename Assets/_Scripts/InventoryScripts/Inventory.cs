using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{
    [SerializeField] GameObject inventoryHolder;
    GameObject[] slots;
    public Dictionary<string, int> itemsOnHand;
    public List<Slot> itemsInUse;

    [Header("---Drag Stats---")]
    int slotAmount;
    int slotNumber;
    public TMP_Text itemDescription;
    public TMP_Text itemStats;
    public GameObject backButton;
    public bool isOpen;

    // Start is called before the first frame update
    void Awake()
    {
        //Get all the slot child of the slot holder and add them to a slot array
        slotAmount = inventoryHolder.transform.childCount;
        //Debug.Log("slot amunt is " + slotAmount);
        slots = new GameObject[slotAmount];
        for (int i = 0; i < slotAmount; i++)
        {
            //Set slot on array to slot in the SlotHolder
            slots[i] = inventoryHolder.transform.GetChild(i).GetChild(0).gameObject;
            slots[i].GetComponentInParent<SlotBackground>().SlotID = i;
        }
        itemsOnHand = new Dictionary<string, int>();
        itemsInUse = new List<Slot>();
    }

    public bool AddItem(Item stats)
    {
        Slot invSlot;
        for (int i = 0; i < slotAmount; i++)
        {
            invSlot = slots[i].GetComponent<Slot>();
            //Add the passed Item to the next empty Slot
            if (invSlot.GetItemStackMax() == 0)
            {
                invSlot.IncrementStackBy(1);
                invSlot.AddItemToSlot(stats.ID, stats.type, stats.itemName, stats.description, stats.stackMax, stats.icon, stats.itemPrefab, stats.amountToAdd, stats.usable, stats.slotType, stats.weaponLevel, stats.damage, stats.strength, stats.speed, stats._name, stats.index);
                invSlot.UpdateSlot();
                gameManager.instance.inventoryAud.PlayOneShot(gameManager.instance.pickup);
                if(itemsOnHand.ContainsKey(stats.itemName))
                {
                    itemsOnHand[stats.itemName] += 1;
                }
                else
                {
                    itemsOnHand.Add(stats.itemName, 1);
                }
                return true;
            }
            //if the slot has the same item and it has space, add one //THIS IS ASSUMING ALL ITEMS WE PICK HAS ONE ONLY
            //We need to updated to take more than one 
            else if (invSlot.GetID() == stats.ID && invSlot.GetItemStackAmount() < stats.stackMax)
            {
                invSlot.IncrementStackBy(1);
                invSlot.UpdateSlot();
                gameManager.instance.inventoryAud.PlayOneShot(gameManager.instance.pickup);
                if (itemsOnHand.ContainsKey(stats.itemName))
                {
                    itemsOnHand[stats.itemName] += 1;
                }
                else
                {
                    itemsOnHand.Add(stats.itemName, 1);
                }
                return true;
            }
        }
        return false;
    }
    public void HoldItem(string item)
    {
        Slot invSlot;
        for (int i = 0; i < slotAmount; i++)
        {
            invSlot = slots[i].GetComponent<Slot>();
            if (invSlot.GetItemName() == item)
            {
                itemsInUse.Add(invSlot);
            }
        }
    }

    public void RemoveItem(string item)
    {
        Slot invSlot;
        for (int i = 0; i < slotAmount; i++)
        {
            invSlot = slots[i].GetComponent<Slot>();
            if (invSlot.GetItemName() == item)
            {
                invSlot.DecrementStackBy(1);
                invSlot.UpdateSlot();
            }
        }
    }
    public void ReturnItem()
    {

    }
}
