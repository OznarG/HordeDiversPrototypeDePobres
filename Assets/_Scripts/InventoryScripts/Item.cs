using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    //this will take from scriptable object to be added into the inventory
    public int ID;
    //Types 2: Heals the character | Type 10: Weapons | Type 5: Ammo | 
    public ItemType type;
    public string itemName;
    public string description;
    public int stackMax;
    public bool usable;
    public int amountToAdd;
    public Sprite icon;
    public GameObject itemPrefab;
    public ItemWeaponStats weaponStats;
    public SlotType slotType;
    //ID 
    //--Weapons--
    [Header("Item Weapon")]
    public int weaponLevel;
    public float damage;
    public float strength;
    public float speed;
    public string _name;
    public int index;

}
