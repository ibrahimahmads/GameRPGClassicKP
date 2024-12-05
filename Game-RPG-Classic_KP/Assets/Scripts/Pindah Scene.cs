using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PindahScene : MonoBehaviour
{
    public string namaScene;
    public void pindahkeScene(string namaScene){
        SceneManager.LoadScene(namaScene);
    }
}
