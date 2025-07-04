using UnityEngine;

public class SimpleCameraSwitcher : MonoBehaviour
{
    public Camera thirdPersonCam;
    public Camera firstPersonCam;

    private void Start()
    {
        thirdPersonCam.enabled = true;
        firstPersonCam.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TightSpace"))
        {
            thirdPersonCam.enabled = false;
            firstPersonCam.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TightSpace"))
        {
            thirdPersonCam.enabled = true;
            firstPersonCam.enabled = false;
        }
    }
}
