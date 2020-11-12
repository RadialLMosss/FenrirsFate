using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : MonoBehaviour
{
    public Text percentText;
    public Image foregroundImage;


    //=============================================================================================
    private void Start()
    {
        string sceneName = PlayerPrefs.GetString("SCENE_TO_LOAD", "StartMenuScene");
        PlayerPrefs.SetString("SCENE_TO_LOAD", "StartMenuScene");
        PlayerPrefs.Save();

        percentText.text = "0%";
        foregroundImage.fillAmount = 0;
        StartCoroutine(LoadSceneAsync(sceneName));
    }


    //=============================================================================================
    public static void LoadScene(string sceneName)
    {
        PlayerPrefs.SetString("SCENE_TO_LOAD", sceneName);
        PlayerPrefs.Save();
        SceneManager.LoadScene("LoadingScene");
    }

    //=============================================================================================
    IEnumerator LoadSceneAsync(string sceneName)
    {
        yield return new WaitForSeconds(1);
        System.GC.Collect();
        
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while(!asyncLoad.isDone)
        {
            percentText.text = (asyncLoad.progress * 100).ToString("N0") + "%";
            foregroundImage.fillAmount = asyncLoad.progress;
            yield return new WaitForEndOfFrame();
        }
    }
}
