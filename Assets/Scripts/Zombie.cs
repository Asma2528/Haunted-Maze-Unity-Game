using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    private Vector3 randomPos;          // The random position for the zombie to walk to
    private GameObject target;          // The player object, which the zombie will chase
    private NavMeshAgent agent;         // The NavMeshAgent component for pathfinding

    public AudioSource groanSFX;        // AudioSource for the zombie's groan sound effect

    private bool isRunning;             // Flag to check if the zombie is currently running
    private bool isWalking;             // Flag to check if the zombie is currently walking

    private Animator anim;              // Animator for controlling zombie animations

    private void Start()
    {
        // Initialize variables
        randomPos = transform.position;           // Start with the current position
        target = GameObject.FindGameObjectWithTag("Player"); // Find the player object
        agent = GetComponent<NavMeshAgent>();     // Get the NavMeshAgent component
        anim = GetComponent<Animator>();          // Get the Animator component

        WalkToRandomSpot(); // Command the zombie to start walking to a random spot

        // Check if the NavMeshAgent is properly placed on the NavMesh
        if (!agent.isOnNavMesh)
        {
            Debug.LogError("Zombie agent is not placed on the NavMesh!");
        }
    }

    private void Update()
    {
        // Check if zombies are allowed to move
        if (MapManager.instance.ZombiesCanMove)
        {
            if (agent.isOnNavMesh)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, target.transform.position);

                // Check distance to player and decide behavior
                if (distanceToPlayer <= 5)
                {
                    ChasePlayer(); // Chase the player if within range
                }
                else if (!isRunning)
                {
                    WalkToRandomSpot(); // Walk to a random spot if not running
                }

                // Check if the zombie has reached the random position
                if (isWalking)
                {
                    if (Vector3.Distance(transform.position, randomPos) <= 3)
                    {
                        isWalking = false; // Stop walking if close to the target position
                    }
                }

                // Trigger the attack animation if the zombie is very close to the player
                if (Vector3.Distance(transform.position, target.transform.position) <= 1)
                {
                    anim.SetTrigger("attack");
                }
            }
            else
            {
                Debug.LogWarning("Agent is not on NavMesh in Update");
            }
        }
    }

    // Method to make the zombie chase the player
    private void ChasePlayer()
    {
        if (agent.isOnNavMesh)
        {
            agent.SetDestination(target.transform.position); // Set the destination to the player's position

            // Update animation and sound if the zombie starts running
            if (!isRunning)
            {
                groanSFX.Play(); // Play the groan sound effect
                isRunning = true;
                isWalking = false;
                agent.speed = 2; // Increase speed for running
                anim.SetBool("isRunning", isRunning); // Set the running animation state
                anim.SetBool("isWalking", isWalking); // Ensure walking animation state is false
            }
        }
        else
        {
            Debug.LogWarning("Agent is not on NavMesh in ChasePlayer");
        }
    }

    // Method to make the zombie walk to a random spot
    private void WalkToRandomSpot()
    {
        if (MapManager.instance != null)
        {
            agent.speed = 0.75f; // Set a slower speed for walking
            randomPos = MapManager.instance.GetRandomPosition(); // Get a random position from MapManager

            if (agent.isOnNavMesh)
            {
                agent.SetDestination(randomPos); // Set the destination to the random position
                isRunning = false;
                isWalking = true;
            }
            else
            {
                Debug.LogWarning("Agent is not on NavMesh in WalkToRandomSpot");
            }
        }
    }
}
