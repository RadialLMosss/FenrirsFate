using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public Image lifeBar;
    public float baseLifeValue;
    float lifeValue;

    void Start()
    {
        lifeValue = baseLifeValue;
    }

    void Update()
    {
        
    }

    void TakeDamage(float damage)
    {
        lifeValue -= damage;
        lifeBar.fillAmount = lifeValue / baseLifeValue;
    }

    void BossDeath()
    {
        //Cutscene Final
        //Tela de créditos
        //menu inicial
    }
}
