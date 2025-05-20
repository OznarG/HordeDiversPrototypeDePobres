using System.Collections;
using UnityEngine;

public class LoockedDoor : MonoBehaviour
{
    [SerializeField] int codeNeeded;
    [SerializeField] int door;

    public float openSpeed = 2.0f; // Speed at which the door opens
    public float openAngle = 90.0f; // Angle to which the door opens
    //private bool isOpening = false; // Is the door currently opening
    private Quaternion originalRotation;
    private Quaternion targetRotation;

    private void Start()
    {
        originalRotation = transform.rotation;
        targetRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, openAngle, 0));
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(gameManager.instance.playerController.keys.Contains(codeNeeded))
            {
                StartCoroutine(OpenDoor());
            }
            else
            {
                Debug.Log("You Need the key motherfucker");
            }
        }

    }

    private IEnumerator OpenDoor()
    {
        float timeElapsed = 0;
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(originalRotation, targetRotation, timeElapsed * openSpeed);
            timeElapsed += Time.deltaTime;
            yield return null; // Wait for the next frame
        }
        transform.rotation = targetRotation; // Ensure final position is exact
    }
}
