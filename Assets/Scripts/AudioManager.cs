using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Public variables to assign audio sources from the Inspector
    public AudioSource ambientMusic; // This AudioSource will play ambient music
    public AudioSource sfx;          // This AudioSource will play sound effects (SFX)

    // Static instance to implement the Singleton pattern for global access
    public static AudioManager instance;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Check if the instance already exists
        if (instance == null)
        {
            // If not, set the instance to this script instance
            instance = this;
        }
        else
        {
            // If the instance already exists, destroy this duplicate
            Destroy(gameObject);
        }
    }

    // Method to play sound effects
    public void PlaySFX(AudioClip clip)
    {
        // Play the provided audio clip once using the sfx AudioSource
        sfx.PlayOneShot(clip);
    }
}
