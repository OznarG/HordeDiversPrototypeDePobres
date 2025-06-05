using UnityEngine;
[CreateAssetMenu]
public class Recipes : ScriptableObject
{
    public Item[] items;
    public int[] amount;
    public int index;
    public string recipeName;
}
