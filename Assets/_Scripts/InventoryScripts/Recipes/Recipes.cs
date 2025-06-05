using UnityEngine;
[CreateAssetMenu]
public class Recipes : ScriptableObject
{
    [SerializeField] Item[] items;
    [SerializeField] int[] amount;
    [SerializeField] int index;
    [SerializeField] string recipeName;
}
