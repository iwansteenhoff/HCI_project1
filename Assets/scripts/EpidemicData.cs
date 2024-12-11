using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EpidemicEvent
{
    public string Name; // Epidemic name
    public int StartYear;
    public int EndYear;
    public List<string> Locations; // List of countries affected
}

public class EpidemicData : MonoBehaviour
{
    public List<EpidemicEvent> epidemicEvents = new List<EpidemicEvent>();

    void Start()
    {
        LoadEpidemicData("EpidemicsDatabase.csv");
    }

    void LoadEpidemicData(string fileName)
    {
        TextAsset csvFile = Resources.Load<TextAsset>(fileName);
        if (csvFile == null)
        {
            Debug.LogError($"CSV file {fileName} not found in Resources folder!");
            return;
        }

        string[] lines = csvFile.text.Split('\n');
        Debug.Log($"Total lines read from CSV: {lines.Length}");

        for (int i = 1; i < lines.Length; i++) // Skip the header row
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue;

            string[] values = line.Split(',');

            // Debugging the raw CSV line and split values
            Debug.Log($"Line {i}: {line}");
            Debug.Log($"Values: {string.Join(", ", values)}");

            // Parse fields
            string name = values[0];
            string[] dateRange = values[1].Split('–');
            int startYear = int.Parse(dateRange[0]);
            int endYear = dateRange.Length > 1 ? int.Parse(dateRange[1]) : startYear;
            List<string> locations = new List<string>(values[2].Split(';'));

            // Debugging parsed fields
            Debug.Log($"Parsed Epidemic: {name} from {startYear} to {endYear}");
            Debug.Log($"Locations: {string.Join(", ", locations)}");

            epidemicEvents.Add(new EpidemicEvent
            {
                Name = name,
                StartYear = startYear,
                EndYear = endYear,
                Locations = locations
            });
        }
    }


    public List<EpidemicEvent> GetEpidemicsByYear(int year)
    {
        return epidemicEvents.FindAll(e => e.StartYear <= year && e.EndYear >= year);
    }
}