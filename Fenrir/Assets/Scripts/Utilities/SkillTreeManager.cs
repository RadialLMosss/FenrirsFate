using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeManager : MonoBehaviour
{

    public GameObject notEnough_Message;
    public Skill[] skills;
    public Player player;
    public GameObject skillTreePanel;

    [System.Serializable]
    public class Skill
    {
        //public string skillName;
        public int skillCost;
        public Button buttonCurrent;
        public Button[] buttonNext;
    }

    public void GetSkill(int skillIndex)
    {
        Skill skill = skills[skillIndex];

        if (Player.fury > skill.skillCost)
        {
            player.UpdateFuryCurrency(-skill.skillCost);

            skill.buttonCurrent.interactable = false;
            skill.buttonCurrent.GetComponent<Image>().color = Color.red;

            if (skill.buttonNext != null && skill.buttonNext.Length > 0)
            {
                for (int i = 0; i < skill.buttonNext.Length; i++)
                {
                    skill.buttonNext[i].interactable = true;
                    skill.buttonNext[i].GetComponent<Image>().color = Color.white;
                }
            }
            Player.EnableSkill(skillIndex);
        }
        else
        {
            //show error saying that the player doesn't have enough fury currency
            StartCoroutine(ShowErrorMessage());
        }
    }
    
    IEnumerator ShowErrorMessage()
    {
        notEnough_Message.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        notEnough_Message.SetActive(false);
    }

    private void OnEnable()
    {
        if (notEnough_Message.activeSelf)
        {
            notEnough_Message.SetActive(false);
        }
    }

    public void Close()
    {
        skillTreePanel.SetActive(false);
    }
}
