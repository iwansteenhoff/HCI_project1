using UnityEngine;
using UnityEngine.UI; // Import the UI namespace for Image

public class VirusImageController : MonoBehaviour
{
    // Reference to the Image component
    private Image imageComponent;

    // Called when the script instance is being loaded
    private void Awake()
    {
        // Get the Image component on the same GameObject
        imageComponent = GetComponent<Image>();

        if (imageComponent == null)
        {
            Debug.LogError("Image component not found on the GameObject!");
            return;
        }

        // Initially hide the image
        imageComponent.sprite = null; // Clear any assigned sprite
        imageComponent.enabled = false; // Disable the Image component
    }

    // Function to set the sprite
    public void SetVirusImage(string virus)
    {
        Sprite image = Resources.Load<Sprite>($"virus images/{virus}");
        if (image == null)
        {
            Debug.LogError($"Sprite '{virus}' not found in Resources/virus images/");
            return;
        }

        // Set the sprite and make the Image visible
        imageComponent.sprite = image;
        imageComponent.enabled = true;
    }
}
