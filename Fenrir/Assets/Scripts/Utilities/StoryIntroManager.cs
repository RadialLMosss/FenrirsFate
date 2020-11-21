using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryIntroManager : MonoBehaviour
{
    int textCount = -1;
    public Image storyTextPanel;
    public Sprite[] storyPanels;
    string nextSceneName;
    public GameObject storyPanel;
    public void StartStoryIntro(string nextScene)
    {
        storyPanel.SetActive(true);
        nextSceneName = nextScene;
        StartCoroutine(ShowStory());
    }

    IEnumerator ShowStory()
    {
        textCount++;
        if(textCount < storyPanels.Length)
        {
            storyTextPanel.sprite = storyPanels[textCount];
            yield return new WaitForSeconds(7);
            StartCoroutine(ShowStory());
        }
        else
        {
            LoadingSceneManager.LoadScene(nextSceneName);
        }
    }
}
