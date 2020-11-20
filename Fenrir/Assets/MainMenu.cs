using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject shopPanel;
    public GameObject creditsPanel;
    public void ShopButton()
    {
        shopPanel.SetActive(!shopPanel.activeSelf);
    }
    public void CreditsButton()
    {
        creditsPanel.SetActive(!creditsPanel.activeSelf);
    }
}
