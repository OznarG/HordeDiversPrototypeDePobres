using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private Transform arms;
    [SerializeField] private Transform body;

    private float xRot;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
       // Debug.Log("Cursor should now be hidden!");

    }
    private void Update()
    {
        HandleCameraMovement();
    }

    private void HandleCameraMovement()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRot -= mouseY;
        xRot = Mathf.Clamp(xRot, -90, 90);

        arms.localRotation = Quaternion.Euler(new Vector3(xRot, 0, 0));
        body.Rotate(new Vector3(0, mouseX, 0));
    }
}
