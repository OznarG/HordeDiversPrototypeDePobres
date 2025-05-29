using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    public static EnemiesManager instance;
    public GameObject[] enemies;

    private void Awake()
    {
        instance = this;
    }

}
