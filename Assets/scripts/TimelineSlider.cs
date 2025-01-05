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
    public TMP_InputField inputField; // Input field for keyboard input

    void Start()
    {
        pandemicDatabase = FindObjectOfType<PandemicDatabase>();
        // Set initial year display
        UpdateYear(timelineSlider.value);

        // Add listener for slider value changes
        timelineSlider.onValueChanged.AddListener(UpdateYear);

        if (inputField != null)
        {
            inputField.gameObject.SetActive(false);
            inputField.onEndEdit.AddListener(OnYearInputSubmit);
        }

        // Add double-click listener to the year text
        var textButton = yearText.GetComponentInParent<Button>(); // Ensure the TMP_Text is inside a Button parent
        if (textButton != null)
        {
            textButton.onClick.AddListener(OnYearTextDoubleClick);
        }

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
        // Retrieve the pandemics for the selected year
        var pandemics = pandemicDatabase.GetPandemicsByYear(year);

        // Reset all previously marked countries to unselected
        worldMap.MarkAllCountriesNotselectedOrHighlighted();

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

    

    public void OnYearTextDoubleClick()
    {
        yearText.gameObject.SetActive(false);
        inputField.gameObject.SetActive(true);
        inputField.ActivateInputField();
    }

    public void OnYearInputSubmit(string input)
    {
        if (int.TryParse(input, out int year))
        {
            SetSliderToYear(year);
        }
        else
        {
            Debug.LogWarning($"Invalid input: {input}");
        }

        inputField.gameObject.SetActive(false);
        yearText.gameObject.SetActive(true);
    }
}
