using UnityEngine;

public class FPSMovement : MonoBehaviour
{
        //Movement Settings
        public float moveSpeed = 5f;
        public float mouseSensitivity = 100f;
    
        [Header("References")]
        public Rigidbody playerRigidbody;
        public Transform cameraTransform;
    
        private float xRotation = 0f;
    
        void Start()
        {
            // Lock the cursor to the center of the screen
            Cursor.lockState = CursorLockMode.Locked;
        }
    
        void Update()
        {
            // Handle mouse look
            HandleMouseLook();
    
            // Handle movement
            HandleMovement();
        }
    
        void HandleMouseLook()
        {
            // Get mouse input
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
    
            // Adjust vertical rotation (camera)
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Clamp to prevent over-rotation
    
            // Apply rotations
            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX); // Rotate player body horizontally
        }
    
        void HandleMovement()
        {
            // Initialize movement direction
            Vector3 move = Vector3.zero;
    
            // Get input for movement using KeyCode
            if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.W))
            {
                move += transform.forward; // Move forward
            }
            if (Input.GetKey(KeyCode.S))
            {
                move -= transform.forward; // Move backward
            }
            if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.A))
            {
                move -= transform.right; // Move left
            }
            if (Input.GetKey(KeyCode.D))
            {
                move += transform.right; // Move right
            }
            
            // Apply movement using Rigidbody
            playerRigidbody.velocity = move.normalized * moveSpeed +
                                       new Vector3(0, playerRigidbody.velocity.y, 0); // Preserve vertical velocity
        }
}
