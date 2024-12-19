using UnityEngine;
using UnityEngine.UI;
using TMPro; // Include this for TMP_Text support
using System.Collections.Generic;

public class TimelineSlider : MonoBehaviour
{
    public UnityEngine.UI.Slider timelineSlider; // Reference to the UI Slider
    public TMP_Text yearText;                    // Reference to the TextMeshPro UI Text element
    public bool isSliderActive;
    public PandemicDatabase pandemicDatabase;
    public WorldMap worldMap;

    void Start()
    {
        pandemicDatabase = FindObjectOfType<PandemicDatabase>();
        // Set initial year display
        UpdateYear(timelineSlider.value);

        // Add listener for slider value changes
        timelineSlider.onValueChanged.AddListener(UpdateYear);
    }

    public void SetSliderToYear(int year)
    {
        // Ensure the year is within the slider's range
        if (year >= timelineSlider.minValue && year <= timelineSlider.maxValue)
        {
            timelineSlider.value = year; // Set slider position
            UpdateYear(timelineSlider.value); // Manually call UpdateYear to trigger updates
        }
        else
        {
            Debug.LogWarning($"Year {year} is out of range. Slider range is {timelineSlider.minValue} to {timelineSlider.maxValue}.");
        }
    }

    void UpdateYear(float value)
    {
        int year = Mathf.RoundToInt(value); // Ensure it's an integer
        yearText.text = "Year: " + year;
        DisplayPandemicsForYear(year);
    }

    public int GetSelectedYear()
    {
        return Mathf.RoundToInt(timelineSlider.value);  // Return the rounded value of the slider
    }
    // These methods will be called by the EventSystem
    public void OnPointerDown()
    {
            isSliderActive = true;
    }
    
    public void OnPointerUp()
    {
        isSliderActive = false;
    }
    void DisplayPandemicsForYear(int year)
    {
        Debug.Log("timeline slider is active");
        // Retrieve the pandemics for the selected year
        var pandemics = pandemicDatabase.GetPandemicsByYear(year);

        // Reset all previously marked countries to unselected
        worldMap.MarkAllCountriesNotselected();

        if (pandemics.Count == 0)
        {
            Debug.Log("No pandemics found for year: " + year);
        }
        else
        {
            // Create a list of tuples (CountryName, PathogenType)
            var countryPlagueList = new List<(string CountryName, string PathogenType)>();

            foreach (var pandemic in pandemics)
            {
                // Extract the country/region(s), split by '&' if multiple are listed
                string[] countries = pandemic.Country.Split('&');

                foreach (var countryName in countries)
                {
                    countryPlagueList.Add((countryName.Trim(), pandemic.Pathogen)); // Add tuple to list
                }
            }
            // Process the country-plague tuples
            foreach (var (countryName, pathogenType) in countryPlagueList)
            {
                try
                {
                    // Attempt to parse the country name to the Country enum
                    if (System.Enum.TryParse(countryName, out Country countryEnum))
                    {
                        // Mark the country as affected by the pandemic
                        worldMap.MarkPandemic(countryEnum, pathogenType);
                    }
                    else if (countryName == "World")
                    {
                        Debug.Log("Applying pandemic to the whole world.");
                        worldMap.SetAllCountriesToPandemic(pathogenType);
                    }
                    else
                    {
                        Debug.LogWarning($"Country '{countryName}' not recognized as a valid Country enum.");
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"Error processing country '{countryName}': {ex.Message}");
                }
            }
        }
    }
}