using System.Collections.Generic;
using UnityEngine;

public class CraftingMenu : MonoBehaviour
{
    public Dictionary<string, int> itemsOnHand;
    [SerializeField] Recipes recipe;
    int matches;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        itemsOnHand = new Dictionary<string, int>();
    }

    public bool CanBeCrafted()
    {
        itemsOnHand = gameManager.instance.playerInventoryScript.itemsOnHand;
        for(int i = 0; i < recipe.index; i++)
        {
            if (itemsOnHand.ContainsKey(recipe.items[i]._name))
            {
                if (itemsOnHand[recipe.items[i]._name] >= recipe.amount[i])
                {
                    continue;
                }
                else
                {
                    Debug.Log("Not Enough Items");
                    return false;
                }
            }
            else
            {
                Debug.Log("Not Enough Items");
                return false;
            }
        }
        Debug.Log("Enough Items");
        return true;
    }
}
