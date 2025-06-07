using UnityEngine;

public class UpdaterRecipes : MonoBehaviour
{
    private RecipeButtons buttons;

    public void Initialize(RecipeButtons buttons)
    {
        this.buttons = buttons;
    }

    private void Update()
    {
        Debug.Log("About to call Fill");
        buttons.Fill();
    }

}
