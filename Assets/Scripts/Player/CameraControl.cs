using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Camera playerCamera;
    public GameObject player;
    public float sensivity = 5;

    float xRot;
    float yRot;
    void Update()
    {
        MouseMove();
    }

    private void MouseMove()
    {
        xRot += Input.GetAxis("Mouse X");
        yRot += Input.GetAxis("Mouse Y");

        playerCamera.transform.rotation = Quaternion.Euler(-yRot, xRot, 0f);
        player.transform.rotation = Quaternion.Euler(0f, xRot, 0f);
    }
}
