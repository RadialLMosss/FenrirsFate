using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeButton : MonoBehaviour
{
    public Button buttonActual;
    public Button buttonNext;
    
    public bool isCloseBtn;
    public GameObject skillTreePanel;
    public void GetSkill()
    {
        buttonActual.interactable = false;
        buttonActual.GetComponent<Image>().color = Color.red;
        if(buttonNext != null)
        {
            buttonNext.interactable = true;
            buttonNext.GetComponent<Image>().color = Color.white;
        }
    }

    public void Close()
    {
        skillTreePanel.SetActive(false);
    }
}
