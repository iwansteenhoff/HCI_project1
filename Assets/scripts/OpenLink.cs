using UnityEngine;
using UnityEngine.UI;

public class OpenLink : MonoBehaviour
{
    private string documentSource; // Store the link URL

    public void SetDocumentSource(string source)
    {
        documentSource = source;
    }

    // This function will be called when the hyperlink button is clicked
    public void OpenUrl()
    {
        if (!string.IsNullOrEmpty(documentSource))
        {
            Application.OpenURL(documentSource);
        }
        else
        {
            Debug.LogError("Document source is empty or null.");
        }
    }
}
