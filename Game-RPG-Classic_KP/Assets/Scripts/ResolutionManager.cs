using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionManager : MonoBehaviour
{
    // Fungsi untuk mengatur resolusi ke 1920x1080 (windowed)
    public void SetResolution1920x1080()
    {
        Screen.SetResolution(1920, 1080, false);
    }

    // Fungsi untuk mengaktifkan atau menonaktifkan fullscreen
    public void ToggleFullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
}
