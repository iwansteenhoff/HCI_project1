using UnityEngine;

public class QuitGameButton : MonoBehaviour
{
    public void QuitGame()
    {
        // Logs to the console when running in the editor (optional).
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Quits the application when built.
        Application.Quit();
#endif
    }
}