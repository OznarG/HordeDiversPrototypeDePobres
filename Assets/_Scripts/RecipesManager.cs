using UnityEngine;

public class RecipesManager : MonoBehaviour
{
    [Header("--- References ---")]
    public UpdaterRecipes updater;
    public static RecipesManager instance;

    [Header("--- Variables ---")]
    [SerializeField] bool crafting;

    [SerializeField] Recipes stonePickaxe;
    [SerializeField] Recipes stoneSword;
    [SerializeField] Recipes stoneAxe;

    private void Start()
    {
        instance = this;
    }

    public bool GetState()
    {
        return crafting;
    }

    public void SetState(bool param)
    {
        crafting = param;
    }    

}
