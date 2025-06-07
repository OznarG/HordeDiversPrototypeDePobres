using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu]
public class Recipes : ScriptableObject
{
    public Item[] items;
    public int[] amount;
    public int index;
    public string recipeName;
    public Sprite image;
    public Item returnItem;
}
