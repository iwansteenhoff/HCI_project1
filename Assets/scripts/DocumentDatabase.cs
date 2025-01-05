using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class Documents
{
    public string Epidemic;
    public string DocumentSource;
    public string Type;
    public string Subtype;
    public string Title;
    public string Author;
    public string Comments;

    // Constructor to initialize all fields
    public Documents(string epidemic, string documentSource, string type, string subtype, string title, string author, string comments)
    {
        Epidemic = epidemic;
        DocumentSource = documentSource;
        Type = type;
        Subtype = subtype;
        Title = title;
        Author = author;
        Comments = comments;
    }
}

public class DocumentDatabase : MonoBehaviour
{
    public TextAsset csvFile;  // Assign the CSV file in the inspector
    public List<Documents> documentsList = new List<Documents>();  // Changed to store Documents

    // Dictionary to hold lists of data grouped by the Epidemic
    public Dictionary<string, List<string>> epidemicDocumentSources = new Dictionary<string, List<string>>();
    public Dictionary<string, List<string>> epidemicTypes = new Dictionary<string, List<string>>();
    public Dictionary<string, List<string>> epidemicSubtypes = new Dictionary<string, List<string>>();
    public Dictionary<string, List<string>> epidemicTitles = new Dictionary<string, List<string>>();
    public Dictionary<string, List<string>> epidemicAuthors = new Dictionary<string, List<string>>();
    public Dictionary<string, List<string>> epidemicComments = new Dictionary<string, List<string>>();

    void Start()
    {
        LoadDocumentData();  // Renamed function to reflect the new context
    }

    // Function to load data from the CSV file
    void LoadDocumentData()
    {
        string[] lines = csvFile.text.Split('\n');

        // Loop through all the lines of the CSV (starting from 1 to skip the header)
        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(';');

            // Ensure that the row contains the expected number of columns (7 data columns)
            if (values.Length == 7)
            {
                // Create a new Documents object
                Documents newDocument = new Documents(values[0], values[1], values[2], values[3], values[4], values[5], values[6]);
                documentsList.Add(newDocument);

                // Grouping the rows by Epidemic and adding to the respective dictionaries
                if (!epidemicDocumentSources.ContainsKey(newDocument.Epidemic))
                {
                    epidemicDocumentSources[newDocument.Epidemic] = new List<string>();
                    epidemicTypes[newDocument.Epidemic] = new List<string>();
                    epidemicSubtypes[newDocument.Epidemic] = new List<string>();
                    epidemicTitles[newDocument.Epidemic] = new List<string>();
                    epidemicAuthors[newDocument.Epidemic] = new List<string>();
                    epidemicComments[newDocument.Epidemic] = new List<string>();
                }

                epidemicDocumentSources[newDocument.Epidemic].Add(newDocument.DocumentSource);
                epidemicTypes[newDocument.Epidemic].Add(newDocument.Type);
                epidemicSubtypes[newDocument.Epidemic].Add(newDocument.Subtype);
                epidemicTitles[newDocument.Epidemic].Add(newDocument.Title);
                epidemicAuthors[newDocument.Epidemic].Add(newDocument.Author);
                epidemicComments[newDocument.Epidemic].Add(newDocument.Comments);
            }
            else
            {
                Debug.LogWarning("Row does not contain the expected number of columns.");
            }
        }

        // Optionally, print the grouped data to check
        foreach (var epidemic in epidemicDocumentSources.Keys)
        {
            Debug.Log($"Epidemic: {epidemic}");
            Debug.Log($"Document Sources: {string.Join(", ", epidemicDocumentSources[epidemic])}");
            Debug.Log($"Types: {string.Join(", ", epidemicTypes[epidemic])}");
            Debug.Log($"Subtypes: {string.Join(", ", epidemicSubtypes[epidemic])}");
            Debug.Log($"Titles: {string.Join(", ", epidemicTitles[epidemic])}");
            Debug.Log($"Authors: {string.Join(", ", epidemicAuthors[epidemic])}");
            Debug.Log($"Comments: {string.Join(", ", epidemicComments[epidemic])}");
        }
    }
}
