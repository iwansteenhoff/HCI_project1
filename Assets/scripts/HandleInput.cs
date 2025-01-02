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
    public UnityEngine.UI.Slider timelineSliderObject;
    public VirusImageController virusImageController;
    private List<string> pandemicnames = new List<string> {"antonineplague","plagueofjustinian","japanesesmallpox",
    "blackdeath","mexicosmallpox","spainplague",
    "wyandotpeople","greatplagueinthelatemingdynasty","icelandsmallpox",
    "greatnorthernwarplagueoutbreak","greatplague","persianplague","northamericansmallpox",
    "saint-domingueyellowfever","russiafirsttyphus","firstcholera",
    "groningen","northamericantyphus","hawaiiepidemicofinfections",
    "europesmallpox","ugandaafricantrypanosomiasis",
    "papuanewguineakuru","influenza",
    "russiasecondtyphus","egyptcholera","hongkongflu",
    "hiv/aids","latinamericacholera","sarsoutbreak",
    "philippinesdengue","westafricanmeningitisoutbreak","middleeastrespiratorysyndromecoronavirusoutbreak",
    "westernafricanebolavirus","angolaanddemocraticrepublicofthecongoyellowfeveroutbreak","covid-19"};

    public TMP_Dropdown dropdown;
    public List<string> allOptions = new List<string> {"Antonine Plague", "Plague of Justinian", "Japanese smallpox epidemic", "Black Death",
        "Mexico smallpox epidemic", "Spain plague epidemic", "Wyandot people epidemic", "Great Plague in the late Ming dynasty",
        "Iceland smallpox epidemic", "Great Northern War plague outbreak", "Great Plague", "Persian Plague", "North American smallpox epidemic",
        "Saint-Domingue yellow fever epidemic", "Russia first typhus epidemic", "First cholera pandemic", "Groningen epidemic",
        "North American typhus epidemic", "Hawaii epidemic of infections", "Europe smallpox epidemic", "Uganda African trypanosomiasis epidemic",
        "Papua New Guinea kuru epidemic", "influenza pandemic", "Russia second typhus epidemic", "Egypt cholera epidemic", "Hong Kong flu",
        "HIV/AIDS pandemic", "Latin America cholera epidemic", "SARS outbreak", "Philippines dengue epidemic", "West African meningitis outbreak",
        "Middle East respiratory syndrome coronavirus outbreak", "Western African Ebola virus epidemic",
        "Angola and Democratic Republic of the Congo yellow fever outbreak", "COVID-19 pandemic"};
    

    private void Start()
    {
        if (userInputField != null)
        {
            dropdown.gameObject.SetActive(false);
            userInputField.onEndEdit.AddListener(CheckInputAgainstPandemics);
            userInputField.onValueChanged.AddListener(OnInputValueChanged);
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
        Debug.Log(userInput);
        string processedInput = CleanInput(userInput);
        List<Pandemic> pandemics = pandemicDatabase.pandemics;
        Debug.Log($"sdfsdf{processedInput}");
        // Check for a match
        foreach (var name in pandemicnames)
        {
            
            if (name.Equals(processedInput, System.StringComparison.OrdinalIgnoreCase))
            {
                Pandemic matchedpandemic = pandemics[pandemicnames.IndexOf(name)];
                Debug.Log($"Match found: {matchedpandemic.Event}");
                string[] years = matchedpandemic.Date.Split("-");
                if (years.Length == 2)
                {
                    int firstyear = int.Parse(years[0]);
                    int secondyear = int.Parse(years[1]);
                    if (timelineSliderObject.value >= firstyear && timelineSliderObject.value <= secondyear)
                    {
                        pandemicInfoText.text = FormatPandemicInfo(matchedpandemic);
                        virusImageController.SetVirusImage(matchedpandemic.Pathogen);
                        return;
                    }
                }
                
                int thirdyear = int.Parse(years[0]);
                if (timelineSliderObject.value == thirdyear)
                {
                    pandemicInfoText.text = FormatPandemicInfo(matchedpandemic);
                    virusImageController.SetVirusImage(matchedpandemic.Pathogen);
                    return;
                }
                
                timelineSlider.SetSliderToYear(thirdyear);
                pandemicInfoText.text = FormatPandemicInfo(matchedpandemic);
                virusImageController.SetVirusImage(matchedpandemic.Pathogen);

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
                Debug.Log(year);
                
                int year_3 = int.Parse(years[0]);
                Debug.Log(year_3);
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

    public void SetTextToSelectedCountry(string country)
    {
        List<string> confirmedpandemics = new List<string>();
        foreach (Pandemic pandemic in pandemicDatabase.pandemics)
        {
            string countries = pandemic.Country;
            string[] listcountries = countries.Split("&");
            foreach (string potentialcountry in listcountries)
            {
                if (country == potentialcountry)
                {
                    confirmedpandemics.Add(pandemic.Event);
                }
                if (potentialcountry == "World")
                {
                    confirmedpandemics.Add(pandemic.Event);
                }
            }
        }
        string begintext = $"The pandemics that effected {country} are: ";
        foreach (string confirmedpandemic in confirmedpandemics)
        {
            begintext += $"{confirmedpandemic}, ";
        }
        pandemicInfoText.text = begintext;
    }
    void OnInputValueChanged(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            dropdown.gameObject.SetActive(false);
            return;
        }

        // Filter options based on input
        List<string> filteredOptions = allOptions.FindAll(option => option.ToLower().Contains(input.ToLower()));

        if (filteredOptions.Count > 0)
        {
            // Update Dropdown Options
            dropdown.ClearOptions();
            dropdown.AddOptions(filteredOptions);
            dropdown.options.Insert(0, new TMP_Dropdown.OptionData("Select a pandemic...")); // Add placeholder at the top
            dropdown.value = 0; // Set the placeholder as the default selected option
            dropdown.captionText.text = dropdown.options[0].text; // Display placeholder text
            dropdown.gameObject.SetActive(true);  // Show dropdown
        }
        else
        {
            dropdown.gameObject.SetActive(false);
        }
        
        // Select a suggestion if clicked
        dropdown.onValueChanged.AddListener(index =>
        {

            if (index >= 0 && index < dropdown.options.Count)
            {
                string selectedOption = dropdown.options[index].text; // Safely access the option
                Debug.Log(selectedOption);

                userInputField.text = selectedOption;
                CheckInputAgainstPandemics(selectedOption); // Pass the selected option text
                dropdown.gameObject.SetActive(false);
            }
            else
            {
                Debug.LogWarning("Dropdown index out of bounds!");
            }
        });
    }
}
