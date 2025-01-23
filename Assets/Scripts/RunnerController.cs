using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerController : MonoBehaviour
{
    public float moveSpeed = 10f; // Vitesse de course
    public float jumpForce = 5f; // Force du saut
    public Transform groundCheck; // Point de vérification au sol
    public LayerMask groundLayer; // Détection du sol

    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Mouvement automatique
        rb.velocity = new Vector3(0f, rb.velocity.y, moveSpeed);

        // Vérification au sol
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, groundLayer);

        // Saut
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }
    }
}
