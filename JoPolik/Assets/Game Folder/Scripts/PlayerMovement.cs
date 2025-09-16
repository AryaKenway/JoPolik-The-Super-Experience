//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace SojaExiles

//{
//    public class PlayerMovement : MonoBehaviour
//    {

//        public CharacterController controller;

//        public float speed = 5f;
//        public float gravity = -15f;

//        Vector3 velocity;

//        bool isGrounded;

//        // Update is called once per frame
//        void Update()
//        {

//            float x = Input.GetAxis("Horizontal");
//            float z = Input.GetAxis("Vertical");

//            Vector3 move = transform.right * x + transform.forward * z;

//            controller.Move(move * speed * Time.deltaTime);

//            velocity.y += gravity * Time.deltaTime;

//            controller.Move(velocity * Time.deltaTime);

//        }
//    }
//}

using UnityEngine;
using Photon.Pun;

namespace SojaExiles
{
    public class PlayerMovement : MonoBehaviourPunCallbacks
    {
        public CharacterController controller;
        public float speed = 5f;
        public float gravity = -15f;
        public float interactRange = 3f; // range to interact with doors

        Vector3 velocity;

        void Update()
        {
            if (!photonView.IsMine) return;

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
