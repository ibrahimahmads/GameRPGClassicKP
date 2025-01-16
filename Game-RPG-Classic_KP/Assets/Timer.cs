using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText; // Referensi ke Text di Canvas
    private float elapsedTime = 0f;   // Waktu yang berjalan
    private bool isTimerRunning = false;

    void Start()
    {
        StartTimer(); // Mulai timer saat game dimulai
    }

    void Update()
    {
        if (isTimerRunning)
        {
            elapsedTime += Time.deltaTime; // Tambahkan waktu setiap frame
            UpdateTimerDisplay();
        }
    }

    // Mulai timer
    public void StartTimer()
    {
        elapsedTime = 0f;
        isTimerRunning = true;
    }

    // Hentikan timer
    public void StopTimer()
    {
        isTimerRunning = false;
    }

    // Ambil waktu yang berjalan
    public float GetElapsedTime()
    {
        return elapsedTime;
    }

    // Update tampilan timer
    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60f); // Hitung menit
        int seconds = Mathf.FloorToInt(elapsedTime % 60f); // Hitung detik

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds); // Format MM:SS
    }
}
