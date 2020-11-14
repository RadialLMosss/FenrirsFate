﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectablePrize : MonoBehaviour
{

    public enum Version { Shop, Chest}
    public Version version;

    public enum Type { LifePotion, FuryPotion, CrystalBag, LifeRune, FuryRune, CourageRune}
    [HideInInspector] public Type type;

    public enum Size { Small, Medium, Big}
    [HideInInspector] public Size size;

    public GameObject[] possibleVisuals;
    public int[] prices;
    [HideInInspector] public int price;

    private void Start()
    {
        if(LevelGenerator.levelType == LevelGenerator.LevelType.puzzle && version == Version.Chest)
        {
            InitChestVersion();
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

    void InitShopVersion()
    {
        int r = Random.Range(0, 5);
        switch(r)
        {
            case 0:
                type = Type.LifePotion;
                possibleVisuals[0].SetActive(true);
                price = prices[0];
                break;

            case 1:
                type = Type.FuryPotion;
                possibleVisuals[1].SetActive(true);
                price = prices[1];
                break;

            case 2:
                type = Type.LifeRune;
                possibleVisuals[3].SetActive(true);
                price = prices[3];
                break;

            case 3:
                type = Type.FuryRune;
                possibleVisuals[4].SetActive(true);
                price = prices[4];
                break;

            case 4:
                type = Type.CourageRune;
                possibleVisuals[5].SetActive(true);
                price = prices[5];
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
                type = Type.LifePotion;
                possibleVisuals[0].SetActive(true);
                price = prices[0];
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
        RandomSize();
    }

    void RandomSize()
    {
        int r = Random.Range(0, 3);
        if(r == 0)
        {
            size = Size.Small;
        }
        else if(r == 1)
        {
            size = Size.Medium;
            price *= 2;
        }
        else if(r == 2)
        {
            size = Size.Big;
            price *= 3;
        }
    }
}
