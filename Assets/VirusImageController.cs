using UnityEngine;

public class VirusImageController : MonoBehaviour
{
    // Reference to the SpriteRenderer component
    private SpriteRenderer spriteRenderer;

    // Called when the script instance is being loaded
    private void Awake()
    {
        // Get the SpriteRenderer component on the same GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found on the GameObject!");
        }
    }

    // Function to set the sprite
    public void SetVirusImage(string virus)
    {
        Sprite image = Resources.Load<Sprite>($"virus images/{virus}");
        spriteRenderer.sprite = image;
    }
}

