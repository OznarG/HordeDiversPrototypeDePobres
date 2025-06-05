using System.Collections.Generic;
using UnityEngine;

public class RecipeButtons : MonoBehaviour
{
    [SerializeField] Recipes recipe;
    //NEED TO GRAB THIS FROM AN INSTANCE GAMEMANAGER FORM THE CRAFTING MENU REFERENCE
    public Dictionary<string, int> itemsOnHand;
    public void CanBeCrafted()
    {
        itemsOnHand = gameManager.instance.playerInventoryScript.itemsOnHand;
        for (int i = 0; i < recipe.index; i++)
        {
            if (itemsOnHand.ContainsKey(recipe.items[i].itemName))
            {
                if (itemsOnHand[recipe.items[i].itemName] >= recipe.amount[i])
                {
                    //continue;
                }
                else
                {
                    Debug.Log("Not Enough Items");
                    //return false;
                }
            }
            else
            {
                Debug.Log("Not Enough Items");
                //return false;
            }
        }
        Debug.Log("Enough Items");
        //return true;
    }
}
