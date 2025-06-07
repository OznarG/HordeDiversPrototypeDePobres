using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VFolders.Libs;

public class RecipeButtons : MonoBehaviour
{
    [SerializeField] Recipes recipe;
    [SerializeField] Image _image;
    [SerializeField] Image filler;
    UpdaterRecipes updater;
    public float timeToComplete;
    [SerializeField] bool bussy;
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
        if (!bussy)
        {
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
                    }
                    else
                    {
                        result = false;
                        Debug.Log("Not Enough Items");
                        //return false;
                    }
                }
                else
                {
                    Debug.Log("Not Enough Items");
                    result = false;
                }
            }
            if (result)
            {
                filler.enabled = true;
                updater = gameObject.AddComponent<UpdaterRecipes>();
                updater.Initialize(this); // Pass the RecipeButtons reference
            }
            Debug.Log("Enough Items");
            //return true;
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
            Destroy(updater);
            filler.enabled = false;
            bussy = false;
            timeToComplete = 0;
        }
    }
}
