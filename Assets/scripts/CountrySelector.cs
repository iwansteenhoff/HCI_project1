using UnityEngine;
using System.Collections.Generic; // For working with lists

public class CountrySelector : MonoBehaviour
{
    private Material[] originalMaterials;
    public Material highlightMaterial; // Material for highlighting the selected country

    private Transform parentCountry;

    public MenuArea menuArea; // Reference to the MenuArea script
    public EpidemicData epidemicData; // Reference to the EpidemicData script
    public SidePanelManager sidePanelManager; // Reference to the SidePanelManager
    public TimelineSlider timelineSlider; // Reference to the TimelineSlider script

    void Start()
    {
        parentCountry = transform.parent;
        // Assign the highlight material programmatically
        highlightMaterial = Resources.Load<Material>("Materials/selectioncountry");
        if (highlightMaterial == null)
        {
            Debug.LogError("HighlightMaterial not found. Ensure it exists in a 'Resources/Materials' folder.");
        }
        if (timelineSlider == null)
        {
            timelineSlider = FindObjectOfType<TimelineSlider>();
            if (timelineSlider == null)
            {
                Debug.LogError("TimelineSlider not found in the scene.");
            }
        }
        if (epidemicData == null)
        {
            Debug.LogError("EpidemicData is not assigned in CountrySelector.");
            epidemicData = FindObjectOfType<EpidemicData>();
        }
        if (sidePanelManager == null)
        {
            Debug.LogError("Side panel manager is not assigned in CountrySelector.");
            sidePanelManager = FindObjectOfType<SidePanelManager>();
        }
    }

    void OnMouseEnter()
    {
        if (menuArea != null && menuArea.isMouseOverMenu) return;
        // Highlight all child meshes
        MeshRenderer[] renderers = parentCountry.GetComponentsInChildren<MeshRenderer>();
        originalMaterials = new Material[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            originalMaterials[i] = renderers[i].material;
            renderers[i].material = highlightMaterial;
        }
    }

    void OnMouseExit()
    {
        if (menuArea != null && menuArea.isMouseOverMenu) return;
        // Revert all child meshes to their original material
        MeshRenderer[] renderers = parentCountry.GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material = originalMaterials[i];
        }
    }

    void OnMouseDown()
    {
        if (menuArea != null && menuArea.isMouseOverMenu) return;

        string countryName = gameObject.name;
        Debug.Log($"Selected Country: {countryName}");

        // Remove the "_Polygon" suffix if it's part of the clicked country name
        if (countryName.EndsWith("_Polygon"))
        {
            countryName = countryName.Substring(0, countryName.Length - "_Polygon".Length);
        }

        // Check if TimelineSlider is assigned
        if (timelineSlider == null)
        {
            Debug.LogError("TimelineSlider is not assigned.");
            return;
        }

        // Get the selected year from the timeline slider
        int selectedYear = timelineSlider.GetSelectedYear();
        Debug.Log($"Selected Year: {selectedYear}");

        // Check if EpidemicData is assigned
        if (epidemicData == null)
        {
            Debug.LogError("EpidemicData is not assigned.");
            return;
        }

        List<EpidemicEvent> epidemics = epidemicData.GetEpidemicsByYear(selectedYear);
        Debug.Log($"Found {epidemics.Count} epidemics for the year {selectedYear}");

        // Check if SidePanelManager is assigned
        if (sidePanelManager == null)
        {
            Debug.LogError("SidePanelManager is not assigned.");
            return;
        }

        List<EpidemicEvent> epidemicsInCountry = new List<EpidemicEvent>();
        foreach (var epidemic in epidemics)
        {
            // Debug the locations to verify the format
            Debug.Log($"Checking epidemic: {epidemic.Name} in locations: {string.Join(", ", epidemic.Locations)}");

            // Use ToLower() to ensure case-insensitive comparison
            if (epidemic.Locations.Exists(location => location.ToLower() == countryName.ToLower()))
            {
                epidemicsInCountry.Add(epidemic);
            }
        }

        Debug.Log($"Found {epidemicsInCountry.Count} epidemics in {countryName}");

        // Update the side panel with the selected country's epidemics
        sidePanelManager.UpdatePanel(countryName, epidemicsInCountry);
    }
}
