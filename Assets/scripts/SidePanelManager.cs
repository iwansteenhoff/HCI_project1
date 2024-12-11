using UnityEngine;
using TMPro; // For TextMeshPro UI elements
using System.Collections.Generic;

public class SidePanelManager : MonoBehaviour
{
    public TMP_Text panelText;

    public void UpdatePanel(string countryName, List<EpidemicEvent> epidemics)
    {
        panelText.text = $"Country: {countryName}\nEpidemics:\n";

        if (epidemics.Count > 0)
        {
            foreach (var epidemic in epidemics)
            {
                panelText.text += $"- {epidemic.Name}\n";
            }
        }
        else
        {
            panelText.text += "No epidemics recorded for this year.";
        }
    }
}
