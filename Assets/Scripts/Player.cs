using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Public variables to be set from the Inspector
    public AudioSource footsteps;  // AudioSource for playing footstep sounds
    public AudioClip screamSFX;    // AudioClip for the scream sound effect when the player dies

    public float speed = 5;         // Movement speed of the player
    private CharacterController controller; // CharacterController for player movement
    private Animator anim;          // Animator for controlling animations

    private bool isMoving;          // Flag to check if the player is moving
    private bool canMove = true;    // Flag to check if the player can move

    private float horizontalMovement; // Horizontal movement input
    private float verticalMovement;   // Vertical movement input
    private Vector3 direction;        // Direction vector for movement

    public float turnSmoothTime = 0.1f;  // Time for smoothing the rotation turn
    private float turnSmoothVelocity;    // Smooth velocity used in rotation

    void Start()
    {
        // Initialize the CharacterController and Animator components
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (canMove)
        {
            // Check movement and animation states
            movementCheck();
            AnimationCheck();
        }
        else
        {
            // Stop footsteps sound if the player cannot move (e.g., when dead)
            if (footsteps.isPlaying)
            {
                footsteps.Stop();
            }
        }
    }

    // Method to handle player movement
    private void movementCheck()
    {
        // Get input for horizontal and vertical movement
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        // Create direction vector based on input
        direction = new Vector3(horizontalMovement, 0, verticalMovement);

        // Check if there is significant movement input
        if (direction.magnitude > 0.1f)
        {
            // Calculate target angle for rotation
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            // Smoothly rotate the player towards the target angle
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0, angle, 0);

            // Move the player character based on direction and speed
            controller.Move(direction * speed * Time.deltaTime);
        }
    }

    // Method to handle player animation based on movement
    private void AnimationCheck()
    {
        // Check if the player is moving and update animation and footsteps sound
        if (direction != Vector3.zero && !isMoving)
        {
            footsteps.Play(); // Play footstep sound
            isMoving = true;
            anim.SetBool("isRunning", isMoving); // Set animation parameter
        }
        else if (direction == Vector3.zero && isMoving)
        {
            footsteps.Stop(); // Stop footstep sound
            isMoving = false;
            anim.SetBool("isRunning", isMoving); // Set animation parameter
        }
    }

    // Method called when the player collider enters a trigger collider
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to a zombie
        if (other.CompareTag("Zombie"))
        {
            canMove = false; // Disable player movement
            UIManager.instance.ShowGameOver(false); // Show game over screen
            anim.SetTrigger("isDead"); // Trigger death animation
            AudioManager.instance.PlaySFX(screamSFX); // Play scream sound effect
        }
    }
}
