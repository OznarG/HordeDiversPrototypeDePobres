using UnityEngine;

public class FatherSpawner : MonoBehaviour
{
    public Transform[] transforms;

    private void Start()
    {

    }

    public Transform GetTransform(int index)
    {
        return transforms[index];
    }
}
