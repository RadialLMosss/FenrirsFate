using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectablePrize : MonoBehaviour
{

    public enum Version { Shop, Chest}
    public Version version;

    public enum Type { LifePotion, FuryPotion, CrystalBag, LifeRune, FuryRune, CourageRune}
    [HideInInspector] public Type type;

    public enum Size { Small, Medium, Big, Max}
    [HideInInspector] public Size size;

    public GameObject[] possibleVisuals;
    public int[] prices;
    [HideInInspector] public int price;
    public GameObject chestModel;
    [HideInInspector] public bool isChestOpened;

    private void Start()
    {
        if(LevelGenerator.levelType == LevelGenerator.LevelType.puzzle && version == Version.Chest)
        {
            chestModel.SetActive(true);
        }
        else if(LevelGenerator.levelType == LevelGenerator.LevelType.shop && version == Version.Shop)
        {
            InitShopVersion();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void OpenChest()
    {
        if(version == Version.Chest)
        {
            //play animation
            InitChestVersion();
            isChestOpened = true;
        }
    }

    void InitShopVersion()
    {
        int r = Random.Range(0, 5);
        switch(r)
        {
            case 0:
                if (Player.lifePoints == Player.totalLifePoints)
                {
                    type = Type.FuryPotion;
                    possibleVisuals[1].SetActive(true);
                    price = prices[1];
                }
                else
                {
                    type = Type.LifePotion;
                    possibleVisuals[0].SetActive(true);
                    price = prices[0];
                }
                break;

            case 1:
                type = Type.FuryPotion;
                possibleVisuals[1].SetActive(true);
                price = prices[1];
                break;

            case 2:
                if(Player.totalLifePoints <= Player.baseLife)
                {
                    type = Type.LifeRune;
                    possibleVisuals[3].SetActive(true);
                    price = prices[3];
                }
                else
                {
                    if (Player.lifePoints == Player.totalLifePoints)
                    {
                        type = Type.FuryPotion;
                        possibleVisuals[1].SetActive(true);
                        price = prices[1];
                    }
                    else
                    {
                        type = Type.LifePotion;
                        possibleVisuals[0].SetActive(true);
                        price = prices[0];
                    }
                }
                break;

            case 3:
                if(Player.normalDamageValue <= Player.baseNormalDamage)
                {
                    type = Type.FuryRune;
                    possibleVisuals[4].SetActive(true);
                    price = prices[4];
                }
                else
                {
                    type = Type.FuryPotion;
                    possibleVisuals[1].SetActive(true);
                    price = prices[1];
                }
                break;

            case 4:
                if(Player.defenseValue <= Player.baseDefense)
                {
                    type = Type.CourageRune;
                    possibleVisuals[5].SetActive(true);
                    price = prices[5];
                }
                else
                {
                    if(Random.Range(0, 2) == 0)
                    {
                        type = Type.LifePotion;
                        possibleVisuals[0].SetActive(true);
                        price = prices[0];
                    }
                    else
                    {
                        type = Type.FuryPotion;
                        possibleVisuals[1].SetActive(true);
                        price = prices[1];
                    }
                }
                break;
        }
        RandomSize();
    }

    void InitChestVersion()
    {
        int r = Random.Range(0, 3);
        switch (r)
        {
            case 0:
                if (Player.lifePoints == Player.totalLifePoints)
                {
                    int r2 = Random.Range(0, 2);
                    switch (r2)
                    {
                        case 0:
                            type = Type.CrystalBag;
                            possibleVisuals[2].SetActive(true);
                            price = prices[2];
                            break;

                        case 1:
                            type = Type.FuryPotion;
                            possibleVisuals[1].SetActive(true);
                            price = prices[1];
                            break;
                    }
                }
                else
                {
                    type = Type.LifePotion;
                    possibleVisuals[0].SetActive(true);
                    price = prices[0];
                }
                break;

            case 1:
                type = Type.FuryPotion;
                possibleVisuals[1].SetActive(true);
                price = prices[1];
                break;

            case 2:
                type = Type.CrystalBag;
                possibleVisuals[2].SetActive(true);
                price = prices[2];
                break;
        }
        if(r != 0 && Player.lifePoints <= Player.totalLifePoints/3)
        {
            type = Type.LifePotion;
            possibleVisuals[0].SetActive(true);
            price = prices[0];
        }
        RandomSize();
    }

    void RandomSize()
    {
        if(type != Type.CourageRune && type != Type.FuryRune && type != Type.LifeRune)
        {
            int r = Random.Range(0, 3);
            if (r == 0)
            {
                size = Size.Small;
            }
            else if (r == 1)
            {
                size = Size.Medium;
                price *= 2;
            }
            else if (r == 2)
            {
                size = Size.Big;
                price *= 3;
            }
        }
        else
        {
            size = Size.Max;
        }
    }

}
