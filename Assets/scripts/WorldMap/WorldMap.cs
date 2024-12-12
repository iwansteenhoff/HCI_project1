
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;
public class WorldMap : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerDownHandler,IPointerUpHandler
{
    ///<summary>
    ///selected country of value type <Country>
    ///</summary>
    [HideInInspector]public Country selectedCountry;

    ///<summary>
    ///current highlighted country of value type <Country>
    ///</summary>
    [HideInInspector]public Country highlightedCountry;


    ///<summary>
    ///current selected countries list of value type List<Countries>
    ///</summary>
    [HideInInspector]public List<Country> selectedCountries = new List<Country>();

    
    ///<summary>
    /// Fire an event once the user select a country
    ///</summary>
    public event EventHandler onSelectCountry;

    ///<summary>
    /// Fire an event once the user highlight a country
    ///</summary>
    public event EventHandler onHighlightCountry;

    ///<summary>
    /// Fire an event once the user unselect a country
    ///</summary>
    public event EventHandler onUnselectCountry;


    ///<summary>
    /// When set to "true" the user can interact with the map
    ///</summary>
    public bool canInteracte = true;



    RectTransform rectTransform;
    int width,height,textureWidth,textureHeigth;
    [SerializeField]Texture2D GSmap;
    float pickedColorGS,lastColorGS;
    static Color dColor;
    MapStyleController mapStyleController;
    bool mouseOnRange = false;
    Vector2 firstTouch;
    ///<summary>
    /// Clear all selected countries
    ///</summary>
    public void Clear()
    {
        selectedCountries.Clear();
        for(int x=0;x<transform.childCount;x++)
        {
            if(transform.GetChild(x).tag == "Selected")
            {
                transform.GetChild(x).GetComponent<SpriteRenderer>().color = mapStyleController.DefaultColorForCountries;
                transform.GetChild(x).tag = "Not Selected";
            }
        }
        highlightedCountry = Country.Empty;
        selectedCountry = Country.Empty;
    }


    ///<summary>
    /// Get the name of a country
    ///</summary>
    public string GetCountryName(Country country)
    {
        string name = country.ToString();
        return name.Replace("_"," ");
    }

    ///<summary>
    /// Make a country non interactable
    ///</summary>
    public void DisableCountry(Country country)
    {
        Transform countryT = transform.GetChild((int)country);
        countryT.GetComponent<SpriteRenderer>().color = mapStyleController.DefaultColorForNonIntractableCountries;
        countryT.tag = "Disabled";
    }

    ///<summary>
    /// Make a country Interactable
    ///</summary>
    public void EnableCountry(Country country)
    {
        Transform countryT = transform.GetChild((int)country);
        countryT.GetComponent<SpriteRenderer>().color = mapStyleController.DefaultColorForCountries;
        countryT.tag = "Not Selected";
    }


    ///<summary>
    /// Make All countries Interactable
    ///</summary>
    public void EnableAllCountries()
    {
        for(int x=0;x<transform.childCount;x++)
        {
            Transform countryT = transform.GetChild(x);
            if(countryT.tag == "Disabled")
            {
                countryT.GetComponent<SpriteRenderer>().color = mapStyleController.DefaultColorForCountries;
                countryT.tag = "Not Selected";
            }
        }
        
    }

