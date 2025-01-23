using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateformerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Vitesse de déplacement
    public float jumpForce = 5f; // Force du saut
    public Transform groundCheck; // Point de vérification au sol
    public LayerMask groundLayer; // Détection du sol

    private Rigidbody rb;
    public bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Déplacement
        float move = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(move * moveSpeed, rb.velocity.y, 0f);
        rb.velocity = movement;

        // Vérification au sol
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, groundLayer);

        // Saut
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }
    }
}
