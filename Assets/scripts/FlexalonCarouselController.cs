using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // For TextMeshPro
using Flexalon; // Ensure Flexalon is in your project

public class FlexalonCarouselController : MonoBehaviour
{
    public GameObject carouselItemPrefab;  // The prefab used for each carousel item
    public Transform carouselParent;       // The parent object for the carousel (should have FlexalonCircleLayout)

    // Placeholder data
    private string[] itemData = { "Item 1", "Item 2", "Item 3", "Item 4", "Item 5" };
    private bool[] itemColors = { true, false, true, false, true }; // Alternating colors

    void Start()
    {
        CreateCarouselItems();
        EnsureNoScalingIssues();
    }

    void CreateCarouselItems()
    {
        // Ensure we clear the parent for a fresh start
        foreach (Transform child in carouselParent)
        {
            Destroy(child.gameObject);
        }

        // Create and populate items
        for (int i = 0; i < itemData.Length; i++)
        {
            // Instantiate item
            GameObject newItem = Instantiate(carouselItemPrefab, carouselParent);

            // Set the tile's text
            TextMeshPro textComponent = newItem.GetComponentInChildren<TextMeshPro>();
            if (textComponent)
            {
                textComponent.text = itemData[i];
            }

            // Set the tile's color
            Renderer tileRenderer = newItem.GetComponentInChildren<Renderer>();
            if (tileRenderer)
            {
                tileRenderer.material.color = itemColors[i] ? Color.white : Color.black;
            }

            // Set the text color to contrast the background
            if (textComponent)
            {
                textComponent.color = itemColors[i] ? Color.black : Color.white;
            }

            // Reset the scale immediately after instantiation (just in case)
            newItem.transform.localScale = Vector3.one;
        }

        // Allow Flexalon to layout the items
    }

    void EnsureNoScalingIssues()
    {
        // Loop through each child of carouselParent and reset scale to 1
        foreach (Transform child in carouselParent)
        {
            child.localScale = Vector3.one;
        }
    }
}
