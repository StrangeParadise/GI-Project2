using UnityEngine;
using System.Collections;

public class Exit : MonoBehaviour
{
    public void Quit()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }
}