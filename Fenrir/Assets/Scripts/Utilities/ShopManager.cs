using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public GameObject shopPanel;
    public Text productNameUI;
    public Text productPriceUI;
    public Player player;
    public GameObject errorMessage;
    CollectablePrize currentProduct;
    public void ShowShopWindow(CollectablePrize product)
    {
        shopPanel.SetActive(true);
        currentProduct = product;
        productNameUI.text = product.type.ToString() + " - " + product.size.ToString();
        if(Player.hasSkill[15]) //has discount
        {
            productPriceUI.text = (product.price / 2).ToString() + " Crystals";
        }
        else
        {
            productPriceUI.text = product.price.ToString() + " Crystals";
        }
    }

    public void CloseShopWindow()
    {
        shopPanel.SetActive(false);
        playerBuyingPosition = Vector3.zero;
    }

    Vector3 playerBuyingPosition;

    private void Update()
    {
        if(shopPanel.activeSelf)
        {
            if(playerBuyingPosition == Vector3.zero)
            {
                playerBuyingPosition = player.transform.position;
            }
            if(Vector3.Distance(playerBuyingPosition, player.transform.position) > 3f)
            {
                CloseShopWindow();
            }
        }
    }

    public void BuyProduct()
    {
        if (Player.hasSkill[15]) //has discount
        {
            if (Player.crystals >= currentProduct.price/2)
            {
                shopPanel.SetActive(false);
                player.UpdateCrystalCurrency(-currentProduct.price/2);
                player.GetCollectablePrizeEffect(currentProduct);
            }
            else
            {
                StartCoroutine(ShowError());
            }
        }
        else
        {
            if (Player.crystals >= currentProduct.price)
            {
                shopPanel.SetActive(false);
                player.UpdateCrystalCurrency(-currentProduct.price);
                player.GetCollectablePrizeEffect(currentProduct);
            }
            else
            {
                StartCoroutine(ShowError());
            }
        }

    }

    IEnumerator ShowError()
    {
        errorMessage.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        errorMessage.SetActive(false);
    }
}
