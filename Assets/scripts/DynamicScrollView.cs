using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class DynamicScrollView : MonoBehaviour
{
    public GameObject buttonPrefab;      // The button prefab to instantiate.
    public Transform contentTransform;   // The content transform where buttons will be placed.
    public ScrollRect scrollRect;        // Reference to ScrollRect (from ScrollView).
    public DocumentDatabase documentDatabase;  // Reference to DocumentDatabase to get document data
    public GameObject popupPrefab;
    private string selectedEpidemic;     // Store the selected epidemic name

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

    public void UpdateScrollViewForEpidemic(string epidemicName)
    {
        Debug.Log("Updating Scroll View for Epidemic: " + epidemicName);
        selectedEpidemic = epidemicName;  // Update the selected epidemic

        // Populate the scroll view based on the selected epidemic
        PopulateScrollView();
    }

    void PopulateScrollView()
    {
        // Clear existing buttons
        Debug.Log("Clearing existing buttons...");
        foreach (Transform child in contentTransform)
        {
            Destroy(child.gameObject);
        }
        Debug.Log("Existing buttons cleared.");

        // Fetch documents for the selected epidemic
        if (string.IsNullOrEmpty(selectedEpidemic))
        {
            Debug.LogError("Selected Epidemic is null or empty.");
            return;
        }

        List<Documents> epidemicDocuments = documentDatabase.GetDocumentsForEpidemic(selectedEpidemic);
        if (epidemicDocuments == null || epidemicDocuments.Count == 0)
        {
            Debug.LogWarning($"No documents found for epidemic: {selectedEpidemic}");
            return;
        }

        Debug.Log($"Found {epidemicDocuments.Count} documents for epidemic: {selectedEpidemic}");

        // Create a button for each document
        foreach (Documents doc in epidemicDocuments)
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

            // Try to find the required TextMeshProUGUI components
            TextMeshProUGUI titleText = button.transform.Find("TitleText")?.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI authorText = button.transform.Find("AuthorText")?.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI typeText = button.transform.Find("TypeText")?.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI subtypeText = button.transform.Find("SubtypeText")?.GetComponent<TextMeshProUGUI>();

            if (titleText == null)
            {
                Debug.LogError("TitleText component not found in button prefab.");
            }
            if (authorText == null)
            {
                Debug.LogError("AuthorText component not found in button prefab.");
            }
            if (typeText == null)
            {
                Debug.LogError("TypeText component not found in button prefab.");
            }
            if (subtypeText == null)
            {
                Debug.LogError("SubtypeText component not found in button prefab.");
            }

            // Apply truncated title if it's longer than 35 characters
            if (titleText != null)
            {
                titleText.text = TruncateTitle(doc.Title, 35);
            }

            if (authorText != null) authorText.text = doc.Author;
            if (typeText != null) typeText.text = doc.Type;
            if (subtypeText != null) subtypeText.text = doc.Subtype;

            Button buttonComponent = button.GetComponent<Button>();
            if (buttonComponent != null)
            {
                buttonComponent.onClick.AddListener(() => OnButtonClick(doc));
                Debug.Log($"Button created for document: {doc.Title}");
            }
            else
            {
                Debug.LogError("Button component not found on the button prefab.");
            }
        }

        // Force content size recalculation
        RectTransform contentRectTransform = contentTransform.GetComponent<RectTransform>();
        if (contentRectTransform != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentRectTransform);
            Debug.Log("Layout rebuilt");
        }
        else
        {
            Debug.LogError("Content Transform does not have a RectTransform component.");
        }

        // Optional: Reset scroll position to the start
        if (scrollRect != null)
        {
            scrollRect.horizontalNormalizedPosition = 0;
            Debug.Log("Scroll position reset to start.");
        }
        else
        {
            Debug.LogError("ScrollRect is not assigned.");
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentTransform.GetComponent<RectTransform>());
    }

    // Truncates the title to a maximum length of maxLength and adds "..." if necessary
    string TruncateTitle(string title, int maxLength)
    {
        if (title.Length <= maxLength)
        {
            return title;  // If title is shorter than maxLength, return it as is
        }

        // Find the last space within the maxLength limit
        int lastSpaceIndex = title.LastIndexOf(' ', maxLength);

        if (lastSpaceIndex == -1)
        {
            // No space found, truncate directly at maxLength
            return title.Substring(0, maxLength) + "...";
        }

        // Truncate at the last space and add "..."
        return title.Substring(0, lastSpaceIndex) + "...";
    }

    void ShowPopup(Documents document)
    {
        // Instantiate the popup prefab under the correct parent (ensure it's part of the UI)
        GameObject popup = Instantiate(popupPrefab, transform.root);  // Using 'transform.root' to attach to the root canvas

        // Ensure the popup's RectTransform is set up correctly
        RectTransform popupRectTransform = popup.GetComponent<RectTransform>();
        if (popupRectTransform != null)
        {
            // Optional: Reset anchors and position if necessary
            popupRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            popupRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            popupRectTransform.anchoredPosition = Vector2.zero;
        }

        // Set the metadata in the popup (like title, comment, hyperlink, etc.)
        TextMeshProUGUI popupTitle = popup.transform.Find("popupTitle")?.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI popupComment = popup.transform.Find("popupComment")?.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI hyperlinkText = popup.transform.Find("hyperlinkText")?.GetComponent<TextMeshProUGUI>();

        if (popupTitle != null) popupTitle.text = document.Title;
        if (popupComment != null) popupComment.text = document.Comments;
        if (hyperlinkText != null) hyperlinkText.text = document.DocumentSource;

        // Find and set up the close button ("X")
        Button closeButton = popup.transform.Find("CloseButton")?.GetComponent<Button>();
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(() => ClosePopup(popup));
        }
        else
        {
            Debug.LogError("Close button (X) not found in popup prefab.");
        }
    }


    void ClosePopup(GameObject popup)
    {
        Destroy(popup);
    }
    // Example of what happens when a button is clicked
    void OnButtonClick(Documents document)
    {
        Debug.Log($"Button clicked: {document.Title}");
        ShowPopup(document);
    }
}
