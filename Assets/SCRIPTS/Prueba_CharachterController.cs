using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prueba_CharachterController : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public float gravity;

    private Vector3 moveDirection;
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Get input from the user
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate the direction of movement based on the input
        moveDirection = new Vector3(horizontalInput, 0, verticalInput);

        // Apply gravity to the character controller
        moveDirection.y -= gravity * Time.deltaTime;

        // Move the character controller in the direction of movement
        controller.Move(moveDirection * speed * Time.deltaTime);

        // Jump if the user presses the space bar
        if(Input.GetButtonDown("Jump"))
        {
            moveDirection.y = jumpForce;
        }
    }
}
