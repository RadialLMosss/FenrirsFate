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
