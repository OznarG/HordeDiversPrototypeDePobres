using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class Slot : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    //Slot information, it matches item information
    [Header("Item Information")]
    [SerializeField] int ID;
    [SerializeField] ItemType type;
    [SerializeField] SlotType slotType;
    [SerializeField] string itemName;
    [SerializeField] string description;
    [SerializeField] int stackMax;
    [SerializeField] int stackAmount;
    [SerializeField] Sprite icon;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] Sprite defaultImage;
    [SerializeField] int addAmount;
    [SerializeField] bool usable;
    [SerializeField] ItemWeaponStats weaponStats;

    [Header("Slot Information")]
    [SerializeField] bool selected;
    [SerializeField] Canvas canvas;
    bool isDragging;
    RectTransform rectTransform;
    Transform parentAfterDrag;

    private float nextTimeTofire = 0;

    private void Awake()
    {
        //Get transform of this item
        rectTransform = GetComponent<RectTransform>();
    }
    public void UseItem()
    {
        if(type == ItemType.Weapon)
        {
            //Need to store Gun script in the singleton to access without using getcomponen because is too expensive
            gameManager.instance.playerController.currentWeapon.GetComponent<Gun>().Shoot();
        }
        else if(type == ItemType.MeleWeapon)
        {
            //if (Time.time >= nextTimeTofire)
            //{
                
            //    nextTimeTofire = Time.time + 2;
            //    gameManager.instance.thirdPersonPlayerController.playerAnim.SetTrigger("Attack");
            //    Debug.Log("Sword Attacked");
            //}
            //Debug.Log("ATTACKE CON CUCHILLO HEHEHE BOY :V");
            
            if(!gameManager.instance.thirdPersonPlayerController.IsAttacking())
            {
                //NEED BETTER WAY OF DOING THIS
                if (gameManager.instance.thirdPersonPlayerController.currentWeapon == gameManager.instance.thirdPersonPlayerController.playerWeapons[5])
                {
                    gameManager.instance.thirdPersonPlayerController.playerAnim.SetFloat("attackSpeed", gameManager.instance.thirdPersonPlayerController.meleAttackSpeed);
                    gameManager.instance.thirdPersonPlayerController.Attacking();
                    gameManager.instance.thirdPersonPlayerController.playerAnim.SetTrigger("AttackOneHand");
                }
                else
                {
                    gameManager.instance.thirdPersonPlayerController.playerAnim.SetFloat("attackSpeed", gameManager.instance.thirdPersonPlayerController.meleAttackSpeed);
                    gameManager.instance.thirdPersonPlayerController.Attacking();
                    gameManager.instance.thirdPersonPlayerController.playerAnim.SetTrigger("Attack");
                }

            }

        }
        else if(type == ItemType.Seeds)
        {
            if (Time.time >= nextTimeTofire)
            {
                nextTimeTofire = Time.time + 1f / 5;
                gameManager.instance.thirdPersonPlayerController.playerAnim.SetTrigger("AddSeed");
            }
        }
        else if(type == ItemType.Tool)
        {
            if (Time.time >= nextTimeTofire)
            {
                nextTimeTofire = Time.time + 1f / 5;

                /*FOR FIRST PERSON CHARACTER
                RaycastHit hit;
                if (Physics.Raycast(gameManager.instance.playerCamera.transform.position, gameManager.instance.playerCamera.transform.forward, out hit, 50f))
                {
                    FarmingLand target = hit.transform.GetComponent<FarmingLand>();
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
                Debug.Log("Getting Land Ready"); */

                gameManager.instance.thirdPersonPlayerController.playerAnim.SetTrigger("axToolUse");
            }
        }
        else
        {
            UnityEngine.Debug.Log("No method yet");
        }
    }
    public void UpdateSlot()
    {
        //If the Stack is less or equal to 0
        if (stackAmount <= 0)
        {
            //Set everything to default(empty) ID = 0 is a empty Slot
            this.GetComponent<Image>().sprite = defaultImage;
            //defaultImage.GetComponent<Image>().color = Color.black;
            this.GetComponentInChildren<TMP_Text>().text = " ";
            ID = 0;
            type = ItemType.Empty;
            itemName = " ";
            description = "Empty Slot";
            stackMax = 0;
            stackAmount = 0;
            itemPrefab = null;
            addAmount = 0;
            usable = false;
            weaponStats = null;
            slotType = SlotType.Regular;
        }
        else
        {
            //Set the visual to the item placed on top and update the ammount text
            //Why how does it know or were was the icon changed? It is changed on the SlotBacground Script int SwitchItemsLocation function
            this.GetComponent<Image>().sprite = icon;
            this.GetComponentInChildren<TMP_Text>().text = stackAmount.ToString();

        }
    }
    public void SelectThis()
    {
        //If is not dragging it means it was clicked
        if (!isDragging)
        {
            //if this is set as selected
            if (transform.GetComponentInParent<SlotBackground>().selected)
            {
                //unselect it because it wwas clicked again
                transform.GetComponentInParent<SlotBackground>().selected = false;
                gameManager.instance.selectedSlot.GetComponentInParent<SlotBackground>().UpdateSelection();
            }
            else
            {
                //if is not selected set selected to false and update to change its color and avoid errors
                gameManager.instance.selectedSlot.GetComponentInParent<SlotBackground>().selected = false;
                gameManager.instance.selectedSlot.GetComponentInParent<SlotBackground>().UpdateSelection();
                //now set the selectedSlot to this one
                gameManager.instance.selectedSlot = transform.gameObject;
                //update it to selected and change color 
                gameManager.instance.selectedSlot.GetComponentInParent<SlotBackground>().selected = true;
                gameManager.instance.selectedSlot.GetComponentInParent<SlotBackground>().UpdateSelection();
            }
        }
    }

    #region Setters and Getters
    public int GetID()
    {
        return ID;
    }

    public ItemType GetItemType()
    {
        return type;
    }

    public string GetItemName()
    {
        return name;
    }

    public string GetItemDescription()
    {
        return description;
    }

    public int GetItemStackMax()
    {
        return stackMax;
    }

    public int GetItemStackAmount()
    {
        return stackAmount;
    }

    public Sprite GetItemIcon()
    {
        return icon;
    }

    public GameObject GetItemPrefab()
    {
        return itemPrefab;
    }
    public bool GetCanBeUsed()
    {
        return usable;
    }

    public SlotType GetSlotType()
    {
        return slotType;
    }
    public ItemWeaponStats GetWeaponStats()
    {
        return weaponStats;
    }

    // ----- setters -----
    public void SetItemID(int _ID)
    {
        ID = _ID;
    }

    public void SetItemType(ItemType _type)
    {
        type = _type;
    }

    public void SetItemName(string _name)
    {
        name = _name;
    }

    public void SetItemDescription(string _description)
    {
        description = _description;
    }

    public void SetItemStackMax(int _stackMax)
    {
        stackMax = _stackMax;
    }

    public void SetItemStackAmount(int _stackAmount)
    {
        stackAmount = _stackAmount;
    }

    public void SetItemIcon(Sprite _icon)
    {
        icon = _icon;
    }

    public void SetItemPrefab(GameObject _prefab)
    {
        itemPrefab = _prefab;
    }
    public void SetSlotType(SlotType _slotType)
    {
         slotType = _slotType;
    }
    public void SetWeaponStats(ItemWeaponStats _weaponStats)
    {
         weaponStats = _weaponStats;
    }

    #endregion

    #region Helper Functions
    // ----- helper funcs -----

    public void IncrementStackBy(int amount)
    {
        stackAmount += amount;
    }

    public void DecrementStackBy(int amount)
    {
        stackAmount -= amount;
    }

    public int GetFreeSpace()
    {
        return stackMax - stackAmount;
    }

    public void AddItemToSlot(int _ID, ItemType _type, string _itemName, string _description, int _stackMax, Sprite _icon, GameObject _itemPrefab, int _addAmount, bool _usable, ItemWeaponStats _weaponStats, SlotType _slotType)
    {
        ID = _ID;
        type = _type;
        itemName = _itemName;
        description = _description;
        stackMax = _stackMax;
        icon = _icon;
        itemPrefab = _itemPrefab;
        addAmount = _addAmount;
        usable = _usable;
        weaponStats = _weaponStats;
        slotType = _slotType;
    }
    #endregion

    #region Drag Methods
    public void OnBeginDrag(PointerEventData eventData)
    {
        //Set parent to cursor
             parentAfterDrag = transform.parent;
             transform.SetParent(transform.root);
             //Is not colliding with what is under
             transform.GetComponent<Image>().raycastTarget = false;     
    }
    public void OnDrag(PointerEventData eventData)
    { 
            // Get location of the mouse using canvas size and eventData
            isDragging = true;
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
            //Remove cursor as parent and make the parent what it was before being grabed, is able to know what is bellow again
            isDragging = false;
            transform.SetParent(parentAfterDrag);
            transform.GetComponent<Image>().raycastTarget = true;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        // THIS IS CALLED ON THE ACTUAL BUTTON of the slot
    }
    #endregion


    
}
