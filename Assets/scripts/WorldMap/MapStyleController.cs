using UnityEngine;
using TMPro;
public class MapStyleController : MonoBehaviour
{
    public InteractionModes interactionMode;
    public Color DefaultColorForCountries;
    public Color DefaultColorForHighlightedCountries;
    public Color DefaultColorForSelectedCountries;
    public Color DefaultColorForNonIntractableCountries;
    public Color DefaultColorForPandemicCountries;
    public Color DefaultColorForSmallpox;
    public Color DefaultColorForBubonicPlague;
    public Color DefaultColorForYellowFever;
    public Color DefaultColorForTyphus;
    public Color DefaultColorForCholera;
    public Color DefaultColorForMalaria;
    public Color DefaultColorForMeasles;
    public Color DefaultColorForAfricanTrypanosomiasis;
    public Color DefaultColorForKuru;
    public Color DefaultColorForInfluenza;
    public Color DefaultColorForHIV;
    public Color DefaultColorForSARS;
    public Color DefaultColorForDengueFever;
    public Color DefaultColorForMeningitis;
    public Color DefaultColorForMERSCoV;
    public Color DefaultColorForEbola;
    public Color DefaultColorForCOVID19;

    public bool ShowCountryNames = true;
    public Color FontColor;
    public Color32 StrokeColor;
    [Range(0,1f)]public float StrokeSize;
    public TMPro.FontStyles FontStyle;
    
   public void UpdateSettings()
{
    for(int x=0;x<transform.childCount ;x++)
        {
            Transform countryT = transform.GetChild(x).transform;
            if(countryT.tag == "Not Selected")
            countryT.GetComponent<SpriteRenderer>().color = DefaultColorForCountries;

            else if(countryT.tag == "Selected")
            countryT.GetComponent<SpriteRenderer>().color = DefaultColorForSelectedCountries;

            else if(countryT.tag == "Disabled")
            countryT.GetComponent<SpriteRenderer>().color = DefaultColorForNonIntractableCountries;

            else if (countryT.tag == "Pandemic") // Handle new Pandemic tag
                countryT.GetComponent<SpriteRenderer>().color = DefaultColorForPandemicCountries;

            else if (countryT.tag == "Smallpox") // Handle Smallpox tag
                countryT.GetComponent<SpriteRenderer>().color = DefaultColorForSmallpox;

            else if (countryT.tag == "Bubonic plague") // Handle Bubonic plague tag
                countryT.GetComponent<SpriteRenderer>().color = DefaultColorForBubonicPlague;

            else if (countryT.tag == "Yellow fever") // Handle Yellow fever tag
                countryT.GetComponent<SpriteRenderer>().color = DefaultColorForYellowFever;

            else if (countryT.tag == "Typhus") // Handle Typhus tag
                countryT.GetComponent<SpriteRenderer>().color = DefaultColorForTyphus;

            else if (countryT.tag == "Cholera") // Handle Cholera tag
                countryT.GetComponent<SpriteRenderer>().color = DefaultColorForCholera;

            else if (countryT.tag == "Malaria") // Handle Malaria tag
                countryT.GetComponent<SpriteRenderer>().color = DefaultColorForMalaria;

            else if (countryT.tag == "Measles") // Handle Measles tag
                countryT.GetComponent<SpriteRenderer>().color = DefaultColorForMeasles;

            else if (countryT.tag == "African trypanosomiasis") // Handle African trypanosomiasis tag
                countryT.GetComponent<SpriteRenderer>().color = DefaultColorForAfricanTrypanosomiasis;

            else if (countryT.tag == "Kuru") // Handle Kuru tag
                countryT.GetComponent<SpriteRenderer>().color = DefaultColorForKuru;

            else if (countryT.tag == "Influenza") // Handle Influenza tag
                countryT.GetComponent<SpriteRenderer>().color = DefaultColorForInfluenza;

            else if (countryT.tag == "HIV") // Handle HIV tag
                countryT.GetComponent<SpriteRenderer>().color = DefaultColorForHIV;

            else if (countryT.tag == "SARS") // Handle SARS tag
                countryT.GetComponent<SpriteRenderer>().color = DefaultColorForSARS;

            else if (countryT.tag == "Dengue fever") // Handle Dengue fever tag
                countryT.GetComponent<SpriteRenderer>().color = DefaultColorForDengueFever;

            else if (countryT.tag == "Meningitis") // Handle Meningitis tag
                countryT.GetComponent<SpriteRenderer>().color = DefaultColorForMeningitis;

            else if (countryT.tag == "MERS-CoV") // Handle MERS-CoV tag
                countryT.GetComponent<SpriteRenderer>().color = DefaultColorForMERSCoV;

            else if (countryT.tag == "Ebola") // Handle Ebola tag
                countryT.GetComponent<SpriteRenderer>().color = DefaultColorForEbola;

            else if (countryT.tag == "COVID-19") // Handle COVID-19 tag
                countryT.GetComponent<SpriteRenderer>().color = DefaultColorForCOVID19;

            countryT.transform.GetChild(0).gameObject.SetActive(ShowCountryNames);
            if(ShowCountryNames)
            {
            TextMeshProUGUI text = countryT.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            Material mat = text.fontSharedMaterial;
            mat.shaderKeywords = new string[]{"OUTLINE_ON"};
            text.color = FontColor;
            text.fontStyle = FontStyle;
            text.outlineWidth = StrokeSize;
            text.outlineColor = StrokeColor;
            text.text = countryT.name;
            }
        }
        
}
}
public enum InteractionModes
{
    Multiple,Single,
}
