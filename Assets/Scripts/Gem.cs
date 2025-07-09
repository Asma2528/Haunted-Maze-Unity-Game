using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    // Public variables to be set from the Inspector
    public Mesh[] allGems;          // Array to hold different gem meshes
    public Color[] allColors;       // Array to hold different gem colors
    public AudioClip pickUpSFX;     // Audio clip to be played when the gem is picked up

    // Start is called before the first frame update
    private void Start()
    {
        // Set a random mesh for the gem from the allGems array
        // Random.Range(min, max) returns a random integer between min (inclusive) and max (exclusive)
        GetComponent<MeshFilter>().mesh = allGems[Random.Range(0, allGems.Length)];

        // Set a random color for the gem from the allColors array
        // This is done by accessing the Renderer component and setting its material color
        GetComponent<Renderer>().material.color = allColors[Random.Range(0, allColors.Length)];
    }

    // Called when another collider enters the trigger collider attached to this gem
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider that entered is tagged as "Player"
        if (other.CompareTag("Player"))
        {
            // Notify the MapManager that a gem has been picked up
            MapManager.instance.GemPickedUp();

            // Play the pick-up sound effect using the AudioManager
            AudioManager.instance.PlaySFX(pickUpSFX);

            // Destroy the gem game object
            Destroy(gameObject);
        }
    }
}
