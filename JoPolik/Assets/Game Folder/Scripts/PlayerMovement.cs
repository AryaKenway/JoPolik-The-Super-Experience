using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;

namespace SojaExiles
{
    public class PlayerMovement : MonoBehaviourPunCallbacks
    {
        public CharacterController controller;
        public float speed = 5f;
        public float gravity = -15f;
        public float interactRange = 3f; // range to interact with doors
        public bool canMove = true;


        Vector3 velocity;

        void Update()
        {
            if (!photonView.IsMine) return;
            if (!canMove) return; // disable movement/input when panel is open


            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;
            controller.Move(move * speed * Time.deltaTime);

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);

            // Interaction with "E"
            if (Input.GetKeyDown(KeyCode.E))
            {
                TryInteract();
            }
        }

        void TryInteract()
        {
            Ray ray = new Ray(transform.position + Vector3.up, transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, interactRange))
            {
                opencloseDoor door = hit.collider.GetComponent<opencloseDoor>();
                if (door != null)
                {
                    if (!door.open)
                    {
                        StartCoroutine(door.opening());
                    }
                    else
                    {
                        StartCoroutine(door.closing());
                    }
                }
            }
        }
    }
}
