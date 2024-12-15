using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class Pandemic
{
    public string Event;
    public string Date;
    public string Location;
    public string Country;
    public string Pathogen;
    public string RealPathogen;
    public string Confirmed;
    public string DeathToll;
    public string PopulationPercentage;

    public Pandemic(string eventName, string date, string location, string country, string pathogen, string realpathogen, string confirmed, string deathToll, string populationPercentage)
    {
        Event = eventName;
        Date = date;
        Location = location;
        Country = country;
        Pathogen = pathogen;
        RealPathogen = realpathogen;
        Confirmed = confirmed;
        DeathToll = deathToll;
        PopulationPercentage = populationPercentage;
    }
}

public class PandemicDatabase : MonoBehaviour
{
    public TextAsset csvFile;  // Assign the CSV file in the inspector
    private List<Pandemic> pandemics = new List<Pandemic>();

    void Start()
    {
        LoadPandemicData();
    }

    // Function to load data from the CSV file
    void LoadPandemicData()
    {
        string[] lines = csvFile.text.Split('\n');
        for (int i = 1; i < lines.Length; i++) // Skip the header row
        {
            string[] values = lines[i].Split(';');
            if (values.Length == 11)
            {
                Pandemic newPandemic = new Pandemic(values[1], values[2], values[3], values[4], values[5], values[6], values[7], values[8], values[9]);
                pandemics.Add(newPandemic);
            }
        }
        
    }

    // Function to get pandemics by year (you can modify this based on your specific needs)
    public List<Pandemic> GetPandemicsByYear(int year)
    {
        List<Pandemic> result = new List<Pandemic>();
        foreach (Pandemic pandemic in pandemics)
        {
            // If the date range is in a "startYear-endYear" format
            string[] years = pandemic.Date.Split('-');

            if (years.Length == 2) // If it's a range, check if the year is within the range
            {
                int startYear, endYear;
                if (int.TryParse(years[0], out startYear) && int.TryParse(years[1], out endYear))
                {
                    if (year >= startYear && year <= endYear) // Check if the selected year falls within the range
                    {
                        result.Add(pandemic);
                    }
                }
            }
            int singleYear;
            if (int.TryParse(pandemic.Date, out singleYear) && singleYear == year)
            {
                result.Add(pandemic);
            }
        }
        return result;
    }
}

