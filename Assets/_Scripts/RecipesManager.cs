using UnityEngine;

public class RecipesManager : MonoBehaviour
{
    [Header("--- References ---")]
    public UpdaterRecipes updater;
    public static RecipesManager instance;

    [SerializeField] Recipes stonePickaxe;
    [SerializeField] Recipes stoneSword;
    [SerializeField] Recipes stoneAxe;

    private void Start()
    {
        instance = this;
    }


}
