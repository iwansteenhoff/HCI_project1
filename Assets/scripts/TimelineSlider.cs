using UnityEngine;
using UnityEngine.UI;
using TMPro; // Include this for TMP_Text support

public class TimelineSlider : MonoBehaviour
{
    public UnityEngine.UI.Slider timelineSlider; // Reference to the UI Slider
    public TMP_Text yearText;                    // Reference to the TextMeshPro UI Text element
    public bool isSliderActive;

    void Start()
    {
        // Set initial year display
        UpdateYear(timelineSlider.value);

        // Add listener for slider value changes
        timelineSlider.onValueChanged.AddListener(UpdateYear);
    }

    void UpdateYear(float value)
    {
        int year = Mathf.RoundToInt(value); // Ensure it's an integer
        yearText.text = "Year: " + year;
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
}