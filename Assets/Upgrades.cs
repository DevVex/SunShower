using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Upgrades : MonoBehaviour
{

    public MechanicsManager mechanicsManager;
    public PlayerController playerController;

    public int[] potPrices;
    public int[] speedPrices;
    public int[] balancePrices;

    public GameObject potLevels;
    public GameObject speedLevels;
    public GameObject balanceLevels;

    public SpriteRenderer balancer;
    public Sprite balancer1;
    public Sprite balancer2;
    public Sprite balancer3;

    public GameObject potPosition;
    public GameObject pot0;
    public GameObject pot1;
    public GameObject pot2;
    public GameObject pot3;

    public int potLevel;
    public int speedLevel;
    public int balanceLevel;

    public Button upgradePot;
    public Button upgradeSpeed;
    public Button upgradeBalance;

    public TextMeshProUGUI potPriceUi;
    public TextMeshProUGUI speedPriceUi;
    public TextMeshProUGUI balancePriceUi;

    public TextMeshProUGUI money;
    private float updateSpeedMoney = 0.001f;

    private void OnEnable()
    {

        Debug.Log("Pot Level: " + potLevel);
        Debug.Log("Pot Prices Length: " + potPrices.Length);
        money.text = mechanicsManager.money.ToString();
        UpdatePurchaseButtons();
        UpdatePot();
        UpdateBalancer();
        UpdatePrices();
        UpdateLevels();
        StartCoroutine(MoneyUpdater());
        playerController.SetSpeed(speedLevel);
    }

    private void UpdatePot()
    {
        foreach (Transform child in potPosition.transform)
        {
            Destroy(child.gameObject);
        }

        if (potLevel == 1)
        {
            Instantiate(pot1, potPosition.transform);
        }
        else if(potLevel == 2)
        {
            Instantiate(pot2, potPosition.transform);
        }
        else if (potLevel == 3)
        {
            Instantiate(pot3, potPosition.transform);
        }
        else
        {
            Instantiate(pot0, potPosition.transform);
        }
    }

    private void UpdateBalancer()
    {
        if (balanceLevel == 1)
        {
            balancer.sprite = balancer1;
        }
        else if (balanceLevel == 2)
        {
            balancer.sprite = balancer2;
        }
        else if (balanceLevel == 3)
        {
            balancer.sprite = balancer3;
        }
        else
        {
            balancer.sprite = null;
        }
    }

    private void UpdatePurchaseButtons()
    {
        upgradePot.interactable = false;
        upgradeSpeed.interactable = false;
        upgradeBalance.interactable = false;
        if (potLevel < potPrices.Length && potPrices[potLevel] <= mechanicsManager.money)
        {
            upgradePot.interactable = true;
        }

        if (speedLevel < speedPrices.Length && speedPrices[speedLevel] <= mechanicsManager.money)
        {
            upgradeSpeed.interactable = true;
        }

        if (balanceLevel < balancePrices.Length && balancePrices[balanceLevel] <= mechanicsManager.money)
        {
            upgradeBalance.interactable = true;
        }
    }

    private void UpdateLevels()
    {
        for(int i = 1; i <= potLevels.transform.childCount; i++)
        {
            if(potLevel >= i)
            {
                potLevels.transform.GetChild(i-1).gameObject.SetActive(true);
            }
            else
            {
                potLevels.transform.GetChild(i - 1).gameObject.SetActive(false);
            }
        }

        for (int i = 1; i <= speedLevels.transform.childCount; i++)
        {
            if (speedLevel >= i)
            {
                speedLevels.transform.GetChild(i - 1).gameObject.SetActive(true);
            }
            else
            {
                speedLevels.transform.GetChild(i - 1).gameObject.SetActive(false);
            }
        }

        for (int i = 1; i <= balanceLevels.transform.childCount; i++)
        {
            if (balanceLevel >= i)
            {
                balanceLevels.transform.GetChild(i - 1).gameObject.SetActive(true);
            }
            else
            {
                balanceLevels.transform.GetChild(i - 1).gameObject.SetActive(false);
            }
        }
    }

    private void UpdatePrices()
    {
        if(potLevel == potPrices.Length)
        {
            potPriceUi.text = "MAX";
        }
        else
        {
            potPriceUi.text = potPrices[potLevel].ToString();
        }

        if (speedLevel == speedPrices.Length)
        {
            speedPriceUi.text = "MAX";
        }
        else
        {
            speedPriceUi.text = speedPrices[speedLevel].ToString();
        }

        if (balanceLevel == balancePrices.Length)
        {
            balancePriceUi.text = "MAX";
        }
        else
        {
            balancePriceUi.text = balancePrices[balanceLevel].ToString();
        }
    }


    public void UpgradePot()
    {
        mechanicsManager.money -= potPrices[potLevel];
        potLevel++;
        UpdatePurchaseButtons();
        UpdateLevels();
        UpdatePrices();
        UpdatePot();
    }

    public void UpgradeSpeed()
    {
        mechanicsManager.money -= speedPrices[speedLevel];
        speedLevel++;
        UpdatePurchaseButtons();
        UpdateLevels();
        UpdatePrices();
        playerController.SetSpeed(speedLevel);
    }

    public void UpgradeBalance()
    {
        mechanicsManager.money -= balancePrices[balanceLevel];
        balanceLevel++;
        UpdatePurchaseButtons();
        UpdateLevels();
        UpdatePrices();
        UpdateBalancer();
    }


    private IEnumerator MoneyUpdater()
    {
        while (true)
        {
            if (int.Parse(money.text) > mechanicsManager.money)
            {
                if (int.Parse(money.text) - mechanicsManager.money > 1000)
                {
                    money.text = (int.Parse(money.text) - 100).ToString();
                }else if (int.Parse(money.text) - mechanicsManager.money > 100)
                {
                    money.text = (int.Parse(money.text) - 10).ToString();
                }else 
                {
                    money.text = (int.Parse(money.text) - 1).ToString();
                }
            }
            yield return new WaitForSeconds(updateSpeedMoney); // I used .2 secs but you can update it as fast as you want
        }
    }
}
