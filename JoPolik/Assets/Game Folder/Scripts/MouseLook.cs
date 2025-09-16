using UnityEngine;
using Photon.Pun;

namespace SojaExiles
{
    public class MouseLook : MonoBehaviourPunCallbacks
    {
        public float mouseXSensitivity = 100f;
        public Transform playerBody;

        float xRotation = 0f;

        void Start()
        {
            if (!photonView.IsMine)
            {
                // Disable this script if it's not our player
                enabled = false;
                return;
            }

            Cursor.lockState = CursorLockMode.Locked;
        }

        void Update()
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseXSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseXSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }
}
