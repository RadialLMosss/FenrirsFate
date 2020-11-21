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
            storyIntroManager.StartStoryIntro(sceneName);
        }

    }
}
