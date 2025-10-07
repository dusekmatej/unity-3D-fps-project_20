using UnityEngine;

namespace Horse
{
    public class HorseMovement : MonoBehaviour
    {
        public CharacterController controller;

        public float speed = 6f;
        public float gravity = -9.81f;
        public float sprintSpeed = 13f;
        public float jumpHeight = 10f;

        Vector3 _velocity;

        public Transform groundCheck;
        public float groundDistance = 0.4f;
        public LayerMask groundMask;

        bool _isGrounded;

        void Update()
        {
            _isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (_isGrounded && _velocity.y < 0)
            {
                _velocity.y = -2f;
            }
        
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;
        
            _velocity.y += gravity * Time.deltaTime;

            controller.Move(_velocity * Time.deltaTime);

            if (Input.GetButtonDown("Jump") && _isGrounded)
            {
                _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
            
            // Horse Sprinting
        
            if (Input.GetKey(KeyCode.LeftShift))
            {
                controller.Move(move * sprintSpeed * Time.deltaTime);
            }
            else
            {
                controller.Move(move * speed * Time.deltaTime);
            }
        }
    }
}