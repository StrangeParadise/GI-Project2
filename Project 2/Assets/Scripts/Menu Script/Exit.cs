﻿using UnityEngine;
using System.Collections;

public class Exit : MonoBehaviour
{
    public void quit()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }
}