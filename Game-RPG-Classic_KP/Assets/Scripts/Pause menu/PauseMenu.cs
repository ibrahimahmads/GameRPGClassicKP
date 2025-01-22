using UnityEngine;
using System.Collections; 
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;  // Drag the pause menu UI panel here in the inspector
    private bool isPaused = false;
    public Animator animator;
    public Slider expSlider;
    public TextMeshProUGUI textExp;
    public TextMeshProUGUI textLevel;
    public TextMeshProUGUI textNama;
    private float waitToLoad = 1f;

    void Awake()
    {
        if(pauseMenuUI==null)
        {
            return;
        }
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
        PlayerController.Instance.SetPauseState(false);
    }

    IEnumerator PauseCoroutine()
    {
        GameManager.Instance.posisi = PlayerStat.Instance.transform.position;
        SaveManager.Instance.SavePlayerData();

        int maxExp = PlayerStat.Instance.maxExp;
        int curExp = PlayerStat.Instance.curExp;
        int lvl = PlayerStat.Instance.level;
        expSlider.maxValue = maxExp;
        expSlider.value = curExp;
        textExp.text = "EXP : "+curExp+" / "+maxExp;
        textLevel.text = "Level :  " + lvl;
        pauseMenuUI.SetActive(true);  // Aktifkan panel sebelum animasi berjalan
        animator.Play("pause_show");

        yield return new WaitForSecondsRealtime(1.08f);  // Menunggu animasi selesai di real-time (tidak terpengaruh Time.timeScale)
        
        Time.timeScale = 0f;  // Pause game setelah animasi selesai
        isPaused = true;
        PlayerController.Instance.SetPauseState(true);
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
        Time.timeScale = 1f;
        GameManager.Instance.currentScene = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString("LastScene", GameManager.Instance.currentScene);
        PlayerPrefs.Save();
        SceneManager.LoadScene(sceneName);
    }

    public void NewGame(string sceneName)
    {
        SaveManager.Instance.DeletePlayerData();
        SaveManager.Instance.InitializeNewGame();
        Time.timeScale = 1f;
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    private IEnumerator LoadSceneRoutine(string targetScene)
    {
        while(waitToLoad>=0)
        {
            waitToLoad -= Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadScene(targetScene);
    }

    public void MemuatGame()
    {
        // Pastikan data pemain dimuat terlebih dahulu
        SaveManager.Instance.LoadGame();

        // Dapatkan nama scene terakhir dari PlayerPrefs
        string lastScene =  PlayerPrefs.GetString("LastScene", "Lorong");
        FadeTransition.Instance.FadeToBlack();
        StartCoroutine(LoadSceneRoutineAnim(lastScene));
    }

    private IEnumerator LoadSceneRoutineAnim(string targetScene)
    {
        GameManager.Instance.currentScene = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString("LastScene", GameManager.Instance.currentScene);
        PlayerPrefs.Save();
        while(waitToLoad>=0)
        {
            waitToLoad -= Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadScene(targetScene);
    }   
}
