using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    // Public variables to be set from the Inspector
    public Texture2D[] maps;          // Array of maps represented as textures
    public GameObject wallPrefab;    // Prefab for the wall object
    public GameObject gemPrefab;     // Prefab for the gem object
    public GameObject zombiePrefab;  // Prefab for the zombie object

    private int gemsRemaining;        // Counter for remaining gems

    public bool ZombiesCanMove = true; // Flag to control whether zombies can move

    public Texture2D selectedMap;     // The currently selected map texture

    public List<Vector3> openPositions = new List<Vector3>(); // List of available positions for placing objects

    public static MapManager instance; // Singleton instance of MapManager

    private Color wallColor = Color.black; // Color used to identify walls in the map

    private void Awake()
    {
        // Implement Singleton pattern to ensure only one instance of MapManager exists
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    private void Start()
    {
        // Check if maps are assigned
        if (maps.Length == 0)
        {
            Debug.LogError("No maps assigned to the MapManager.");
            return;
        }
        // Generate the map, gems, and zombies
        GenerateNewMap();
        GenerateGems();
        GenerateZombies();
    }

    // Method to generate a new map based on the selected map texture
    private void GenerateNewMap()
    {
        openPositions.Clear(); // Clear previous positions

        // Randomly select a map texture
        int mapIndex = Random.Range(0, maps.Length);
        selectedMap = maps[mapIndex];

        // Check if the selected map is valid
        if (selectedMap == null)
        {
            Debug.LogError("Selected map is null.");
            return;
        }

        // Iterate over each pixel in the map texture
        for (int x = 0; x < selectedMap.width; x++)
        {
            for (int y = 0; y < selectedMap.height; y++)
            {
                GenerateTile(x, y); // Generate tiles based on pixel colors
            }
        }
    }

    // Method to generate a tile at position (x, y)
    private void GenerateTile(int x, int y)
    {
        // Get the color of the pixel at (x, y)
        Color pixelColor = selectedMap.GetPixel(x, y);

        // Check if the pixel is transparent (open position)
        if (pixelColor.a == 0)
        {
            openPositions.Add(new Vector3(x, 0, y)); // Add to open positions list
            return;
        }
        // Check if the pixel color matches the wall color
        if (pixelColor == wallColor)
        {
            // Instantiate a wall prefab at the position
            Instantiate(wallPrefab, new Vector3(x, 0, y), Quaternion.identity, transform);
        }
    }

    // Method to generate a fixed number of gems
    private void GenerateGems()
    {
        for (int i = 0; i < 10; i++)
        {
            // Select a random position from the list of open positions
            int index = Random.Range(0, openPositions.Count);
            
            // Instantiate a gem prefab at the selected position
            Instantiate(gemPrefab, openPositions[index], Quaternion.identity);
            
            // Remove the position from the list
            openPositions.RemoveAt(index);
        }

        // Set the number of remaining gems
        gemsRemaining = 10;
    }

    // Method to generate a fixed number of zombies
    private void GenerateZombies()
    {
        for (int i = 0; i < 5; i++)
        {
            // Select a random position from the list of open positions
            int index = Random.Range(0, openPositions.Count);
            
            // Instantiate a zombie prefab at the selected position
            Instantiate(zombiePrefab, openPositions[index], Quaternion.identity);
            
            // Remove the position from the list
            openPositions.RemoveAt(index);
        }
    }

    // Method to get a random open position from the list
    public Vector3 GetRandomPosition()
    {
        return openPositions[Random.Range(0, openPositions.Count)];
    }

    // Method to handle when a gem is picked up
    public void GemPickedUp()
    {
        gemsRemaining--; // Decrease the count of remaining gems

        // Check if all gems have been picked up
        if (gemsRemaining == 0)
        {
            ZombiesCanMove = false; // Stop zombies from moving
            UIManager.instance.ShowGameOver(true); // Show the game over screen
        }
    }
}
