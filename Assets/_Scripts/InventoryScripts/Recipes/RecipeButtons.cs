using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VFolders.Libs;

public class RecipeButtons : MonoBehaviour
{
    [SerializeField] Recipes recipe;
    [SerializeField] Image _image;
    [SerializeField] Image filler;
    public float timeToComplete;
    //NEED TO GRAB THIS FROM AN INSTANCE GAMEMANAGER FORM THE CRAFTING MENU REFERENCE
    public Dictionary<string, int> itemsOnHand;

    public float craftingTime;
    private void Start()
    {
        _image.sprite = recipe.image; 
    }
    public void CanBeCrafted()
    {
        Debug.Log("Pressed");
        if (!RecipesManager.instance.GetState())
        {
            RecipesManager.instance.SetState(true);
            bool result = true;
            itemsOnHand = gameManager.instance.playerInventoryScript.itemsOnHand;
            for (int i = 0; i < recipe.index; i++)
            {
                if (itemsOnHand.ContainsKey(recipe.items[i].itemName))
                {
                    if (itemsOnHand[recipe.items[i].itemName] >= recipe.amount[i])
                    {
                        //continue;
                        result = true;
                        for(int j = 0; j < recipe.amount[i]; j++)
                        {
                            gameManager.instance.playerInventoryScript.HoldItem(recipe.items[i].itemName);
                        }
                    }
                    else
                    {                        
                        result = false;
                        Debug.Log("Not Enough Items");
                        break;
                    }
                }
                else
                {
                    Debug.Log("Not Enough Items");
                    result = false;                   
                    break;
                }
            }
            if (result)
            {
                filler.enabled = true;
                RecipesManager.instance.updater = RecipesManager.instance.gameObject.AddComponent<UpdaterRecipes>();
                RecipesManager.instance.updater.Initialize(this); // Pass the RecipeButtons reference
            }
            else
            {
                RecipesManager.instance.SetState(false);
            }
            Debug.Log("Enough Items");           
        }
    }

    public void Fill()
    {
        Debug.Log("Calling Fill");
        timeToComplete += Time.unscaledDeltaTime;
        filler.fillAmount = timeToComplete / craftingTime;
        if(timeToComplete >= craftingTime)
        {
            gameManager.instance.playerInventoryScript.AddItem(recipe.returnItem);
            foreach(Slot item in gameManager.instance.playerInventoryScript.itemsInUse)
            {
                Debug.Log(item.GetItemName() + " Is the current tem in use" );
                if(gameManager.instance.playerInventoryScript.itemsOnHand.ContainsKey(item.GetItemName()))
                {
                    gameManager.instance.playerInventoryScript.itemsOnHand[item.GetItemName()] -= 1;
                    Debug.Log(item.GetItemName() + " was removed from items on hand");
                }
            }
            gameManager.instance.playerInventoryScript.itemsInUse.Clear();
            Destroy(RecipesManager.instance.updater);
            filler.enabled = false;
            RecipesManager.instance.SetState(false);
            timeToComplete = 0;
        }
    }
}
