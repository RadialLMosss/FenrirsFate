using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryIntroManager : MonoBehaviour
{
    int textCount = -1;
    public Text storyText;
    public string[] storyStrings;
    string nextSceneName;

    public void StartStoryIntro(string nextScene)
    {
        nextSceneName = nextScene;
        StartCoroutine(ShowStory());
    }

    IEnumerator ShowStory()
    {
        textCount++;
        if(textCount < storyStrings.Length)
        {
            storyText.text = storyStrings[textCount];
            yield return new WaitForSeconds(3);
            StartCoroutine(ShowStory());
        }
        else
        {
            LoadingSceneManager.LoadScene(nextSceneName);
        }
    }
}
