using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class AutocompleteDropdown : MonoBehaviour
{
    public TMP_InputField inputField;
    public TMP_Dropdown dropdown;

    // List of all possible options
    public List<string> allOptions = new List<string> {"Antonine Plague", "Plague of Justinian", "Japanese smallpox epidemic", "Black Death",
        "Mexico smallpox epidemic", "Spain plague epidemic", "Wyandot people epidemic", "Great Plague in the late Ming dynasty",
        "Iceland smallpox epidemic", "Great Northern War plague outbreak", "Great Plague", "Persian Plague", "North American smallpox epidemic",
        "Saint-Domingue yellow fever epidemic", "Russia first typhus epidemic", "First cholera pandemic", "Groningen epidemic",
        "North American typhus epidemic", "Hawaii epidemic of infections", "Europe smallpox epidemic", "Uganda African trypanosomiasis epidemic",
        "Papua New Guinea kuru epidemic", "influenza pandemic", "Russia second typhus epidemic", "Egypt cholera epidemic", "Hong Kong flu",
        "HIV/AIDS pandemic", "Latin America cholera epidemic", "SARS outbreak", "Philippines dengue epidemic", "West African meningitis outbreak",
        "Middle East respiratory syndrome coronavirus outbreak", "Western African Ebola virus epidemic",
        "Angola and Democratic Republic of the Congo yellow fever outbreak", "COVID-19 pandemic"};

    void Start()
    {
        dropdown.gameObject.SetActive(false);  // Hide dropdown initially
        inputField.onValueChanged.AddListener(OnInputValueChanged);
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
            dropdown.gameObject.SetActive(true);  // Show dropdown
        }
        else
        {
            dropdown.gameObject.SetActive(false);
        }

        // Select a suggestion if clicked
        dropdown.onValueChanged.AddListener(index =>
        {
            inputField.text = dropdown.options[index].text;
            dropdown.gameObject.SetActive(false);
        });
    }
}

