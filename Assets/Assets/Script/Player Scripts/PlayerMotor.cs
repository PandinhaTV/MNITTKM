using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    
    private CharacterController controller;
    private Vector3 playerVelocity;
    public float speed = 5f;
    public bool isGrounded;
    public float gravity = 9.8f;
    public float jumpHeight = 3f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    
    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;
    }
    //Get inputs from InputManager.cs and applies it to the character
    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        controller.Move(transform.TransformDirection(moveDirection) * (speed * Time.deltaTime));
        playerVelocity.y += -gravity * Time.deltaTime;    
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f; // Reset velocity before gravity is applied

        }
        
        controller.Move(playerVelocity * Time.deltaTime);
        Debug.Log(playerVelocity.y);
        Debug.Log("Gravity: " + gravity);
    }

    public void Jump()
    {
        Debug.Log("Jump");
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Abs(jumpHeight * -2.0f * gravity);
        }
    }
}
