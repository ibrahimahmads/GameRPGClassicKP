using UnityEngine;
using System.Collections; 
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;  // Drag the pause menu UI panel here in the inspector
    private bool isPaused = false;
    public Animator animator;

    void Awake()
    {
        pauseMenuUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                StartCoroutine(PauseCoroutine());
            }
        }
    }

    public void Resume()
    {
        animator.Play("pause hide");
        Invoke("DeactivatePauseMenu", .4f);
        Time.timeScale = 1f;  // Resume the game time
        isPaused = false;
    }

    IEnumerator PauseCoroutine()
    { 
        pauseMenuUI.SetActive(true);  // Aktifkan panel sebelum animasi berjalan
        animator.Play("pause_show");

        yield return new WaitForSecondsRealtime(1.08f);  // Menunggu animasi selesai di real-time (tidak terpengaruh Time.timeScale)
        
        Time.timeScale = 0f;  // Pause game setelah animasi selesai
        isPaused = true;
    }

    void DeactivatePauseMenu()
    {
        pauseMenuUI.SetActive(false);  // Nonaktifkan panel setelah animasi selesai
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }

    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
