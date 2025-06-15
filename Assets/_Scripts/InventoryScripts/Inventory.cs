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

    public bool AddItem(Item stats, int amount = 0)
    {
        Slot invSlot;
        for (int i = 0; i < slotAmount; i++)
        {
            invSlot = slots[i].GetComponent<Slot>();
            //Add the passed Item to the next empty Slot
            if (invSlot.GetItemStackMax() == 0)
            {
                //NUNCA DEBO DE ANADIRMAS DE LO QUE SE DEBE, PORQUE SE CRASHEA?
                //int fitAmount = (invSlot.GetItemStackMax() - invSlot.GetItemStackAmount());
                ////amount -= fitAmount;
                //if (amount > fitAmount)
                //{
                //    AddItem(stats, amount - fitAmount);
                //}
                //else
                //{
                //    fitAmount = amount;
                //}
                invSlot.IncrementStackBy(amount);
                invSlot.AddItemToSlot(stats.ID, stats.type, stats.itemName, stats.description, stats.stackMax, stats.icon, stats.itemPrefab, stats.amountToAdd, stats.usable, stats.slotType, stats.weaponLevel, stats.damage, stats.strength, stats.speed, stats._name, stats.index);
                invSlot.UpdateSlot();
                gameManager.instance.inventoryAud.PlayOneShot(gameManager.instance.pickup);
                if(itemsOnHand.ContainsKey(stats.itemName))
                {
                    itemsOnHand[stats.itemName] += amount;
                }
                else
                {
                    itemsOnHand.Add(stats.itemName, amount);
                }
                return true;
            }
            //if the slot has the same item and it has space, add one //THIS IS ASSUMING ALL ITEMS WE PICK HAS ONE ONLY
            //We need to updated to take more than one 
            else if (invSlot.GetID() == stats.ID && invSlot.GetItemStackAmount() < stats.stackMax)
            {
                int fitAmount = (invSlot.GetItemStackMax() - invSlot.GetItemStackAmount());
                //amount -= fitAmount;
                if (amount > fitAmount)
                {
                    invSlot.IncrementStackBy(fitAmount);
                    invSlot.UpdateSlot();
                    //gameManager.instance.inventoryAud.PlayOneShot(gameManager.instance.pickup);
                    if (itemsOnHand.ContainsKey(stats.itemName))
                    {
                        itemsOnHand[stats.itemName] += fitAmount;
                    }
                    else
                    {
                        itemsOnHand.Add(stats.itemName, fitAmount);
                    }
                    AddItem(stats, amount - fitAmount);
                }
                else
                {
                    fitAmount = amount;
                    invSlot.IncrementStackBy(fitAmount);
                    invSlot.UpdateSlot();
                    //gameManager.instance.inventoryAud.PlayOneShot(gameManager.instance.pickup);
                    if (itemsOnHand.ContainsKey(stats.itemName))
                    {
                        itemsOnHand[stats.itemName] += fitAmount;
                    }
                    else
                    {
                        itemsOnHand.Add(stats.itemName, fitAmount);
                    }
                }

                return true;
            }
        }
        return false;
    }
    public void HoldItem(string item)
    {
        Slot invSlot = new Slot();
        Slot temp;
        for (int i = 0; i < slotAmount; i++)
        {
            invSlot.CopySlot(slots[i].GetComponent<Slot>());
            temp = slots[i].GetComponent<Slot>();
            string name = invSlot.GetItemName();
            if (name == item)
            {
                //Can try to hard code this iusing invSlot instead
                itemsInUse.Add(invSlot);
                temp.DecrementStackBy(1);
                temp.UpdateSlot();
                break;
            }
        }
    }

    public void RemoveItem(string item)
    {
        Slot invSlot;
        for (int i = 0; i < slotAmount; i++)
        {
            invSlot = slots[i].GetComponent<Slot>();
            string name = invSlot.GetItemName();
            if (name == item)
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
