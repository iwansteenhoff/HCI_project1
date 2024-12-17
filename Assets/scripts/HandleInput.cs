using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HandleInput : MonoBehaviour
{
    public TMP_Text pandemicInfoText;
    public TMP_InputField userInputField; // Drag the InputField from the Inspector
    public PandemicDatabase pandemicDatabase; // Reference to your pandemic database
    public TimelineSlider timelineSlider;
    private List<string> pandemicnames = new List<string> {"antonineplague","plagueofjustinian","japanesesmallpox",
    "blackdeath","mexicosmallpox","spainplague",
    "wyandotpeople","greatplagueinthelatemingdynasty","icelandsmallpox",
    "greatnorthernwarplagueoutbreak","greatplague","persianplague","northamericansmallpox",
    "saint-domingueyellowfever","russiatyphus","firstcholera",
    "groningen","northamericantyphus","hawaiiepidemicofinfections",
    "europesmallpox","ugandaafricantrypanosomiasis",
    "papuanewguineakuru","influenza",
    "russiatyphus","egyptcholera","hongkongflu",
    "hiv/aids","latinamericacholera","sarsoutbreak",
    "philippinesdengue","westafricanmeningitisoutbreak","middleeastrespiratorysyndromecoronavirusoutbreak",
    "westernafricanebolavirus","angolaanddemocraticrepublicofthecongoyellowfeveroutbreak","covid-19"};

    private void Start()
    {
        if (userInputField != null)
        {
            userInputField.onEndEdit.AddListener(CheckInputAgainstPandemics);
        }
    }

    string CleanInput(string input)
    {
        // Convert to lowercase, remove spaces, and strip "pandemic" and "epidemic"
        string cleanedInput = input.ToLower().Replace(" ", "");
        cleanedInput = cleanedInput.Replace("pandemic", "").Replace("epidemic", "");
        return cleanedInput;
    }

    string FormatPandemicInfo(Pandemic pandemic)
    {
        return $"<b>{pandemic.Event}</b>\n" +
               $"Date: {pandemic.Date}\n" +
               $"Location: {pandemic.Location}\n" +
               $"Pathogen: {pandemic.RealPathogen}\n" +
               $"Death Toll: {pandemic.DeathToll}\n" +
               $"Population Affected: {pandemic.PopulationPercentage}";
    }

    void CheckInputAgainstPandemics(string userInput)
    {
        if (string.IsNullOrWhiteSpace(userInput))
        {
            Debug.Log("Input is empty or null.");
            return;
        }

        string processedInput = CleanInput(userInput);
        List<Pandemic> pandemics = pandemicDatabase.pandemics;

        // Check for a match
        foreach (var name in pandemicnames)
        {
            
            if (name.Equals(processedInput, System.StringComparison.OrdinalIgnoreCase))
            {
                Pandemic matchedpandemic = pandemics[pandemicnames.IndexOf(name)];
                Debug.Log($"Match found: {matchedpandemic.Event}");
                string[] years = matchedpandemic.Date.Split("-");
                int firstyear = int.Parse(years[0]);
                timelineSlider.SetSliderToYear(firstyear);
                pandemicInfoText.text = FormatPandemicInfo(matchedpandemic);

                return; // Exit after finding the first match
            }
        }

        Debug.Log("No match found for the input.");
    }

    public void SetTextToSelectedVirus(string tagcountry, float year)
    {
        foreach (Pandemic pandemic in pandemicDatabase.pandemics)
        {
            string[] years = pandemic.Date.Split('-');
            if (years.Length == 2)
            {
                int year_1 = int.Parse(years[0]);
                int year_2 = int.Parse(years[1]);
                if (year >= year_1 && year <= year_2)
                {
                    if (pandemic.Pathogen == tagcountry)
                    {
                        CheckInputAgainstPandemics(pandemic.Event);
                        return;
                    }
                }
            }
            
            else
            {
                int year_3 = int.Parse(years[0]);
                if (year_3 == year)
                {
                    if (pandemic.Pathogen == tagcountry)
                    {
                        CheckInputAgainstPandemics(pandemic.Event);
                        return;
                    }
                }    
                
            }
        }
    }
}
