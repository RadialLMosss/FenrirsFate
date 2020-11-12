using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSceneButton : MonoBehaviour
{
    public StoryIntroManager storyIntroManager;

    public void ButtonLoadOtherScene(string sceneName)
    {
        if(sceneName == "GameScene")
        {
            if (PlayerPrefs.GetInt("IS_FIRST_TIME") == 0)
            {
                gameObject.SetActive(false);
                PlayerPrefs.SetInt("IS_FIRST_TIME", 1);
                storyIntroManager.StartStoryIntro(sceneName);
            }
            else
            {
                LoadingSceneManager.LoadScene(sceneName);
            }
        }

    }
}
