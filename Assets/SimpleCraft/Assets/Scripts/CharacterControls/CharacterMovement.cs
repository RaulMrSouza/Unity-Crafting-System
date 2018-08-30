using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterControls{

    public class CharacterMovement : MonoBehaviour{
        public float speed = 6.0F;
        public float jumpSpeed = 8.0F;
        public float gravity = 20.0F;

        private Vector3 moveDirection = Vector3.zero;

        CharacterController controller;
        void Start(){
            controller = GetComponent<CharacterController>();
        }

        void FixedUpdate(){
            if (controller.isGrounded){
                moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                moveDirection = transform.TransformDirection(moveDirection);
                moveDirection *= speed;
                if (Input.GetButton("Jump"))
                    moveDirection.y = jumpSpeed;
            }

            moveDirection.y -= gravity * Time.deltaTime;
            controller.Move(moveDirection * Time.deltaTime);
        }
    }
}
