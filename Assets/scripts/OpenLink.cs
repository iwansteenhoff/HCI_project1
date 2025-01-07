using UnityEngine;
using UnityEngine.UI;

public class OpenLink : MonoBehaviour
{
    private string documentSource; // Store the link URL

    public void SetDocumentSource(string source)
    {
        documentSource = source;
        Debug.Log($"SetDocumentSource called on instance: {this.GetInstanceID()} with source: {source}");
    }

    public void OpenUrl()
    {
        Debug.Log($"OpenUrl called on instance: {this.GetInstanceID()} with documentSource: {documentSource}");
        if (!string.IsNullOrEmpty(documentSource))
        {
            Application.OpenURL(documentSource);
            Debug.Log("we entered");
        }
        else
        {
            Debug.LogError("Document source is empty or null.");
        }
    }

}