    ///<summary>
    /// Select Country
    ///</summary>
    public void SelectCountry(Country country)
    {
        if(mapStyleController.interactionMode == InteractionModes.Single)
        {
            foreach (Country c in selectedCountries)
            {
                Transform _countryT = transform.GetChild((int)c);
            _countryT.gameObject.tag = "Not Selected";
            _countryT.GetComponent<SpriteRenderer>().color = mapStyleController.DefaultColorForCountries;
            }
            selectedCountries.Clear();
        }
        selectedCountry = country;
        selectedCountries.Add(country);
        Transform countryT = transform.GetChild((int)country);
        countryT.gameObject.tag = "Selected";
        countryT.GetComponent<SpriteRenderer>().color = mapStyleController.DefaultColorForSelectedCountries;
    }
    ///<summary>
    /// Unselect Country
    ///</summary>
    public void UnselectCountry(Country country)
    {
        selectedCountries.Remove(country);
        Transform countryT = transform.GetChild((int)country);
        countryT.gameObject.tag = "Not Selected";
        countryT.GetComponent<SpriteRenderer>().color = mapStyleController.DefaultColorForCountries;
        selectedCountry = Country.Empty;
        
    }
    //-----------------------------
    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOnRange = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOnRange = false;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        float magnitude = (firstTouch - new Vector2(Input.mousePosition.x,Input.mousePosition.y)).magnitude;
        if(canInteracte&&magnitude ==0)
        SelectCountry();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        firstTouch = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
    }
    void Start()
    {
        highlightedCountry = Country.Empty;
        selectedCountry = Country.Empty;
        mapStyleController = transform.GetComponent<MapStyleController>();
        rectTransform = transform.GetComponent<RectTransform>();
        width = (int)rectTransform.rect.width;
        height = (int)rectTransform.rect.height;
        textureWidth = GSmap.width;
        textureHeigth = GSmap.height;
    }
    void Update()
    {
        if(mouseOnRange && canInteracte)
        {
            HighlightCountry();
        }
    }
    ColorsGS GetGSid()
    {
        Vector2 mousePos = new Vector2();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform,Input.mousePosition,Camera.main,out mousePos);
                float gsValue = GSmap.GetPixel((int)mousePos.x,(int)mousePos.y).grayscale;
                string id = gsValue.ToString();
                if(gsValue < 1f)
                {
                    id = id.Remove(0,2);
                }
                string gsId = "GS"+id;
                ColorsGS colorGS = new ColorsGS();
              Enum.TryParse(gsId,out colorGS);
              return colorGS;
    }
    void SelectCountry()
    {
        ColorsGS grayScale = GetGSid();
        if(grayScale != ColorsGS.GS1)
        {
            int index =((int)grayScale);
            if(transform.GetChild(index).tag == "Not Selected")
            {
                SelectCountry((Country)index);
                if(onSelectCountry != null)
                onSelectCountry(this,EventArgs.Empty);
            }
            else if(transform.GetChild(index).tag == "Selected")
            {
                UnselectCountry((Country)index);
                if(onUnselectCountry != null)
                onUnselectCountry(this,EventArgs.Empty);
            }
        }
    }
    
    void HighlightCountry()
    {
        ColorsGS grayScale = GetGSid();
            int index =((int)grayScale);
            if(transform.GetChild(index).tag == "Not Selected")
            {
                if(transform.GetChild((int)highlightedCountry).tag != "Selected")
                transform.GetChild((int)highlightedCountry).GetComponent<SpriteRenderer>().color=mapStyleController.DefaultColorForCountries;


                highlightedCountry = (Country)index;
                transform.GetChild(index).GetComponent<SpriteRenderer>().color=mapStyleController.DefaultColorForHighlightedCountries;

                if(onHighlightCountry != null)
                onHighlightCountry(this,EventArgs.Empty);
            }
    }
    
}
public enum Country
{
     Empty=0, United_Arab_Emirates=1, Afghanistan=2, Albania=3, Armenia=4, Angola=5, Argentina=6, Austria=7, Australia=8, Azerbaijan=9, Bosnia_and_Herzegovina=10, Bangladesh=11, Belgium=12, Burkina_Faso=13, Bulgaria=14, Burundi=15, Benin=16, Brunei_Darussalam=17, Bolivia=18, Brazil=19, Bahamas=20, Bhutan=21, Botswana=22, Belarus=23, Belize=24, Canada=25, Democratic_Republic_of_the_Congo=26, Central_African_Republic=27, Congo=28, Switzerland=29, Côte_dIvoire=30, Chile=31, Cameroon=32, China=33, Colombia=34, Costa_Rica=35, Cuba=36, Cape_Verde=37, Cyprus=38, Czech_Republic=39, Germany=40, Djibouti=41, Denmark=42, Dominica=43, Dominican_Republic=44, Algeria=45, Ecuador=46, Estonia=47, Egypt=48, Eritrea=49, Spain=50, Western_Sahara=51, Ethiopia=52, Finland=53, Falkland_Islands_اMalvinasا=54, France=55, Gabon=56, United_Kingdom=57, Georgia=58, Ghana=59, Greenland=60, Gambia=61, Guinea=62, Equatorial_Guinea=63, Greece=64, Guatemala=65, Guinea_Bissau=66, Guyana=67, Honduras=68, Croatia=69, Haiti=70, Hungary=71, Indonesia=72, Ireland=73, India=74, Iraq=75, Iran=76, Iceland=77, Italy=78, Jamaica=79, Jordan=80, Japan=81, Kenya=82, Kyrgyzstan=83, Cambodia=84, Comoros=85, North_Korea=86, South_Korea=87, Kuwait=88, Kazakhstan=89, Lao_Peoples_Democratic_Republic=90, Saint_Lucia=91, Sri_Lanka=92, Liberia=93, Lesotho=94, Lithuania=95, Luxembourg=96, Latvia=97, Libya=98, Morocco=99, Moldova=100, Montenegro=101, Madagascar=102, Macedonia=103, Mali=104, Myanmar=105, Mongolia=106, Mauritania=107, Malta=108, Mauritius=109, Maldives=110, Malawi=111, Mexico=112, Malaysia=113, Mozambique=114, Namibia=115, New_Caledonia=116, Niger=117, Nigeria=118, Nicaragua=119, Netherlands=120, Norway=121, Nepal=122, New_Zealand=123, Oman=124, Panama=125, Peru=126, Papua_New_Guinea=127, Philippines=128, Pakistan=129, Poland=130, Puerto_Rico=131, Palestine=132, Portugal=133, Paraguay=134, Qatar=135, Romania=136, Serbia=137, Russia=138, Rwanda=139, Saudi_Arabia=140, Solomon_Islands=141, Seychelles=142, Sudan=143, Sweden=144, Singapore=145, Slovenia=146, Slovakia=147, Sierra_Leone=148, Senegal=149, Somalia=150, Suriname=151, South_Sudan=152, Sao_Tome_and_Principe=153, El_Salvador=154, Syrian_Arab_Republic=155, Swaziland=156, Chad=157, Togo=158, Thailand=159, Tajikistan=160, Turkmenistan=161, Tunisia=162, Turkey=163, Trinidad_and_Tobago=164, Taiwan=165, Tanzania=166, Ukraine=167, Uganda=168, United_States=169, Uruguay=170, Uzbekistan=171, Saint_Vincent_and_the_Grenadines=172, Venezuela=173, Viet_Nam=174, Vanuatu=175, Yemen=176, South_Africa=177, Zambia=178, Zimbabwe=179, Andorra=180, Antigua_and_Barbuda=181, Bahrain=182, Faroe_Islands=183, Guadeloupe=184, South_Georgia_and_the_South_Sandwich_Islands=185, Liechtenstein=186, Monaco=187, Réunion=188, French_Guiana=189, French_Southern_Territories=190

}
enum ColorsGS
{
    GS1 = Country.Empty, GS8041177= Country.United_Arab_Emirates, GS3179098= Country.Afghanistan, GS1787686= Country.Albania, GS7004196= Country.Armenia, GS6317569= Country.Algeria, GS6642471= Country.Western_Sahara, GS5325686= Country.Canada, GS3948353= Country.United_States, GS3573216= Country.Mexico, GS04088628= Country.Guatemala, GS1047765= Country.Belize, GS3163686= Country.El_Salvador, GS3308549= Country.Honduras, GS4422902= Country.Nicaragua, GS5424549= Country.Costa_Rica, GS326353= Country.Panama, GS4024= Country.Cuba, GS6474824= Country.Bahamas, GS9088824= Country.Jamaica, GS481898= Country.Haiti, GS9654039= Country.Dominican_Republic, GS6860039= Country.Puerto_Rico, GS09538431= Country.France, GS319149= Country.Dominica, GS6663334= Country.Saint_Lucia, GS4529922= Country.Saint_Vincent_and_the_Grenadines, GS5894746= Country.Trinidad_and_Tobago, GS3162275= Country.Venezuela, GS6998824= Country.Colombia, GS477949= Country.Ecuador, GS4353373= Country.Guyana, GS7509569= Country.Suriname, GS4908471= Country.Brazil, GS2005647= Country.Peru, GS3002824= Country.Bolivia, GS3974824= Country.Paraguay, GS3873451= Country.Uruguay, GS7615961= Country.Argentina, GS8108785= Country.Chile, GS3287804= Country.Falkland_Islands_اMalvinasا, GS229698= Country.Portugal, GS6212589= Country.Spain, GS2663804= Country.Morocco, GS6868275= Country.Tunisia, GS5033686= Country.Cape_Verde, GS6560157= Country.Libya, GS507102= Country.Mauritania, GS2727294= Country.Mali, GS3055961= Country.Niger, GS2186706= Country.Egypt, GS7116314= Country.Sudan, GS7797216= Country.Chad, GS302251= Country.Eritrea, GS6572706= Country.Djibouti, GS6743255= Country.Somalia, GS2622549= Country.Senegal, GS4933255= Country.Gambia, GS6253647= Country.Guinea_Bissau, GS3001804= Country.Guinea, GS8418863= Country.Sierra_Leone, GS2051569= Country.Liberia, GS3693177= Country.Côte_dIvoire, GS4748314= Country.Burkina_Faso, GS6382942= Country.Ghana, GS6933961= Country.Togo, GS5007961= Country.Benin, GS2755921= Country.Nigeria, GS8454627= Country.Cameroon, GS9255686= Country.Central_African_Republic, GS9016275= Country.South_Sudan, GS4328275= Country.Ethiopia, GS6009922= Country.Kenya, GS3573451= Country.Tanzania, GS3886549= Country.Uganda, GS7093059= Country.Democratic_Republic_of_the_Congo, GS6563451= Country.Gabon, GS5931961= Country.Equatorial_Guinea, GS7345294= Country.Sao_Tome_and_Principe, GS4054353= Country.Congo, GS2450196= Country.Angola, GS5888196= Country.Zambia, GS5262275= Country.Rwanda, GS6173294= Country.Burundi, GS691302= Country.Namibia, GS2466471= Country.Botswana, GS6276471= Country.Zimbabwe, GS4854353= Country.Mozambique, GS4372706= Country.Malawi, GS462102= Country.South_Africa, GS1541686= Country.Lesotho, GS7063137= Country.Swaziland, GS4881177= Country.Madagascar, GS4249569= Country.Comoros, GS5572863= Country.Mauritius, GS4605373= Country.Seychelles, GS6864471= Country.Ireland, GS3424079= Country.United_Kingdom, GS6776157= Country.Belgium, GS819153= Country.Switzerland, GS2242431= Country.Italy, GS5962039= Country.Luxembourg, GS5415177= Country.Germany, GS6067725= Country.Poland, GS7165373= Country.Czech_Republic, GS1509059= Country.Austria, GS5561961= Country.Slovakia, GS8724392= Country.Hungary, GS7245412= Country.Slovenia, GS5356157= Country.Croatia, GS6677138= Country.Bosnia_and_Herzegovina, GS6670235= Country.Norway, GS6959804= Country.Sweden, GS2067333= Country.Denmark, GS5687647= Country.Malta, GS3890118= Country.Montenegro, GS1954549= Country.Serbia, GS6527529= Country.Romania, GS5996079= Country.Bulgaria, GS7014431= Country.Macedonia, GS5405804= Country.Greece, GS8115177= Country.Turkey, GS5524118= Country.Finland, GS5292667= Country.Russia, GS2437373= Country.Estonia, GS5284902= Country.Latvia, GS5876275= Country.Lithuania, GS6069647= Country.Belarus, GS583698= Country.Ukraine, GS3123255= Country.Moldova, GS5481176= Country.Australia, GS3020941= Country.New_Zealand, GS5905843= Country.New_Caledonia, GS3398274= Country.Vanuatu, GS4141647= Country.Solomon_Islands, GS08274118= Country.Papua_New_Guinea, GS3123373= Country.Indonesia, GS4834745= Country.Malaysia, GS3405686= Country.Brunei_Darussalam, GS6569765= Country.Philippines, GS6407765= Country.Taiwan, GS4500392= Country.China, GS3092157= Country.Japan, GS6583334= Country.North_Korea, GS6645647= Country.South_Korea, GS4322353= Country.Mongolia, GS6694314= Country.Kazakhstan, GS8855686= Country.Kyrgyzstan, GS1631176= Country.Uzbekistan, GS6342589= Country.India, GS3679608= Country.Sri_Lanka, GS421549= Country.Myanmar, GS6663216= Country.Thailand, GS1121255= Country.Cambodia, GS8560432= Country.Viet_Nam, GS8006353= Country.Lao_Peoples_Democratic_Republic, GS3385647= Country.Maldives, GS2979177= Country.Bangladesh, GS5092353= Country.Nepal, GS4323334= Country.Bhutan, GS5641098= Country.Saudi_Arabia, GS2742824= Country.Yemen, GS5323138= Country.Oman, GS3567726= Country.Pakistan, GS625953= Country.Iran, GS7702745= Country.Qatar, GS6538667= Country.Kuwait, GS4976628= Country.Syrian_Arab_Republic, GS2390628= Country.Iraq, GS4165686= Country.Jordan, GS5798941= Country.Palestine, GS6310589= Country.Cyprus, GS2078039= Country.Turkmenistan, GS3068= Country.Tajikistan, GS3323843= Country.Azerbaijan, GS2926431= Country.Georgia, GS6159726= Country.Iceland, GS6262902= Country.Netherlands, GS2548628= Country.French_Southern_Territories, GS4002392= Country.South_Georgia_and_the_South_Sandwich_Islands, GS4655882= Country.Réunion, GS5538= Country.Bahrain, GS3064863= Country.Guadeloupe, GS5775373= Country.Greenland, GS4682941= Country.Singapore, GS2772902= Country.French_Guiana, GS6175843= Country.Monaco, GS335051= Country.Andorra, GS2834432= Country.Liechtenstein, GS8153961= Country.Faroe_Islands, GS2569843= Country.Antigua_and_Barbuda
}