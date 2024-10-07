using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;  // Drag the pause menu UI panel here in the inspector
    private bool isPaused = false;
    private Animator animator;

    void Awake()
    {
        pauseMenuUI.SetActive(true);
        animator = pauseMenuUI.GetComponent<Animator>();
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
                Pause();
            }
        }
    }

    public void Resume()
    {
        isPaused = false;
        pauseMenuUI.SetActive(isPaused);
        Time.timeScale = 1f;  // Resume the game time
        
    }

    void Pause()
    { 
        isPaused = true;
        pauseMenuUI.SetActive(isPaused);

        animator.SetTrigger("Pause");
        Time.timeScale = 0f;  // Pause the game time
        
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
