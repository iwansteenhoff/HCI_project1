using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DynamicScrollView : MonoBehaviour
{
    public GameObject buttonPrefab;      // The button prefab to instantiate.
    public Transform contentTransform;   // The content transform where buttons will be placed.
    public ScrollRect scrollRect;        // Reference to ScrollRect (from ScrollView).

    // Sample data (you can replace this with database calls)
    private List<string> sampleData = new List<string>
    {
        "Button 1", "Button 2", "Button 3", "Button 4", "Button 5", "Button 6", "Button 7"
    };

    void Start()
    {
        // Check if buttonPrefab is assigned
        if (buttonPrefab == null)
        {
            Debug.LogError("Button Prefab is not assigned!");
            return;
        }
        else
        {
            Debug.Log("Button Prefab is assigned.");
        }

        // Check if contentTransform is assigned
        if (contentTransform == null)
        {
            Debug.LogError("Content Transform is not assigned!");
            return;
        }
        else
        {
            Debug.Log("Content Transform is assigned.");
        }

        // Check if scrollRect is assigned
        if (scrollRect == null)
        {
            Debug.LogError("ScrollRect is not assigned!");
            return;
        }
        else
        {
            Debug.Log("ScrollRect is assigned.");
        }

        PopulateScrollView();
    }

    void PopulateScrollView()
    {
        // Clear existing buttons
        foreach (Transform child in contentTransform)
        {
            Destroy(child.gameObject);
        }

        Debug.Log("Existing buttons cleared.");

        // Populate with buttons based on sample data
        foreach (string data in sampleData)
        {
            if (buttonPrefab == null)
            {
                Debug.LogError("Button Prefab is null during instantiation!");
                return;
            }

            GameObject button = Instantiate(buttonPrefab, contentTransform);
            if (button == null)
            {
                Debug.LogError("Failed to instantiate the button prefab.");
                return;
            }

            Text buttonText = button.GetComponentInChildren<Text>();
            if (buttonText != null)
            {
                buttonText.text = data; // Assign data to button text
            }
            else
            {
                Debug.LogError("Button prefab does not contain a Text component.");
            }

            button.GetComponent<Button>().onClick.AddListener(() => OnButtonClick(data)); // Optional button click event
            Debug.Log($"Button created: {data}");
        }

        // Debugging step to check the contentTransform component
        RectTransform contentRectTransform = contentTransform.GetComponent<RectTransform>();
        if (contentRectTransform == null)
        {
            Debug.LogError("Content Transform does not have a RectTransform component.");
            return;
        }
        else
        {
            Debug.Log("Content Transform has a RectTransform component.");
        }

        // Force content size recalculation
        Debug.Log("Forcing layout rebuild...");
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentRectTransform);
        Debug.Log("Layout rebuilt");

        // Debug the width of the content after layout rebuild
        Debug.Log("Content Width: " + contentRectTransform.rect.width);

        // Optional: Reset scroll position to the start
        if (scrollRect != null)
        {
            scrollRect.horizontalNormalizedPosition = 0;
        }
    }

    // Example of what happens when a button is clicked
    void OnButtonClick(string buttonData)
    {
        Debug.Log("Button clicked: " + buttonData);
    }
}
