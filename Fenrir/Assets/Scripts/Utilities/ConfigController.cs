using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class ConfigController : MonoBehaviour
{
    public AudioMixer audioMixer;
    public GameObject optionsPanel;
    public Slider sliderBGM;
    public Slider sliderSFX;
    private void OnEnable()
    {
        float bgm;
        audioMixer.GetFloat("volBGM", out bgm);
        sliderBGM.value = Mathf.Pow(10, Mathf.Log10(bgm) * 20);

        float sfx;
        audioMixer.GetFloat("volSFX", out sfx);
        sliderBGM.value = Mathf.Pow(10, Mathf.Log10(sfx) * 20);
    }
    public void SetBGMVolume(float volume)
    {
        audioMixer.SetFloat("volBGM", Mathf.Log10(volume) * 20);
    }
    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("volSFX", Mathf.Log10(volume) * 20);
    }

    public void OptionsButton()
    {
        optionsPanel.SetActive(!optionsPanel.activeSelf);
    }
}
