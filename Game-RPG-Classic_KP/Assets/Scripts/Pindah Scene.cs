using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PindahScene : MonoBehaviour
{
    public string namaScene;
    public void pindahkeScene(string namaScene){
        SaveManager saveManager = FindObjectOfType<SaveManager>();
        if (saveManager != null)
        {
            saveManager.DeletePlayerData();
            //saveManager.InitializeNewGame();
        }
        SceneManager.LoadScene(namaScene);
    }
}
