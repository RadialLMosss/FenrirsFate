using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectablePrize : MonoBehaviour
{

    public enum Version { Shop, Chest, LootBox}
    public Version version;

    public enum Type { Vida, Fúria, Minério, _Vida, Runa_Fúria, Runa_Coragem}
    [HideInInspector] public Type type;

    public enum Size { P, M, G, x}
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
        else if(LevelGenerator.levelType == LevelGenerator.LevelType.shop && version == Version.LootBox)
        {
            chestModel.SetActive(true);
            InitShopVersion();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public Animator anim;
    public void OpenChest()
    {
        if(version == Version.Chest && !isChestOpened)
        {
            anim.Play("OpenChest");
            InitChestVersion();
            StartCoroutine(SetVisualChestItem());
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
                    type = Type.Fúria;
                    possibleVisuals[1].SetActive(true);
                    price = prices[1];
                }
                else
                {
                    type = Type.Vida;
                    possibleVisuals[0].SetActive(true);
                    price = prices[0];
                }
                break;

            case 1:
                type = Type.Fúria;
                possibleVisuals[1].SetActive(true);
                price = prices[1];
                break;

            case 2:
                if(Player.totalLifePoints <= Player.baseLife)
                {
                    type = Type._Vida;
                    possibleVisuals[3].SetActive(true);
                    price = prices[3];
                }
                else
                {
                    if (Player.lifePoints == Player.totalLifePoints)
                    {
                        type = Type.Fúria;
                        possibleVisuals[1].SetActive(true);
                        price = prices[1];
                    }
                    else
                    {
                        type = Type.Vida;
                        possibleVisuals[0].SetActive(true);
                        price = prices[0];
                    }
                }
                break;

            case 3:
                if(Player.normalDamageValue <= Player.baseNormalDamage)
                {
                    type = Type.Runa_Fúria;
                    possibleVisuals[4].SetActive(true);
                    price = prices[4];
                }
                else
                {
                    type = Type.Fúria;
                    possibleVisuals[1].SetActive(true);
                    price = prices[1];
                }
                break;

            case 4:
                if(Player.defenseValue <= Player.baseDefense)
                {
                    type = Type.Runa_Coragem;
                    possibleVisuals[5].SetActive(true);
                    price = prices[5];
                }
                else
                {
                    if(Random.Range(0, 2) == 0)
                    {
                        type = Type.Vida;
                        possibleVisuals[0].SetActive(true);
                        price = prices[0];
                    }
                    else
                    {
                        type = Type.Fúria;
                        possibleVisuals[1].SetActive(true);
                        price = prices[1];
                    }
                }
                break;
        }
        RandomSize();
    }

    int chestItemIndex;
    void InitChestVersion()
    {
        int r = Random.Range(0, 3);
        chestItemIndex = r;
        switch (r)
        {
            case 0:
                if (Player.lifePoints == Player.totalLifePoints)
                {
                    int r2 = Random.Range(0, 2);
                    switch (r2)
                    {
                        case 0:
                            type = Type.Minério;
                            price = prices[2];
                            break;

                        case 1:
                            type = Type.Fúria;
                            price = prices[1];
                            break;
                    }
                }
                else
                {
                    type = Type.Vida;
                    price = prices[0];
                }
                break;

            case 1:
                type = Type.Fúria;
                price = prices[1];
                break;

            case 2:
                type = Type.Minério;
                price = prices[2];
                break;
        }
        if(r != 0 && Player.lifePoints <= Player.totalLifePoints/3)
        {
            type = Type.Vida;
            price = prices[0];
        }
        RandomSize();
    }

    IEnumerator SetVisualChestItem()
    {
        yield return new WaitForSeconds(2f);
        possibleVisuals[chestItemIndex].SetActive(true);
        isChestOpened = true;
    }

    void RandomSize()
    {
        if(type != Type.Runa_Coragem && type != Type.Runa_Fúria && type != Type._Vida)
        {
            int r = Random.Range(0, 3);
            if (r == 0)
            {
                size = Size.P;
            }
            else if (r == 1)
            {
                size = Size.M;
                price *= 2;
            }
            else if (r == 2)
            {
                size = Size.G;
                price *= 3;
            }
        }
        else
        {
            size = Size.x;
        }
    }

}
