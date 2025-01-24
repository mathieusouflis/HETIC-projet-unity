using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    
    private Rigidbody playerRigidbody;
    public double playerHealth = 100;
    public double KillCount = 0;
    public Vector3 PlayerSpawnPoint;
    public float playerSpeed = 2f;
    private bool _isGrounded;
    public bool canMove = true;
    public AudioSource death;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerSpawnPoint.y <= 0)
        {
            PlayerSpawnPoint = new Vector3(0, 2, 0);
        }
        playerRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            Move();
        }
    }
    
    private void Move()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            // Demander au prof comment faire autrement
            //Rigidbody rigidbody = GetComponent<Rigidbody>();
            if (!canMove) return;
            JumpPlayer();
        }
            
        //Deplacements
        if (Input.GetKey(KeyCode.W))
        {
            MovePlayer("z");
        }
        if (Input.GetKey(KeyCode.A))
        {
            MovePlayer("q");
        }
        if (Input.GetKey(KeyCode.S))
        {
            MovePlayer("s");
        }
        if (Input.GetKey(KeyCode.D))
        {
            MovePlayer("d");
        }

        CheckGround();
    }

    public void JumpPlayer()
    {
        if(canMove) GameObject.Find("Game Values").GetComponent<WS>().SendMessage("jump", new Dictionary<string, object> {{"jump", "jump"}});
        playerRigidbody.velocity = Vector3.up * 3;
    }
    public void MoveFromPosition(Vector3 position)
    {
        transform.position = position;
    }
    public void MovePlayer(string axis)
    {
        switch (axis)
        {
            case "z":
                transform.position += transform.forward * Time.deltaTime * playerSpeed;
                break;
            case "q":
                transform.position -= transform.right * Time.deltaTime * playerSpeed;
                break;
            case "s":
                transform.position -= transform.forward * Time.deltaTime * playerSpeed;
                break;
            case "d":
                transform.position += transform.right * Time.deltaTime * playerSpeed;
                break;
            
        }
        if (canMove)
        {
            GameObject.Find("Game Values").GetComponent<WS>().SendMessage("move", new Dictionary<string, object> {{"x", transform.position.x}, {"y", transform.position.y}, {"z", transform.position.z}});
        }
        
    }

    private void CheckGround()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, (float)1.3);
    }

    public void CheckHp()
    {
        if (playerHealth <= 0)
        {
            Debug.Log("Player Dead");
            
            if (this.name == "Player")
            {
                GameObject.Find("Player2").GetComponent<PlayerControler>().KillCount++;
            }
            else
            {
                GameObject.Find("Player").GetComponent<PlayerControler>().KillCount++;
                death.Play();

            }
            // On change les compteurs
            this.playerHealth = 100;
            transform.position = PlayerSpawnPoint;
            
        }
    }
}
